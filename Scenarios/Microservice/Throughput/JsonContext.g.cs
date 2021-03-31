using System.Text.Json;
using System.Text.Json.Serialization;

namespace Throughput.JsonSourceGeneration
{
    internal partial class JsonContext : JsonSerializerContext
    {
        private static JsonContext s_default;
        public static JsonContext Default => s_default ??= new JsonContext();

        private JsonContext()
        {
            
        }

        public JsonContext(JsonSerializerOptions options) : base(options)
        {
            
        }

        
    }
}
