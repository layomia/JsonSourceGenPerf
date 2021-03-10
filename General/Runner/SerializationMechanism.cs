using System;
using System.Text.Json;
using Runner.JsonSourceGeneration;

namespace Runner
{
    internal static class SerializationMechanism
    {
        public static void RunBenchmark()
        {
            _ = JsonSerializer.SerializeToUtf8Bytes(new MyClass0(), JsonContext.Instance.MyClass0);
        }
    }
}
