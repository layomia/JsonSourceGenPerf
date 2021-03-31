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
        private JsonValueInfo<System.Int32> _Int32;
        public JsonTypeInfo<System.Int32> Int32
        {
            get
            {
                if (_Int32 == null)
                {
                    _Int32 = new JsonValueInfo<System.Int32>(new Int32Converter(), numberHandling: null, GetOptions());
                }

                return _Int32;
            }
        }
    }
}
