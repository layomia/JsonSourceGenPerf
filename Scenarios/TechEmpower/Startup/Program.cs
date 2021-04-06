using System;
using System.Diagnostics;
using System.Text.Json;
using System.IO;
using System.Buffers;

//#if SERIALIZER_NEW || SOURCE_GEN_DIRECT || SOURCE_GEN_DIRECT_BYTEARRAY
using System.Text.Json.Serialization.Metadata;
using System.Text.Json.SourceGeneration;
using Startup;
using Startup.JsonSourceGeneration;

[assembly: JsonSerializable(typeof(JsonMessage))]
//#endif

namespace Startup
{
    class Program
    {
        private static byte[] s_serialized = new byte[] { 123, 34, 109, 101, 115, 115, 97, 103, 101, 34, 58, 34, 72, 101, 108, 108, 111, 44, 32, 87, 111, 114, 108, 100, 33, 34, 125 };
        private static JsonMessage message = new JsonMessage { message = "Hello, World!" };

#if SERIALIZER_OLD
        private static JsonSerializerOptions s_oldPatternOptions = new();
#elif SERIALIZER_NEW || SOURCE_GEN_DIRECT || SOURCE_GEN_DIRECT_BYTEARRAY
        private static JsonTypeInfo<JsonMessage> s_newPatternMetadata = JsonContext.Default.JsonMessage;
#endif

        static void Main(string[] args)
        {
            Stopwatch sw = new();

            Process process = Process.GetCurrentProcess();
            process.Refresh();
            long oldPrivateBytes = process.PrivateMemorySize64 / 1024;

            sw.Start();

#if SERIALIZE
            RunSerializeBenchMark();
#else
            RunDeserializeBenchMark();
#endif

            sw.Stop();
            process.Refresh();

            long newPrivateBytes = process.PrivateMemorySize64 / 1024;
            Console.Write("Private bytes (KB): ");
            Console.WriteLine(newPrivateBytes - oldPrivateBytes);

            Console.Write("Elapsed time (ms):");
            Console.WriteLine(sw.ElapsedMilliseconds);
        }

        private static void RunSerializeBenchMark()
        {
#if SERIALIZER_OLD
            _ = JsonSerializer.SerializeToUtf8Bytes(message, s_oldPatternOptions);
#elif SERIALIZER_NEW
            _ = JsonSerializer.SerializeToUtf8Bytes(message, s_newPatternMetadata);
#elif SOURCE_GEN_DIRECT_BYTEARRAY
            var bw = new PooledByteBufferWriter(16 * 1024);
            using (var writer = new Utf8JsonWriter(bw))
            {
                s_newPatternMetadata.SerializeObject!(writer, message, null!);
            }
            _ = bw.WrittenMemory.Span.ToArray();
#elif SOURCE_GEN_DIRECT
            var bw = new MemoryStream();
            using (var writer = new Utf8JsonWriter(bw))
            {
                s_newPatternMetadata.SerializeObject!(writer, message, null!);
            }
#elif LOW_LEVEL_BYTEARRAY
            var bw = new PooledByteBufferWriter(16 * 1024);
            using (var writer = new Utf8JsonWriter(bw))
            {
                writer.WriteStartObject();
                writer.WriteString("message", message.message);
                writer.WriteEndObject();
            }
            _ = bw.WrittenMemory.Span.ToArray();
#else
            var bw = new MemoryStream();
            using (var writer = new Utf8JsonWriter(bw))
            {
                writer.WriteStartObject();
                writer.WriteString("message", message.message);
                writer.WriteEndObject();
            }
#endif
        }

        private static void RunDeserializeBenchMark()
        {
#if SERIALIZER_OLD
            _ = JsonSerializer.Deserialize<JsonMessage>(s_serialized, s_oldPatternOptions);
#elif SERIALIZER_NEW
            _ = JsonSerializer.Deserialize(s_serialized, s_newPatternMetadata);
#else
            JsonMessage m = new();
            Utf8JsonReader reader = new(s_serialized);
            reader.Read();
            reader.Read();

            if (reader.GetString() != "message")
            {
                throw new JsonException();
            }

            reader.Read();
            m.message = reader.GetString();
#endif
        }
    }

    public struct JsonMessage
    {
        public string message { get; set; }
    }
}