using System;
using System.Text.Json;

namespace ConsoleBefore
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] json = JsonSerializer.SerializeToUtf8Bytes(new MyClass { MyInt = 1 });
            MyClass obj = JsonSerializer.Deserialize<MyClass>(json);
            Console.WriteLine(obj.MyInt);
        }
    }

    public class MyClass
    {
        public int MyInt { get; set; }
    }
}
