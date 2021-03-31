using System;
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
        private JsonObjectInfo<Throughput.WeatherForecast> _WeatherForecast;
        public JsonTypeInfo<Throughput.WeatherForecast> WeatherForecast
        {
            get
            {
                if (_WeatherForecast == null)
                {
                    _WeatherForecast = new(createObjectFunc: static () => new Throughput.WeatherForecast(), numberHandling: null, this.GetOptions());

                    _WeatherForecast.AddProperty(
                        clrPropertyName: "Date",
                        memberType: System.Reflection.MemberTypes.Property,
                        declaringType: typeof(Throughput.WeatherForecast),
                        classInfo: this.DateTime,
                        getter: static (obj) => { return ((Throughput.WeatherForecast)obj).Date; },
                        setter: static (obj, value) => { ((Throughput.WeatherForecast)obj).Date = value; },
                        jsonPropertyName: null,
                        ignoreCondition: null,
                        numberHandling: null);
                
                    _WeatherForecast.AddProperty(
                        clrPropertyName: "TemperatureC",
                        memberType: System.Reflection.MemberTypes.Property,
                        declaringType: typeof(Throughput.WeatherForecast),
                        classInfo: this.Int32,
                        getter: static (obj) => { return ((Throughput.WeatherForecast)obj).TemperatureC; },
                        setter: static (obj, value) => { ((Throughput.WeatherForecast)obj).TemperatureC = value; },
                        jsonPropertyName: null,
                        ignoreCondition: null,
                        numberHandling: null);
                
                    _WeatherForecast.AddProperty(
                        clrPropertyName: "TemperatureF",
                        memberType: System.Reflection.MemberTypes.Property,
                        declaringType: typeof(Throughput.WeatherForecast),
                        classInfo: this.Int32,
                        getter: static (obj) => { return ((Throughput.WeatherForecast)obj).TemperatureF; },
                        setter: null,
                        jsonPropertyName: null,
                        ignoreCondition: null,
                        numberHandling: null);
                
                    _WeatherForecast.AddProperty(
                        clrPropertyName: "Summary",
                        memberType: System.Reflection.MemberTypes.Property,
                        declaringType: typeof(Throughput.WeatherForecast),
                        classInfo: this.String,
                        getter: static (obj) => { return ((Throughput.WeatherForecast)obj).Summary; },
                        setter: static (obj, value) => { ((Throughput.WeatherForecast)obj).Summary = value; },
                        jsonPropertyName: null,
                        ignoreCondition: null,
                        numberHandling: null);
                
                    _WeatherForecast.CompleteInitialization(canBeDynamic: false);
                }

                return _WeatherForecast;
            }
        }
    }
}
