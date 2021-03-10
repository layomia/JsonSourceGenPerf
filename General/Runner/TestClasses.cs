//using System;
using System.Text.Json.Serialization;
using Runner;

[assembly: JsonSerializable(typeof(MyClass0))]

namespace Runner
{

    internal class MyClass0
    {

        public int MyInt { get; set; } = 5;
        

        //public string MyString { get; internal set; } = "Hello world";
        

        //public float MyFloat { get; set; } = 5;
        

        //public bool MyBool { get; set; } = true;
    }
}
