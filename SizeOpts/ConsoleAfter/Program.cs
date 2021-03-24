using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using ConsoleAfter.JsonSourceGeneration;

[assembly: JsonSerializable(typeof(ConsoleAfter.MyClass))]

namespace ConsoleAfter
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] json = JsonSerializer.SerializeToUtf8Bytes(new MyClass { MyInt = 1, MyString = "Hello" }, JsonContext.Instance.MyClass);
            MyClass obj = JsonSerializer.Deserialize(json, JsonContext.Instance.MyClass);
            Console.WriteLine(obj.MyInt);
            Console.WriteLine(obj.MyString);
        }
    }

    public class MyClass
    {
        public int MyInt { get; set; }
        public string MyString { get; set; }
    }
}
