using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Text.Json.SourceGeneration;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Throughput;
using Throughput.JsonSourceGeneration;

[assembly: JsonSerializable(typeof(JsonMessage))]

namespace Throughput
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        }
    }

    public class Serialize
    {
        private JsonMessage _result;
        private JsonSerializerOptions _oldPatternOptions;
        private JsonTypeInfo<JsonMessage> _newPatternInfo;

        private PooledByteBufferWriter _sourceGenBufferWriter;
        private MemoryStream _sourceGenMemoryStream;
        private Utf8JsonWriter _sourceGenWriterBufferWriter;
        private Utf8JsonWriter _sourceGenWriterMemoryStream;

        private PooledByteBufferWriter _writerBufferWriter;
        private MemoryStream _writerMemoryStream;
        private Utf8JsonWriter _writerWBufferWriter;
        private Utf8JsonWriter _writerWMemoryStream;

        [GlobalSetup]
        public void Setup()
        {
            _result = new JsonMessage { message = "Hello, World!" };

            _oldPatternOptions = new JsonSerializerOptions();
            _newPatternInfo = JsonContext.Default.JsonMessage;

            _sourceGenBufferWriter = new PooledByteBufferWriter(16 * 1024);
            _sourceGenMemoryStream = new MemoryStream();
            _sourceGenWriterBufferWriter = new Utf8JsonWriter(_sourceGenBufferWriter);
            _sourceGenWriterMemoryStream = new Utf8JsonWriter(_sourceGenMemoryStream);

            _writerBufferWriter = new PooledByteBufferWriter(16 * 1024);
            _writerMemoryStream = new MemoryStream();
            _writerWBufferWriter = new Utf8JsonWriter(_writerBufferWriter);
            _writerWMemoryStream = new Utf8JsonWriter(_writerMemoryStream);
        }

        [Benchmark(Baseline = true)]
        public void Serializer()
        {
            _ = JsonSerializer.SerializeToUtf8Bytes(_result, _oldPatternOptions);
        }

        [Benchmark]
        public void SourceGen()
        {
            _ = JsonSerializer.SerializeToUtf8Bytes(_result, _newPatternInfo);
        }

        [Benchmark]
        public void SourceGenDirectPooledWriter()
        {
            _newPatternInfo.SerializeObject!(_sourceGenWriterBufferWriter, _result, options: null);
            _sourceGenBufferWriter.Clear();
            _sourceGenWriterBufferWriter.Reset();
        }

        [Benchmark]
        public void Utf8JsonWriterPooledWriter()
        {
            _writerWBufferWriter.WriteStartObject();
            _writerWBufferWriter.WriteString("message", _result.message);
            _writerWBufferWriter.WriteEndObject();
            _writerBufferWriter.Clear();
            _writerWBufferWriter.Reset();
        }

        [Benchmark]
        public void SourceGenDirectMemoryStreamWriter()
        {
            _newPatternInfo.SerializeObject!(_sourceGenWriterMemoryStream, _result, options: null);
            _sourceGenMemoryStream.Position = 0;
            _sourceGenWriterMemoryStream.Reset();
        }

        [Benchmark]
        public void Utf8JsonWriterMemoryStreamWriter()
        {
            _writerWMemoryStream.WriteStartObject();
            _writerWMemoryStream.WriteString("message", _result.message);
            _writerWMemoryStream.WriteEndObject();
            _writerMemoryStream.Position = 0;
            _writerWMemoryStream.Reset();
        }
    }

    //public class Deserialize
    //{
    //    private byte[] _serialized;
    //    private JsonSerializerOptions _oldPatternOptions;
    //    private JsonContext _newPatternContext;

    //    [GlobalSetup]
    //    public void Setup()
    //    {
    //        _serialized = JsonSerializer.SerializeToUtf8Bytes(Program.Get());

    //        _oldPatternOptions = new JsonSerializerOptions();
    //        _newPatternContext = new JsonContext(new JsonSerializerOptions());
    //    }

    //    [Benchmark(Baseline = true)]
    //    public void DeserializeOld()
    //    {
    //        _ = JsonSerializer.Deserialize<WeatherForecast[]>(_serialized, _oldPatternOptions);
    //    }

    //    [Benchmark]
    //    public void DeserializeNew()
    //    {
    //        _ = JsonSerializer.Deserialize(_serialized, _newPatternContext.WeatherForecastArray);
    //    }
    //}

    public struct JsonMessage
    {
        public string message { get; set; }
    }
}
