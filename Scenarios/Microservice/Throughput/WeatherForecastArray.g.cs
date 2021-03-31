using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Converters;
using System.Text.Json.Serialization.Metadata;
using Throughput;

namespace Throughput.JsonSourceGeneration
{
    internal partial class JsonContext : JsonSerializerContext
    {
        private JsonCollectionTypeInfo<Throughput.WeatherForecast[]> _WeatherForecastArray;
        public JsonTypeInfo<Throughput.WeatherForecast[]> WeatherForecastArray
        {
            get
            {
                if (_WeatherForecastArray == null)
                {
                    _WeatherForecastArray = KnownCollectionTypeInfos<Throughput.WeatherForecast>.GetArray(this.WeatherForecast, this, numberHandling: null);
                }
      
                return _WeatherForecastArray;
            }
        }
    }
}
