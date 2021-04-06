using System.Text.Json;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BenchmarkHelper;

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
        [Benchmark(Baseline = true)]
        public void SerializeOld()
        {
            _ = JsonSerializer.SerializeToUtf8Bytes(Helper.LoginViewModelInstance);
            _ = JsonSerializer.SerializeToUtf8Bytes(Helper.LocationInstance);
            _ = JsonSerializer.SerializeToUtf8Bytes(Helper.IndexViewModelInstance);
            _ = JsonSerializer.SerializeToUtf8Bytes(Helper.MyEventsListerViewModelInstance);
        }

        [Benchmark]
        public void SerializeNew()
        {
            _ = JsonSerializer.SerializeToUtf8Bytes(Helper.LoginViewModelInstance, Helper.LoginViewModelMetadata);
            _ = JsonSerializer.SerializeToUtf8Bytes(Helper.LocationInstance, Helper.LocationMetadata);
            _ = JsonSerializer.SerializeToUtf8Bytes(Helper.IndexViewModelInstance, Helper.IndexViewModelMetadata);
            _ = JsonSerializer.SerializeToUtf8Bytes(Helper.MyEventsListerViewModelInstance, Helper.MyEventsListerViewModelMetadata);
        }
    }

    public class Deserialize
    {
        private byte[] _loginViewModelUtf8Serialized;
        private byte[] _locationUtf8Serialized;
        private byte[] _indexViewModelUtf8Serialized;
        private byte[] _myEventsListerViewModelUtf8Serialized;

        [GlobalSetup]
        public void Setup()
        {
            _loginViewModelUtf8Serialized = JsonSerializer.SerializeToUtf8Bytes(Helper.LoginViewModelInstance);
            _locationUtf8Serialized = JsonSerializer.SerializeToUtf8Bytes(Helper.LocationInstance);
            _indexViewModelUtf8Serialized = JsonSerializer.SerializeToUtf8Bytes(Helper.IndexViewModelInstance);
            _myEventsListerViewModelUtf8Serialized = JsonSerializer.SerializeToUtf8Bytes(Helper.MyEventsListerViewModelInstance);
        }

        [Benchmark(Baseline = true)]
        public void DeserializeOld()
        {
            _ = JsonSerializer.Deserialize<LoginViewModel>(_loginViewModelUtf8Serialized);
            _ = JsonSerializer.Deserialize<Location>(_locationUtf8Serialized);
            _ = JsonSerializer.Deserialize<IndexViewModel>(_indexViewModelUtf8Serialized);
            _ = JsonSerializer.Deserialize<MyEventsListerViewModel>(_myEventsListerViewModelUtf8Serialized);
        }

        [Benchmark]
        public void DeserializeNew()
        {
            _ = JsonSerializer.Deserialize(_loginViewModelUtf8Serialized, Helper.LoginViewModelMetadata);
            _ = JsonSerializer.Deserialize(_locationUtf8Serialized, Helper.LocationMetadata);
            _ = JsonSerializer.Deserialize(_indexViewModelUtf8Serialized, Helper.IndexViewModelMetadata);
            _ = JsonSerializer.Deserialize(_myEventsListerViewModelUtf8Serialized, Helper.MyEventsListerViewModelMetadata);
        }
    }
}
