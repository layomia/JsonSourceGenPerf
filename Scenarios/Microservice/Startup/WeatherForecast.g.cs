using Startup;
using System;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Converters;
using System.Text.Json.Serialization.Metadata;

namespace Startup.JsonSourceGeneration
{
    internal partial class JsonContext : JsonSerializerContext
    {
        private JsonTypeInfo<Startup.WeatherForecast> _WeatherForecast;
        public JsonTypeInfo<Startup.WeatherForecast> WeatherForecast
        {
            get
            {
                if (_WeatherForecast == null)
                {
                    JsonSerializerOptions options = GetOptions();

                    JsonConverter customConverter;
                    if (options.Converters.Count > 0 && (customConverter = GetRuntimeProvidedCustomConverter(typeof(Startup.WeatherForecast), options)) != null)
                    {
                        _WeatherForecast = new JsonValueInfo<Startup.WeatherForecast>(customConverter, numberHandling: null, options);
                    }
                    else
                    {
                        JsonObjectInfo<Startup.WeatherForecast> objectInfo = new(createObjectFunc: static () => new Startup.WeatherForecast(), numberHandling: null, this.GetOptions());
                        _WeatherForecast = objectInfo;
    
                        objectInfo.AddProperty(CreateProperty<System.DateTime>(
                            clrPropertyName: "Date",
                            memberType: System.Reflection.MemberTypes.Property,
                            declaringType: typeof(Startup.WeatherForecast),
                            classInfo: this.DateTime,
                            converter: this.DateTime.ConverterBase,
                            getter: static (obj) => { return ((Startup.WeatherForecast)obj).Date; },
                            setter: static (obj, value) => { ((Startup.WeatherForecast)obj).Date = value; },
                            jsonPropertyName: null,
                            ignoreCondition: null,
                            nameAsUtf8Bytes: new byte[] {68,97,116,101},
                            escapedNameSection: new byte[] {34,68,97,116,101,34,58},
                            numberHandling: null));
                    
                        objectInfo.AddProperty(CreateProperty<System.Int32>(
                            clrPropertyName: "TemperatureC",
                            memberType: System.Reflection.MemberTypes.Property,
                            declaringType: typeof(Startup.WeatherForecast),
                            classInfo: this.Int32,
                            converter: this.Int32.ConverterBase,
                            getter: static (obj) => { return ((Startup.WeatherForecast)obj).TemperatureC; },
                            setter: static (obj, value) => { ((Startup.WeatherForecast)obj).TemperatureC = value; },
                            jsonPropertyName: null,
                            ignoreCondition: null,
                            nameAsUtf8Bytes: new byte[] {84,101,109,112,101,114,97,116,117,114,101,67},
                            escapedNameSection: new byte[] {34,84,101,109,112,101,114,97,116,117,114,101,67,34,58},
                            numberHandling: null));
                    
                        objectInfo.AddProperty(CreateProperty<System.Int32>(
                            clrPropertyName: "TemperatureF",
                            memberType: System.Reflection.MemberTypes.Property,
                            declaringType: typeof(Startup.WeatherForecast),
                            classInfo: this.Int32,
                            converter: this.Int32.ConverterBase,
                            getter: static (obj) => { return ((Startup.WeatherForecast)obj).TemperatureF; },
                            setter: null,
                            jsonPropertyName: null,
                            ignoreCondition: null,
                            nameAsUtf8Bytes: new byte[] {84,101,109,112,101,114,97,116,117,114,101,70},
                            escapedNameSection: new byte[] {34,84,101,109,112,101,114,97,116,117,114,101,70,34,58},
                            numberHandling: null));
                    
                        objectInfo.AddProperty(CreateProperty<System.String>(
                            clrPropertyName: "Summary",
                            memberType: System.Reflection.MemberTypes.Property,
                            declaringType: typeof(Startup.WeatherForecast),
                            classInfo: this.String,
                            converter: this.String.ConverterBase,
                            getter: static (obj) => { return ((Startup.WeatherForecast)obj).Summary; },
                            setter: static (obj, value) => { ((Startup.WeatherForecast)obj).Summary = value; },
                            jsonPropertyName: null,
                            ignoreCondition: null,
                            nameAsUtf8Bytes: new byte[] {83,117,109,109,97,114,121},
                            escapedNameSection: new byte[] {34,83,117,109,109,97,114,121,34,58},
                            numberHandling: null));
                    
                        objectInfo.CompleteInitialization();
                    }
                }

                return _WeatherForecast;
            }
        }
    }
}
