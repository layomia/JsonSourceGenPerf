using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Throughput;
using Throughput.JsonSourceGeneration;

//[assembly: JsonSerializable(typeof(WeatherForecast[]))]

namespace Throughput
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        }

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        internal static WeatherForecast[] Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }

    public class Serialize
    {
        private WeatherForecast[] _result;
        private JsonSerializerOptions _oldPatternOptions;
        private JsonContext _newPatternContext;

        [GlobalSetup]
        public void Setup()
        {
            _result = Program.Get();

            _oldPatternOptions = new JsonSerializerOptions();
            _newPatternContext = new JsonContext(new JsonSerializerOptions());
        }

        [Benchmark(Baseline = true)]
        public void SerializeOld()
        {
            _ = JsonSerializer.SerializeToUtf8Bytes(_result, _oldPatternOptions);
        }

        [Benchmark]
        public void SerializeNew()
        {
            _ = JsonSerializer.SerializeToUtf8Bytes(_result, _newPatternContext.WeatherForecastArray);
        }
    }

    public class Deserialize
    {
        private byte[] _serialized;
        private JsonSerializerOptions _oldPatternOptions;
        private JsonContext _newPatternContext;

        [GlobalSetup]
        public void Setup()
        {
            _serialized = JsonSerializer.SerializeToUtf8Bytes(Program.Get());

            _oldPatternOptions = new JsonSerializerOptions();
            _newPatternContext = new JsonContext(new JsonSerializerOptions());
        }

        [Benchmark(Baseline = true)]
        public void DeserializeOld()
        {
            _ = JsonSerializer.Deserialize<WeatherForecast[]>(_serialized, _oldPatternOptions);
        }

        [Benchmark]
        public void DeserializeNew()
        {
            _ = JsonSerializer.Deserialize(_serialized, _newPatternContext.WeatherForecastArray);
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
