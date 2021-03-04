using System;
using System.Text.Json.Serialization;
using Runner;

[assembly: JsonSerializable(typeof(MyClass0))]
[assembly: JsonSerializable(typeof(MyClass1))]
[assembly: JsonSerializable(typeof(MyClass2))]
[assembly: JsonSerializable(typeof(MyClass3))]
[assembly: JsonSerializable(typeof(MyClass4))]

namespace Runner
{

    internal class MyClass0
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int MyInt { get; set; } = 5;
        
        [JsonInclude]
        public string MyString { get; internal set; } = "Hello world";
        
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
        public float MyFloat { get; set; } = 5;
        
        [JsonPropertyName("Bool")]
        public bool MyBool { get; set; } = true;
    }

    internal class MyClass1
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int MyInt { get; set; } = 5;
        
        [JsonInclude]
        public string MyString { get; internal set; } = "Hello world";
        
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
        public float MyFloat { get; set; } = 5;
        
        [JsonPropertyName("Bool")]
        public bool MyBool { get; set; } = true;
    }

    internal class MyClass2
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int MyInt { get; set; } = 5;
        
        [JsonInclude]
        public string MyString { get; internal set; } = "Hello world";
        
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
        public float MyFloat { get; set; } = 5;
        
        [JsonPropertyName("Bool")]
        public bool MyBool { get; set; } = true;
    }

    internal class MyClass3
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int MyInt { get; set; } = 5;
        
        [JsonInclude]
        public string MyString { get; internal set; } = "Hello world";
        
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
        public float MyFloat { get; set; } = 5;
        
        [JsonPropertyName("Bool")]
        public bool MyBool { get; set; } = true;
    }

    internal class MyClass4
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int MyInt { get; set; } = 5;
        
        [JsonInclude]
        public string MyString { get; internal set; } = "Hello world";
        
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]
        public float MyFloat { get; set; } = 5;
        
        [JsonPropertyName("Bool")]
        public bool MyBool { get; set; } = true;
    }
}
