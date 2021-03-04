using System;
using System.Text.Json;
using Runner.JsonSourceGeneration;

namespace Runner
{
    internal static class SerializationMechanism
    {
        public static void RunBenchmark()
        {
            _ = JsonSerializer.Serialize(new MyClass0(), JsonContext.Instance.MyClass0);
            _ = JsonSerializer.Serialize(new MyClass1(), JsonContext.Instance.MyClass1);
            _ = JsonSerializer.Serialize(new MyClass2(), JsonContext.Instance.MyClass2);
            _ = JsonSerializer.Serialize(new MyClass3(), JsonContext.Instance.MyClass3);
            _ = JsonSerializer.Serialize(new MyClass4(), JsonContext.Instance.MyClass4);
        }
    }
}
