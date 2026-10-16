#C .AI
Expense Tracker (สมุดบันทึกรายรับ-รายจ่าย)
แอปพลิเคชันคอนโซล (Console Application) สำหรับบันทึกและจัดการรายรับ-รายจ่ายส่วนบุคคล พัฒนาด้วยภาษา C# บน .NET 10 พร้อมระบบจัดเก็บข้อมูลแบบไฟล์ JSON อัตโนมัติ

ฟีเจอร์หลัก (Features)
ดูรายการทั้งหมด: แสดงประวัติรายการรายรับ-รายจ่าย เรียงตามวันที่ล่าสุด พร้อมไฮไลต์สี (เขียว = รายรับ, แดง = รายจ่าย)
เพิ่มรายรับ / รายจ่าย: บันทึกรายละเอียด, จำนวนเงิน, วันที่ (เลือกใส่วันนี้หรือระบุวันที่ย้อนหลัง) และหมวดหมู่
ลบรายการ: เลือกลบรายการที่ไม่ต้องการตาม ID
สรุปรายงานการเงิน:
ยอดรวมรายรับ รายจ่าย และคงเหลือสุทธิ
สรุปรายจ่ายแยกตามหมวดหมู่ (เรียงจากมากไปน้อย)
สรุปภาพรวมรายรับ-รายจ่ายแยกตามเดือน
อันดับรายจ่ายสูงสุด 5 อันดับแรก
ค้นหารายการ: ค้นหาตามคำสำคัญ (Keyword) ในรายละเอียดรายการ
จัดเก็บข้อมูลอัตโนมัติ (Persistence): ข้อมูลถูกบันทึกเป็นไฟล์ JSON อัตโนมัติใน Application Data ของผู้ใช้ ไม่สูญหายเมื่อปิดโปรแกรม
หมวดหมู่ที่มีให้เลือก (Categories)
Food (อาหาร)
Transport (การเดินทาง)
Shopping (ช้อปปิ้ง)
Bills (บิล/ค่าน้ำ-ไฟ-โทรศัพท์)
Entertainment (ความบันเทิง)
Salary (เงินเดือน/รายได้)
Other (อื่นๆ)
ความต้องการของระบบ (Prerequisites)
.NET 10.0 SDK หรือใหม่กว่า
ระบบปฏิบัติการ: Windows, macOS หรือ Linux
ตรวจสอบความพร้อมของ .NET SDK ในเครื่อง:

dotnet --version
โครงสร้างโปรเจกต์ (Project Structure)
LearnDotNet/
├── LearnDotNet.slnx
├── README.md
└── ExpenseTracker/
    ├── ExpenseTracker.csproj      # ไฟล์กำหนดค่าโปรเจกต์ (Target net10.0)
    ├── Program.cs                 # เมนู Console และการรับ-แสดงผลข้อมูล
    ├── Models/
    │   ├── Category.cs            # Enum หมวดหมู่รายการ
    │   └── Expense.cs             # Data Model สำหรับรายรับ-รายจ่าย
    ├── Services/
    │   └── ExpenseService.cs      # Business Logic และการประมวลผล LINQ
    └── Storage/
        └── JsonFileStorage.cs     # ระบบบันทึก/โหลดข้อมูลไฟล์ JSON
วิธีการติดตั้งและรันโปรแกรม (Getting Started)
Clone หรือดาวน์โหลดโปรเจกต์ มาไว้ที่เครื่องของคุณ

เปิด Terminal / Command Prompt ไปที่โฟลเดอร์ของโปรเจกต์:

cd d:\LearnDotNet
สั่งรันโปรแกรม:

dotnet run --project ExpenseTracker
การ Build โปรเจกต์:

dotnet build ExpenseTracker
ตำแหน่งที่จัดเก็บข้อมูล (Data Storage Location)
ข้อมูลรายรับ-รายจ่ายจะถูกบันทึกไว้ในรูปแบบ JSON ที่:

Windows: %AppData%\ExpenseTracker\expenses.json (เช่น C:\Users\<YourUser>\AppData\Roaming\ExpenseTracker\expenses.json)
การนำไปต่อยอด (Future Enhancements)
 เชื่อมต่อฐานข้อมูลจริง เช่น SQLite หรือ PostgreSQL (Entity Framework Core)
 Export ข้อมูลออกมาเป็นไฟล์ Excel (.xlsx) หรือ CSV
 พัฒนาต่อยอดเป็น RESTful Web API (ASP.NET Core)
 ทำหน้าเว็บ UI หรือ Desktop UI (Blazor / MAUI / React)
