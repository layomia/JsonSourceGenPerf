using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.SourceGeneration;
using ConsoleAfter;
using ConsoleAfter.JsonSourceGeneration;

[assembly: JsonSerializable(typeof(MyClass))]
[assembly: JsonSerializable(typeof(Point))]

namespace ConsoleAfter
{
    class Program
    {
        static void Main(string[] args)
        {
            // Regular usage
            MyClass obj = new MyClass { MyInt = 1, MyStrings = new[] { "Hello", "World" } };
            byte[] json = JsonSerializer.SerializeToUtf8Bytes(obj, JsonContext.Default.MyClass);
            obj = JsonSerializer.Deserialize(json, JsonContext.Default.MyClass);

            Console.WriteLine(obj.MyInt);
            Console.WriteLine(obj.MyStrings[0]);
            Console.WriteLine(obj.MyStrings[1]);

            // Fast path usage
            using var ms = new MemoryStream();
            using var writer = new Utf8JsonWriter(ms);
            JsonContext.Default.Point.SerializeObject!(writer, new Point { X = 1, Y = 2 }, options: null!);
        }
    }

    public class MyClass
    {
        public int MyInt { get; set; }
        public string[] MyStrings { get; set; }
    }

    public struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
