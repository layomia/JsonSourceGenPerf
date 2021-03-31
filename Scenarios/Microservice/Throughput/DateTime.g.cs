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
        private JsonValueInfo<System.DateTime> _DateTime;
        public JsonTypeInfo<System.DateTime> DateTime
        {
            get
            {
                if (_DateTime == null)
                {
                    _DateTime = new JsonValueInfo<System.DateTime>(new DateTimeConverter(), numberHandling: null, GetOptions());
                }

                return _DateTime;
            }
        }
    }
}
