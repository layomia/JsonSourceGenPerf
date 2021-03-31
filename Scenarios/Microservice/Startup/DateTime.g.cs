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
        private JsonTypeInfo<System.DateTime> _DateTime;
        public JsonTypeInfo<System.DateTime> DateTime
        {
            get
            {
                if (_DateTime == null)
                {
                    JsonSerializerOptions options = GetOptions();

                    JsonConverter customConverter;
                    if (options.Converters.Count > 0 && (customConverter = GetRuntimeProvidedCustomConverter(typeof(DateTime), options)) != null)
                    {
                        _DateTime = new JsonValueInfo<System.DateTime>(customConverter, numberHandling: null, options);
                    }
                    else
                    {
                        _DateTime = new JsonValueInfo<System.DateTime>(new DateTimeConverter(), numberHandling: null, options);
                    }
                }

                return _DateTime;
            }
        }
    }
}
