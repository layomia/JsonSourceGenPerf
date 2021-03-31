using System;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Converters;
using System.Text.Json.Serialization.Metadata;

namespace Throughput.JsonSourceGeneration
{
    internal partial class JsonContext : JsonSerializerContext
    {
        private JsonValueInfo<System.String> _String;
        public JsonTypeInfo<System.String> String
        {
            get
            {
                if (_String == null)
                {
                    _String = new JsonValueInfo<System.String>(new StringConverter(), numberHandling: null, GetOptions());
                }

                return _String;
            }
        }
    }
}
