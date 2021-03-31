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
        private JsonTypeInfo<System.Int32> _Int32;
        public JsonTypeInfo<System.Int32> Int32
        {
            get
            {
                if (_Int32 == null)
                {
                    JsonSerializerOptions options = GetOptions();

                    JsonConverter customConverter;
                    if (options.Converters.Count > 0 && (customConverter = GetRuntimeProvidedCustomConverter(typeof(int), options)) != null)
                    {
                        _Int32 = new JsonValueInfo<System.Int32>(customConverter, numberHandling: null, options);
                    }
                    else
                    {
                        _Int32 = new JsonValueInfo<System.Int32>(new Int32Converter(), numberHandling: null, options);
                    }
                }

                return _Int32;
            }
        }
    }
}
