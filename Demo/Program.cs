using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using JsonCodeGeneration;

[assembly: JsonSerializable(typeof(ConsoleAfter.MyClass))]

namespace ConsoleAfter
{
    class Program
    {
        static void Main(string[] args)
        {
            // Pass known type to serializer: type class info
            string json = JsonSerializer.Serialize(new MyClass { MyInt = 1 }, JsonContext.Instance.MyClass);
            MyClass obj = JsonSerializer.Deserialize(json, JsonContext.Instance.MyClass);
            Console.WriteLine(obj.MyInt);

            // Pass known type to serializer: JSON context
            json = JsonSerializer.Serialize(new MyClass { MyInt = 1 }, JsonContext.Instance);
            obj = JsonSerializer.Deserialize<MyClass>(json, JsonContext.Instance);
            Console.WriteLine(obj.MyInt);

            //// Random type passed to serializer
            json = JsonSerializer.Serialize(GetRandomObject(), JsonContext.Instance); // Throws NotSupportedException
            //object boxedObj = JsonSerializer.Deserialize(json, GetRandomType(), JsonContext.Instance);
        }

        private static object GetRandomObject()
        {
            return new MyUnknownClass();
        }

        private static Type GetRandomType()
        {
            return typeof(MyUnknownClass);
        }
    }

    public class MyClass
    {
        public int MyInt { get; set; }
    }

    public class MyUnknownClass
    {
        public string MyString { get; set; }
    }
}
