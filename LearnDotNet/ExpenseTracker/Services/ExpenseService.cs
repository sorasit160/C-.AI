using ExpenseTracker.Models;
using ExpenseTracker.Storage;

namespace ExpenseTracker.Services;

// Service: logic ทั้งหมด + LINQ อยู่ที่นี่ ไม่ยุ่งกับการพิมพ์หน้าจอ
public class ExpenseService
{
    private readonly JsonFileStorage<Expense> _storage;
    private readonly List<Expense> _items;
    private int _nextId;

    public ExpenseService(JsonFileStorage<Expense> storage)
    {
        _storage = storage;
        _items = _storage.Load();
        _nextId = _items.Count == 0 ? 1 : _items.Max(e => e.Id) + 1;
    }

    public Expense Add(DateTime date, string description, decimal amount, Category category, bool isIncome)
    {
        var item = new Expense
        {
            Id = _nextId++,
            Date = date,
            Description = description,
            Amount = amount,
            Category = category,
            IsIncome = isIncome
        };
        _items.Add(item);
        _storage.Save(_items);      // บันทึกทุกครั้งที่มีการเปลี่ยนแปลง
        return item;
    }

    public bool Remove(int id)
    {
        var item = _items.FirstOrDefault(e => e.Id == id);
        if (item is null) return false;

        _items.Remove(item);
        _storage.Save(_items);
        return true;
    }

    // ---------- LINQ ----------

    // เรียงจากใหม่ไปเก่า
    public List<Expense> GetAll() =>
        _items.OrderByDescending(e => e.Date).ThenByDescending(e => e.Id).ToList();

    public decimal TotalIncome() =>
        _items.Where(e => e.IsIncome).Sum(e => e.Amount);

    public decimal TotalExpense() =>
        _items.Where(e => !e.IsIncome).Sum(e => e.Amount);

    // ยอดรวมรายจ่ายต่อหมวด (มากไปน้อย)
    public List<(Category Category, decimal Total)> ExpenseByCategory() =>
        _items
            .Where(e => !e.IsIncome)
            .GroupBy(e => e.Category)
            .Select(g => (Category: g.Key, Total: g.Sum(e => e.Amount)))
            .OrderByDescending(x => x.Total)
            .ToList();

    // สรุปรายเดือน
    public List<(int Year, int Month, decimal Income, decimal Expense)> MonthlySummary() =>
        _items
            .GroupBy(e => (e.Date.Year, e.Date.Month))
            .Select(g => (
                Year: g.Key.Year,
                Month: g.Key.Month,
                Income: g.Where(e => e.IsIncome).Sum(e => e.Amount),
                Expense: g.Where(e => !e.IsIncome).Sum(e => e.Amount)))
            .OrderBy(x => x.Year).ThenBy(x => x.Month)
            .ToList();

    // รายจ่ายสูงสุด n รายการ
    public List<Expense> TopExpenses(int count) =>
        _items
            .Where(e => !e.IsIncome)
            .OrderByDescending(e => e.Amount)
            .Take(count)
            .ToList();

    // ค้นหาจากคำในรายละเอียด (ไม่สนตัวพิมพ์เล็กใหญ่)
    public List<Expense> Search(string keyword) =>
        _items
            .Where(e => e.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(e => e.Date)
            .ToList();
}
