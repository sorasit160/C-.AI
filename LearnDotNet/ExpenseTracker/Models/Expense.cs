namespace ExpenseTracker.Models;

// Model: เก็บข้อมูลอย่างเดียว ไม่มี logic
public class Expense
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; } = "";
    public decimal Amount { get; set; }      // ใช้ decimal กับเงินเสมอ (float/double มีปัญหาทศนิยม)
    public Category Category { get; set; }
    public bool IsIncome { get; set; }       // true = รายรับ, false = รายจ่าย
}
