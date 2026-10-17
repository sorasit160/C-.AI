using System.Globalization;
using System.Text;
using ExpenseTracker.Models;
using ExpenseTracker.Services;
using ExpenseTracker.Storage;

// ให้แสดงภาษาไทยในคอนโซลได้
Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

// เก็บไฟล์ไว้ในโฟลเดอร์ข้อมูลแอปของผู้ใช้ ไม่ขึ้นกับว่ารันจากที่ไหน
var dataPath = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
    "ExpenseTracker",
    "expenses.json");

var service = new ExpenseService(new JsonFileStorage<Expense>(dataPath));

while (true)
{
    ShowMenu();
    var choice = Console.ReadLine()?.Trim();
    Console.WriteLine();

    switch (choice)
    {
        case "1": ShowList(service.GetAll()); break;
        case "2": AddItem(service, isIncome: false); break;
        case "3": AddItem(service, isIncome: true); break;
        case "4": RemoveItem(service); break;
        case "5": ShowSummary(service); break;
        case "6": SearchItems(service); break;
        case "0":
            Console.WriteLine("ลาก่อนครับ");
            return;
        default:
            Console.WriteLine("ไม่มีเมนูนี้ ลองใหม่อีกครั้ง");
            break;
    }
}

// ================= ฟังก์ชันแสดงผล =================

static void ShowMenu()
{
    Console.WriteLine();
    Console.WriteLine("===== สมุดรายรับรายจ่าย =====");
    Console.WriteLine("1) ดูรายการทั้งหมด");
    Console.WriteLine("2) เพิ่มรายจ่าย");
    Console.WriteLine("3) เพิ่มรายรับ");
    Console.WriteLine("4) ลบรายการ");
    Console.WriteLine("5) สรุปยอด");
    Console.WriteLine("6) ค้นหา");
    Console.WriteLine("0) ออก");
    Console.Write("เลือก: ");
}

static void ShowList(List<Expense> items)
{
    if (items.Count == 0)
    {
        Console.WriteLine("ยังไม่มีรายการ");
        return;
    }

    Console.WriteLine($"{"ID",-4} {"วันที่",-11} {"หมวด",-14} {"จำนวนเงิน",12}  รายละเอียด");
    Console.WriteLine(new string('-', 62));

    foreach (var e in items)
    {
        var sign = e.IsIncome ? "+" : "-";
        var color = e.IsIncome ? ConsoleColor.Green : ConsoleColor.Red;

        Console.ForegroundColor = color;
        Console.WriteLine(
            $"{e.Id,-4} {e.Date:yyyy-MM-dd} {e.Category,-14} {sign}{e.Amount,11:N2}  {e.Description}");
        Console.ResetColor();
    }
}

static void ShowSummary(ExpenseService service)
{
    var income = service.TotalIncome();
    var expense = service.TotalExpense();

    Console.WriteLine("--- ภาพรวม ---");
    Console.WriteLine($"รายรับรวม : {income,12:N2}");
    Console.WriteLine($"รายจ่ายรวม: {expense,12:N2}");
    Console.WriteLine($"คงเหลือ   : {income - expense,12:N2}");

    Console.WriteLine();
    Console.WriteLine("--- รายจ่ายแยกตามหมวด ---");
    foreach (var (category, total) in service.ExpenseByCategory())
        Console.WriteLine($"{category,-14} {total,12:N2}");

    Console.WriteLine();
    Console.WriteLine("--- สรุปรายเดือน ---");
    foreach (var m in service.MonthlySummary())
        Console.WriteLine($"{m.Year}-{m.Month:00}  รับ {m.Income,10:N2}  จ่าย {m.Expense,10:N2}");

    Console.WriteLine();
    Console.WriteLine("--- รายจ่ายสูงสุด 5 อันดับ ---");
    ShowList(service.TopExpenses(5));
}

// ================= ฟังก์ชันรับข้อมูล =================

static void AddItem(ExpenseService service, bool isIncome)
{
    Console.Write("รายละเอียด: ");
    var description = Console.ReadLine()?.Trim();
    if (string.IsNullOrEmpty(description))
    {
        Console.WriteLine("ต้องกรอกรายละเอียด ยกเลิกการเพิ่ม");
        return;
    }

    var amount = ReadDecimal("จำนวนเงิน: ");
    var date = ReadDate("วันที่ (yyyy-MM-dd, เว้นว่าง = วันนี้): ");
    var category = isIncome ? Category.Salary : ReadCategory();

    var item = service.Add(date, description, amount, category, isIncome);
    Console.WriteLine($"บันทึกแล้ว (ID {item.Id})");
}

static void RemoveItem(ExpenseService service)
{
    ShowList(service.GetAll());
    Console.Write("ใส่ ID ที่ต้องการลบ: ");

    if (!int.TryParse(Console.ReadLine(), out var id))
    {
        Console.WriteLine("ต้องเป็นตัวเลข");
        return;
    }

    Console.WriteLine(service.Remove(id) ? "ลบแล้ว" : "ไม่พบ ID นี้");
}

static void SearchItems(ExpenseService service)
{
    Console.Write("คำที่ต้องการค้นหา: ");
    var keyword = Console.ReadLine()?.Trim();
    if (string.IsNullOrEmpty(keyword)) return;

    ShowList(service.Search(keyword));
}

// ================= ฟังก์ชันช่วยอ่านค่า (วนจนกว่าจะกรอกถูก) =================

static decimal ReadDecimal(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        var text = Console.ReadLine();

        if (decimal.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out var value)
            && value > 0)
            return value;

        Console.WriteLine("กรุณากรอกตัวเลขที่มากกว่า 0");
    }
}

static DateTime ReadDate(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        var text = Console.ReadLine()?.Trim();

        if (string.IsNullOrEmpty(text)) return DateTime.Today;

        if (DateTime.TryParseExact(text, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var date))
            return date;

        Console.WriteLine("รูปแบบไม่ถูกต้อง ตัวอย่าง 2026-10-05");
    }
}

static Category ReadCategory()
{
    // เลือกเฉพาะหมวดรายจ่าย (ไม่รวม Salary)
    var options = Enum.GetValues<Category>().Where(c => c != Category.Salary).ToList();

    while (true)
    {
        Console.WriteLine("เลือกหมวด:");
        for (var i = 0; i < options.Count; i++)
            Console.WriteLine($"  {i + 1}) {options[i]}");
        Console.Write("เลือก: ");

        if (int.TryParse(Console.ReadLine(), out var n) && n >= 1 && n <= options.Count)
            return options[n - 1];

        Console.WriteLine("เลือกไม่ถูกต้อง");
    }
}
