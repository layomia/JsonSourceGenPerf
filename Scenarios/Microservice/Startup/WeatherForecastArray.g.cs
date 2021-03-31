using Startup;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Converters;
using System.Text.Json.Serialization.Metadata;

namespace Startup.JsonSourceGeneration
{
    internal partial class JsonContext : JsonSerializerContext
    {
        private JsonTypeInfo<Startup.WeatherForecast[]> _WeatherForecastArray;
        public JsonTypeInfo<Startup.WeatherForecast[]> WeatherForecastArray
        {
            get
            {
                if (_WeatherForecastArray == null)
                {
                    JsonSerializerOptions options = GetOptions();

                    JsonConverter customConverter;
                    if (options.Converters.Count > 0 && (customConverter = GetRuntimeProvidedCustomConverter(typeof(Startup.WeatherForecast[]), options)) != null)
                    {
                        _WeatherForecastArray = new JsonValueInfo<Startup.WeatherForecast[]>(customConverter, numberHandling: null, options);
                    }
                    else
                    {
                        _WeatherForecastArray = KnownCollectionTypeInfos<Startup.WeatherForecast>.GetArray(this.WeatherForecast, this, numberHandling: null);
                    }
                }

                return _WeatherForecastArray;
            }
        }
    }
}
