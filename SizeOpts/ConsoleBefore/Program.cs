using System;
using System.Text.Json;

namespace ConsoleBefore
{
    class Program
    {
        static void Main(string[] args)
        {
            MyClass obj = new MyClass { MyInt = 1, MyStrings = new[] { "Hello", "World" } };
            byte[] json = JsonSerializer.SerializeToUtf8Bytes(obj);
            obj = JsonSerializer.Deserialize<MyClass>(json);
            
            Console.WriteLine(obj.MyInt);
            Console.WriteLine(obj.MyStrings[0]);
            Console.WriteLine(obj.MyStrings[1]);
        }
    }

    public class MyClass
    {
        public int MyInt { get; set; }
        public string[] MyStrings { get; set; }
    }
}
