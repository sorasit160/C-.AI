using System.Text.Json;
using System.Text.Json.Serialization;

namespace ExpenseTracker.Storage;

// Generic class: <T> คือชนิดข้อมูลอะไรก็ได้ เอาไปใช้ซ้ำกับโปรเจกต์อื่นได้
public class JsonFileStorage<T>
{
    private readonly string _path;

    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,                           // จัดรูปให้อ่านง่าย
        Converters = { new JsonStringEnumConverter() }  // เก็บ enum เป็นชื่อ เช่น "Food" แทนเลข
    };

    public JsonFileStorage(string path) => _path = path;

    public List<T> Load()
    {
        if (!File.Exists(_path)) return new();          // ยังไม่มีไฟล์ = เริ่มจากรายการว่าง

        try
        {
            var json = File.ReadAllText(_path);
            return JsonSerializer.Deserialize<List<T>>(json, Options) ?? new();
        }
        catch (JsonException)
        {
            Console.WriteLine("ไฟล์ข้อมูลเสียหาย เริ่มต้นด้วยรายการว่าง");
            return new();
        }
    }

    public void Save(List<T> items)
    {
        // สร้างโฟลเดอร์ให้ถ้ายังไม่มี
        Directory.CreateDirectory(Path.GetDirectoryName(_path)!);
        File.WriteAllText(_path, JsonSerializer.Serialize(items, Options));
    }
}
