using System.Text.Json.Serialization;
using Backend.Data;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// JSON Serialization
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// 1. Configure PostgreSQL Connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// 2. Auto-create database & tables on startup safely
using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.EnsureCreated();
        app.Logger.LogInformation("Database connected and initialized successfully.");
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "Could not initialize database on startup. Please check PostgreSQL connection.");
    }
}

app.UseCors("AllowAll");

var api = app.MapGroup("/api");

// 1. Get all expenses / search
api.MapGet("/expenses", async (AppDbContext db, string? q) =>
{
    var query = db.Expenses.AsNoTracking();

    if (!string.IsNullOrWhiteSpace(q))
    {
        query = query.Where(e => EF.Functions.ILike(e.Description, $"%{q}%"));
    }

    var list = await query
        .OrderByDescending(e => e.Date)
        .ThenByDescending(e => e.Id)
        .ToListAsync();

    return Results.Ok(list);
});

// 2. Summary stats
api.MapGet("/expenses/summary", async (AppDbContext db) =>
{
    var all = await db.Expenses.AsNoTracking().ToListAsync();

    var income = all.Where(e => e.IsIncome).Sum(e => e.Amount);
    var expense = all.Where(e => !e.IsIncome).Sum(e => e.Amount);
    var balance = income - expense;

    var byCategory = all
        .Where(e => !e.IsIncome)
        .GroupBy(e => e.Category)
        .Select(g => new { category = g.Key.ToString(), total = g.Sum(e => e.Amount) })
        .OrderByDescending(x => x.total)
        .ToList();

    var monthly = all
        .GroupBy(e => new { e.Date.Year, e.Date.Month })
        .Select(g => new
        {
            year = g.Key.Year,
            month = g.Key.Month,
            income = g.Where(e => e.IsIncome).Sum(e => e.Amount),
            expense = g.Where(e => !e.IsIncome).Sum(e => e.Amount)
        })
        .OrderBy(x => x.year)
        .ThenBy(x => x.month)
        .ToList();

    var topExpenses = all
        .Where(e => !e.IsIncome)
        .OrderByDescending(e => e.Amount)
        .Take(5)
        .ToList();

    return Results.Ok(new
    {
        totalIncome = income,
        totalExpense = expense,
        balance = balance,
        expenseByCategory = byCategory,
        monthlySummary = monthly,
        topExpenses = topExpenses
    });
});

// 3. Add new expense/income
api.MapPost("/expenses", async (AppDbContext db, CreateExpenseDto dto) =>
{
    if (string.IsNullOrWhiteSpace(dto.Description))
    {
        return Results.BadRequest(new { message = "Description is required." });
    }

    if (dto.Amount <= 0)
    {
        return Results.BadRequest(new { message = "Amount must be greater than zero." });
    }

    var item = new Expense
    {
        Date = dto.Date ?? DateTime.UtcNow,
        Description = dto.Description.Trim(),
        Amount = dto.Amount,
        Category = dto.Category,
        IsIncome = dto.IsIncome
    };

    db.Expenses.Add(item);
    await db.SaveChangesAsync();

    return Results.Created($"/api/expenses/{item.Id}", item);
});

// 4. Delete an item
api.MapDelete("/expenses/{id:int}", async (AppDbContext db, int id) =>
{
    var item = await db.Expenses.FindAsync(id);
    if (item is null)
    {
        return Results.NotFound(new { message = $"Expense with ID {id} not found." });
    }

    db.Expenses.Remove(item);
    await db.SaveChangesAsync();

    return Results.NoContent();
});

// 5. Get categories
api.MapGet("/categories", () =>
{
    var categories = Enum.GetValues<Category>()
        .Select(c => new { id = (int)c, name = c.ToString() });
    return Results.Ok(categories);
});

app.Run();

public record CreateExpenseDto(
    string Description,
    decimal Amount,
    Category Category,
    bool IsIncome,
    DateTime? Date = null
);
