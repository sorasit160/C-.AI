# Expense Tracker (สมุดบันทึกรายรับ-รายจ่าย)

<<<<<<< HEAD
ระบบบันทึกและบริหารรายรับ-รายจ่ายส่วนบุคคลแบบ Full-stack ครบวงจร พัฒนาด้วย **C# .NET 10**, **Entity Framework Core**, ฐานข้อมูล **PostgreSQL** และหน้าเว็บ **Modern Web UI (React + Vite)**

---

## สถาปัตยกรรมระบบ (Architecture)

```text
LearnDotNet/
├── Database (PostgreSQL)/       # ฐานข้อมูล: C# (Port 5432, Table: Expenses)
├── Backend/                     # RESTful Web API (ASP.NET Core Minimal API + EF Core)
│   ├── Program.cs               # API Endpoints (CRUD, Summary, Category, Search)
│   ├── Data/
│   │   └── AppDbContext.cs      # Entity Framework Core DbContext
│   ├── appsettings.json         # การตั้งค่า PostgreSQL Connection String
│   └── bin/Debug/net10.0/       # ไฟล์ Executable และผลลัพธ์การ Build
├── Frontend/                    # Modern Frontend Web UI (React + Vite)
│   ├── src/                     # Glassmorphism Dark UI, Stats Cards, Modal
│   ├── package.json             # NPM Dependencies
│   └── vite.config.js
└── ExpenseTracker/              # Console Application (C# .NET 10 เดิม)
    ├── Models/                  # Shared Models (Expense, Category)
    └── Services/
```

---

## ข้อมูลฐานข้อมูล (Database Specifications)

โปรเจกต์นี้ใช้งาน **PostgreSQL Database** เป็นฐานข้อมูลหลักสำหรับจัดเก็บข้อมูลอย่างถาวร:

- **DBMS**: PostgreSQL (Version 18)
- **Database Name**: `C#`
- **Host / Port**: `localhost:5432`
- **Default User**: `postgres`
- **Schema**: `public`
- **Table Name**: `Expenses`
- **ORM / Driver**: Entity Framework Core 10 (`Npgsql.EntityFrameworkCore.PostgreSQL`)
=======
แอปพลิเคชันคอนโซล (Console Application) สำหรับบันทึกและจัดการรายรับ-รายจ่ายส่วนบุคคล พัฒนาด้วยภาษา **C#** บน **.NET 10** พร้อมระบบจัดเก็บข้อมูลแบบไฟล์ JSON อัตโนมัติ

---

## ฟีเจอร์หลัก (Features)

<<<<<<< HEAD
- **หน้าเว็บ Web UI (React + Vite)**:
  - ธีมสี Dark Luxury สไตล์ Modern Glassmorphism
  - สรุปยอดเงินคงเหลือสุทธิ, รายรับรวม (+), และรายจ่ายรวม (-) แบบ Real-time
  - แถบแสดงสัดส่วนรายจ่ายแยกตามหมวดหมู่ (Progress Bar %)
  - สรุปสถิติยอดรายรับ-รายจ่ายรายเดือน
  - ค้นหารายการตาม Keyword ได้ทันที
  - Modal เพิ่มรายการใหม่ (สลับระหว่าง รายรับ/รายจ่าย ได้สะดวกรวดเร็ว)
  - ลบรายการพร้อมระบบถามยืนยัน
- **RESTful Web API (.NET 10 + EF Core)**:
  - ใช้ **Entity Framework Core (Npgsql)** เชื่อมต่อฐานข้อมูล **PostgreSQL**
  - มีระบบ Auto Migration / `EnsureCreated()` สร้างฐานข้อมูลและตารางให้อัตโนมัติเมื่อเริ่มต้นระบบ
  - ป้องกันข้อผิดพลาด Startup ด้วย Safe Initialization
  - รองรับ CORS ให้ Frontend ติดต่อได้ปลอดภัย
- **ฐานข้อมูลจริง (PostgreSQL)**:
  - เก็บข้อมูลในตาราง `Expenses` ถาวร ปลอดภัย รองรับการขยายตัวในอนาคต
=======
- **ดูรายการทั้งหมด**: แสดงประวัติรายการรายรับ-รายจ่าย เรียงตามวันที่ล่าสุด พร้อมไฮไลต์สี (เขียว = รายรับ, แดง = รายจ่าย)
- **เพิ่มรายรับ / รายจ่าย**: บันทึกรายละเอียด, จำนวนเงิน, วันที่ (เลือกใส่วันนี้หรือระบุวันที่ย้อนหลัง) และหมวดหมู่
- **ลบรายการ**: เลือกลบรายการที่ไม่ต้องการตาม ID
- **สรุปรายงานการเงิน**:
  - ยอดรวมรายรับ รายจ่าย และคงเหลือสุทธิ
  - สรุปรายจ่ายแยกตามหมวดหมู่ (เรียงจากมากไปน้อย)
  - สรุปภาพรวมรายรับ-รายจ่ายแยกตามเดือน
  - อันดับรายจ่ายสูงสุด 5 อันดับแรก
- **ค้นหารายการ**: ค้นหาตามคำสำคัญ (Keyword) ในรายละเอียดรายการ
- **จัดเก็บข้อมูลอัตโนมัติ (Persistence)**: ข้อมูลถูกบันทึกเป็นไฟล์ JSON อัตโนมัติใน Application Data ของผู้ใช้ ไม่สูญหายเมื่อปิดโปรแกรม

---

## หมวดหมู่ที่มีให้เลือก (Categories)

- `Food` (อาหาร)
- `Transport` (การเดินทาง)
- `Shopping` (ช้อปปิ้ง)
- `Bills` (บิล/ค่าน้ำ-ไฟ-โทรศัพท์)
- `Entertainment` (ความบันเทิง)
- `Salary` (เงินเดือน/รายได้)
- `Other` (อื่นๆ)

---

## ความต้องการของระบบ (Prerequisites)

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download) หรือใหม่กว่า
<<<<<<< HEAD
- [Node.js](https://nodejs.org/) (เวอร์ชัน 18 ขึ้นไป สำหรับรัน Frontend)
- [PostgreSQL](https://www.postgresql.org/download/) (เวอร์ชัน 14+ รันบนพอร์ต 5432)

---

## การตั้งค่าฐานข้อมูล (Database Configuration)

ตั้งค่า Connection String ในไฟล์ `Backend/appsettings.json` หรือ `Backend/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=C#;Username=postgres;Password=YOUR_POSTGRES_PASSWORD"
  }
}
=======
- ระบบปฏิบัติการ: Windows, macOS หรือ Linux

ตรวจสอบความพร้อมของ .NET SDK ในเครื่อง:
```bash
dotnet --version
```

---

<<<<<<< HEAD
## วิธีการรันโปรเจกต์ (How to Run)

### 1. รัน Backend Web API (.NET 10)
เปิด Terminal ที่ 1:
```powershell
cd d:\LearnDotNet\Backend
dotnet run --urls "http://localhost:5036"
```
> API จะทำงานอยู่ที่: `http://localhost:5036`

### 2. รัน Frontend Web UI (React + Vite)
เปิด Terminal ที่ 2:
```powershell
cd d:\LearnDotNet\Frontend
npm install      # ทำครั้งแรกเพื่อติดตั้งแพ็กเกจ
npm run dev
```
> หน้าเว็บจะเปิดใช้งานที่: `http://localhost:5173`

### 3. รัน Console Application (ทางเลือก)
หากต้องการทดสอบผ่าน Console เดิม:
```powershell
dotnet run --project ExpenseTracker
=======
## โครงสร้างโปรเจกต์ (Project Structure)

```text
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
```

---

<<<<<<< HEAD
## โครงสร้างตารางในฐานข้อมูล (Database Schema: `Expenses`)

```sql
-- DDL คำสั่งที่สร้างตารางจริงใน PostgreSQL (สร้างให้อัตโนมัติโดย EF Core)
CREATE TABLE "Expenses" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    "Date" timestamp with time zone NOT NULL,
    "Description" character varying(255) NOT NULL,
    "Amount" numeric(18,2) NOT NULL,
    "Category" text NOT NULL,
    "IsIncome" boolean NOT NULL
);
```

### คำอธิบายคอลัมน์ในตาราง `Expenses`

| ชื่อคอลัมน์ | ชนิดข้อมูล (Data Type) | Nullable | คำอธิบาย | ตัวอย่างข้อมูล |
|---|---|---|---|---|
| `Id` | `integer` | No (PK) | รหัสไอดีรายการ (Auto-increment) | `1` |
| `Date` | `timestamp with time zone` | No | วันและเวลาที่บันทึกรายการ | `2026-10-17 07:00:00+07` |
| `Description` | `character varying(255)` | No | รายละเอียดของรายการ / สินค้า | `มาม่า`, `Kao Pad` |
| `Amount` | `numeric(18,2)` | No | จำนวนเงิน (บาท) ทศนิยม 2 ตำแหน่ง | `15.00`, `70.00` |
| `Category` | `text` | No | หมวดหมู่ (`Food`, `Transport`, `Shopping`, `Bills`, `Entertainment`, `Salary`, `Other`) | `Food` |
| `IsIncome` | `boolean` | No | ประเภทรายการ (`true` = รายรับ, `false` = รายจ่าย) | `false` |

---

## วิธีเข้าดูข้อมูลในฐานข้อมูล (Database Inspection)

### 1. ผ่าน PowerShell / CLI
```powershell
$env:PGPASSWORD='YOUR_PASSWORD'; "SELECT * FROM ""Expenses"";" | & "C:\Program Files\PostgreSQL\18\bin\psql.exe" -U postgres -h localhost -p 5432 -d "C#"
```

### 2. ผ่าน pgAdmin 4
- **Servers** > **PostgreSQL 18** > **Databases** > **`Name Database`** > **Schemas** > **public** > **Tables** > **`Expenses`**

---

## API Endpoints หลัก

| Method | Endpoint | รายละเอียด |
|---|---|---|
| `GET` | `/api/expenses` | ดึงรายการทั้งหมดจาก DB (รองรับ `?q=คำค้น`) |
| `GET` | `/api/expenses/summary` | คำนวณสรุปยอดรวม, แยกหมวดหมู่, รายเดือน, Top 5 |
| `POST` | `/api/expenses` | บันทึกรายการใหม่ลงใน PostgreSQL |
| `DELETE` | `/api/expenses/{id}` | ลบรายการออกจาก PostgreSQL ตาม ID |
| `GET` | `/api/categories` | ดึงรายชื่อหมวดหมู่ทั้งหมด |
=======
## วิธีการติดตั้งและรันโปรแกรม (Getting Started)

1. **Clone หรือดาวน์โหลดโปรเจกต์** มาไว้ที่เครื่องของคุณ

2. **เปิด Terminal / Command Prompt** ไปที่โฟลเดอร์ของโปรเจกต์:
   ```powershell
   cd d:\LearnDotNet
   ```

3. **สั่งรันโปรแกรม**:
   ```powershell
   dotnet run --project ExpenseTracker
   ```

4. **การ Build โปรเจกต์**:
   ```powershell
   dotnet build ExpenseTracker
   ```

---

## ตำแหน่งที่จัดเก็บข้อมูล (Data Storage Location)

ข้อมูลรายรับ-รายจ่ายจะถูกบันทึกไว้ในรูปแบบ JSON ที่:
- **Windows**: `%AppData%\ExpenseTracker\expenses.json` (เช่น `C:\Users\<YourUser>\AppData\Roaming\ExpenseTracker\expenses.json`)

---

## การนำไปต่อยอด (Future Enhancements)

 ☑ เชื่อมต่อฐานข้อมูลจริง เช่น SQLite หรือ PostgreSQL (Entity Framework Core)
- [ ] Export ข้อมูลออกมาเป็นไฟล์ Excel (.xlsx) หรือ CSV
- [ ] พัฒนาต่อยอดเป็น RESTful Web API (ASP.NET Core)
  ☑ ทำหน้าเว็บ UI หรือ Desktop UI (Blazor / MAUI / React)
