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
        private JsonTypeInfo<System.String> _String;
        public JsonTypeInfo<System.String> String
        {
            get
            {
                if (_String == null)
                {
                    JsonSerializerOptions options = GetOptions();

                    JsonConverter customConverter;
                    if (options.Converters.Count > 0 && (customConverter = GetRuntimeProvidedCustomConverter(typeof(string), options)) != null)
                    {
                        _String = new JsonValueInfo<System.String>(customConverter, numberHandling: null, options);
                    }
                    else
                    {
                        _String = new JsonValueInfo<System.String>(new StringConverter(), numberHandling: null, options);
                    }
                }

                return _String;
            }
        }
    }
}
