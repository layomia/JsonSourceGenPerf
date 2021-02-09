using System;

namespace ConsoleBefore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(new MyClass { MyInt = 1 }.MyInt);
        }
    }

    public class MyClass
    {
        public int MyInt { get; set; }
    }
}
