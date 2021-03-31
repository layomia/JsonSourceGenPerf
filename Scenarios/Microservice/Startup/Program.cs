//#define SERIALIZE
//#define SERIALIZER_NEW
using System;
using System.Diagnostics;
using System.Text.Json;
using System.Linq;
using System.Text.Json.Serialization.Metadata;
using System.Text.Json.SourceGeneration;
using Startup;
using Startup.JsonSourceGeneration;
using System.IO;

//[assembly: JsonSerializable(typeof(WeatherForecast[]))]

namespace Startup
{
    class Program
    {
        private static WeatherForecast[] s_data = Get();

        private static byte[] s_serialized = new byte[] { 91, 123, 34, 68, 97, 116, 101, 34, 58, 34, 50, 48, 50, 49, 45, 48, 51, 45, 50, 56, 84, 49, 57, 58, 48, 53, 58, 50, 54, 46, 51, 50, 48, 56, 50, 48, 50, 43, 48, 48, 58, 48, 48, 34, 44, 34, 84, 101, 109, 112, 101, 114, 97, 116, 117, 114, 101, 67, 34, 58, 50, 57, 44, 34, 84, 101, 109, 112, 101, 114, 97, 116, 117, 114, 101, 70, 34, 58, 56, 52, 44, 34, 83, 117, 109, 109, 97, 114, 121, 34, 58, 34, 83, 99, 111, 114, 99, 104, 105, 110, 103, 34, 125, 44, 123, 34, 68, 97, 116, 101, 34, 58, 34, 50, 48, 50, 49, 45, 48, 51, 45, 50, 57, 84, 49, 57, 58, 48, 53, 58, 50, 54, 46, 51, 50, 55, 49, 48, 56, 49, 43, 48, 48, 58, 48, 48, 34, 44, 34, 84, 101, 109, 112, 101, 114, 97, 116, 117, 114, 101, 67, 34, 58, 53, 49, 44, 34, 84, 101, 109, 112, 101, 114, 97, 116, 117, 114, 101, 70, 34, 58, 49, 50, 51, 44, 34, 83, 117, 109, 109, 97, 114, 121, 34, 58, 34, 83, 99, 111, 114, 99, 104, 105, 110, 103, 34, 125, 44, 123, 34, 68, 97, 116, 101, 34, 58, 34, 50, 48, 50, 49, 45, 48, 51, 45, 51, 48, 84, 49, 57, 58, 48, 53, 58, 50, 54, 46, 51, 50, 55, 49, 49, 48, 56, 43, 48, 48, 58, 48, 48, 34, 44, 34, 84, 101, 109, 112, 101, 114, 97, 116, 117, 114, 101, 67, 34, 58, 53, 52, 44, 34, 84, 101, 109, 112, 101, 114, 97, 116, 117, 114, 101, 70, 34, 58, 49, 50, 57, 44, 34, 83, 117, 109, 109, 97, 114, 121, 34, 58, 34, 83, 119, 101, 108, 116, 101, 114, 105, 110, 103, 34, 125, 44, 123, 34, 68, 97, 116, 101, 34, 58, 34, 50, 48, 50, 49, 45, 48, 51, 45, 51, 49, 84, 49, 57, 58, 48, 53, 58, 50, 54, 46, 51, 50, 55, 49, 49, 49, 49, 43, 48, 48, 58, 48, 48, 34, 44, 34, 84, 101, 109, 112, 101, 114, 97, 116, 117, 114, 101, 67, 34, 58, 45, 49, 57, 44, 34, 84, 101, 109, 112, 101, 114, 97, 116, 117, 114, 101, 70, 34, 58, 45, 50, 44, 34, 83, 117, 109, 109, 97, 114, 121, 34, 58, 34, 83, 99, 111, 114, 99, 104, 105, 110, 103, 34, 125, 44, 123, 34, 68, 97, 116, 101, 34, 58, 34, 50, 48, 50, 49, 45, 48, 52, 45, 48, 49, 84, 49, 57, 58, 48, 53, 58, 50, 54, 46, 51, 50, 55, 49, 49, 49, 51, 43, 48, 48, 58, 48, 48, 34, 44, 34, 84, 101, 109, 112, 101, 114, 97, 116, 117, 114, 101, 67, 34, 58, 50, 52, 44, 34, 84, 101, 109, 112, 101, 114, 97, 116, 117, 114, 101, 70, 34, 58, 55, 53, 44, 34, 83, 117, 109, 109, 97, 114, 121, 34, 58, 34, 70, 114, 101, 101, 122, 105, 110, 103, 34, 125, 93, };

#if SERIALIZER_OLD
        private static JsonSerializerOptions s_oldPatternOptions = new();
#elif SERIALIZER_NEW
        private static JsonTypeInfo<WeatherForecast[]> s_weatherForecastArrayMetadata = new JsonContext(JsonSerializerOptions.CreateForSizeOpts()).WeatherForecastArray;
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

        public static WeatherForecast[] Get()
        {
            string[] summaries = new[]
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = summaries[rng.Next(summaries.Length)]
            })
            .ToArray();
        }

        private static void RunSerializeBenchMark()
        {
#if SERIALIZER_OLD
            _ = JsonSerializer.SerializeToUtf8Bytes(s_data, s_oldPatternOptions);
#elif SERIALIZER_NEW
            _ = JsonSerializer.SerializeToUtf8Bytes(s_data, s_weatherForecastArrayMetadata);
#else
            var ms = new MemoryStream();
            using (var writer = new Utf8JsonWriter(ms))
            {
                writer.WriteStartArray();
                for (int i = 0; i < s_data.Length; i++)
                {
                    WeatherForecast w = s_data[i];
                    writer.WriteStartObject();
                    writer.WriteString("Date", w.Date);
                    writer.WriteNumber("TemperatureC", w.TemperatureC);
                    writer.WriteNumber("TemperatureF", w.TemperatureF);
                    writer.WriteString("Summary", w.Summary);
                    writer.WriteEndObject();
                }
                writer.WriteEndArray();
            }
#endif
        }

        private static void RunDeserializeBenchMark()
        {
#if SERIALIZER_OLD
            _ = JsonSerializer.Deserialize<WeatherForecast[]>(s_serialized, s_oldPatternOptions);
#elif SERIALIZER_NEW
            _ = JsonSerializer.Deserialize(s_serialized, s_weatherForecastArrayMetadata);
#endif
        }
    }

    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}