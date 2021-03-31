using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Startup.JsonSourceGeneration
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

        private static JsonConverter GetRuntimeProvidedCustomConverter(System.Type type, JsonSerializerOptions options)
        {
            System.Collections.Generic.IList<JsonConverter> converters = options.Converters;

            for (int i = 0; i < converters.Count; i++)
            {
                JsonConverter converter = converters[i];

                if (converter.CanConvert(type))
                {
                    if (converter is JsonConverterFactory factory)
                    {
                        converter = factory.CreateConverter(type, options);
                        if (converter == null || converter is JsonConverterFactory)
                        {
                            throw new System.InvalidOperationException($"The converter '{factory.GetType()}' cannot return null or a JsonConverterFactory instance.");
                        }
                    }

                    return converter;
                }
            }

            return null;
        }

        public JsonPropertyInfo<TProperty> CreateProperty<TProperty>(
                string clrPropertyName,
                System.Reflection.MemberTypes memberType,
                System.Type declaringType,
                JsonTypeInfo<TProperty> classInfo,
                JsonConverter converter,
                System.Func<object, TProperty> getter,
                System.Action<object, TProperty> setter,
                string jsonPropertyName,
                byte[] nameAsUtf8Bytes,
                byte[] escapedNameSection,
                JsonIgnoreCondition? ignoreCondition,
                JsonNumberHandling? numberHandling)
        {
            JsonSerializerOptions options = GetOptions();
            JsonPropertyInfo<TProperty> jsonPropertyInfo = JsonPropertyInfo<TProperty>.Create();
            jsonPropertyInfo.Options = options;
            // Property name settings.
            // TODO: consider whether we need to examine options.Encoder here as well.
            if (options.PropertyNamingPolicy == null && nameAsUtf8Bytes != null && escapedNameSection != null)
            {
                jsonPropertyInfo.NameAsString = jsonPropertyName ?? clrPropertyName;
                jsonPropertyInfo.NameAsUtf8Bytes = nameAsUtf8Bytes;
                jsonPropertyInfo.EscapedNameSection = escapedNameSection;
            }
            else
            {
                jsonPropertyInfo.NameAsString = jsonPropertyName
                    ?? options.PropertyNamingPolicy?.ConvertName(clrPropertyName)
                    ?? (options.PropertyNamingPolicy == null
                            ? null
                            : throw new System.InvalidOperationException("TODO: PropertyNamingPolicy cannot return null."));
                // NameAsUtf8Bytes and EscapedNameSection will be set in CompleteInitialization() below.
            }
            if (ignoreCondition != JsonIgnoreCondition.Always)
            {
                jsonPropertyInfo.Get = getter;
                jsonPropertyInfo.Set = setter;
                jsonPropertyInfo.ConverterBase = converter ?? throw new System.NotSupportedException("TODO: need custom converter here?");
                jsonPropertyInfo.RuntimeClassInfo = classInfo;
                jsonPropertyInfo.DeclaredPropertyType = typeof(TProperty);
                jsonPropertyInfo.DeclaringType = declaringType;
                jsonPropertyInfo.IgnoreCondition = ignoreCondition;
                jsonPropertyInfo.MemberType = memberType;
            }
            jsonPropertyInfo.CompleteInitialization();
            return jsonPropertyInfo;
        }
    }
}
