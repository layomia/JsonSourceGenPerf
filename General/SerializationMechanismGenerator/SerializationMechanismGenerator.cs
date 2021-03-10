using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace SerializationMechanismGenerator
{
    [Generator]
    public class SerializationMechanismGenerator : ISourceGenerator
    {
        private const string BenchmarkSerializationLogicFileName = "SerializationMechanism.cs";

        private const string InvalidSpecificationSourceString = @"using System;

namespace Runner
{
    internal static class SerializationMechanism
    {
        public static void RunBenchmark()
        {
            throw new InvalidOperationException(""Benchmark specification not valid. Please see Test.txt and adjust."");
        }
    }
}
";

        private static SourceText InvalidSepcificationSourceText = SourceText.From(InvalidSpecificationSourceString, Encoding.UTF8);

        public void Execute(GeneratorExecutionContext context)
        {
            //Debugger.Launch();
            AdditionalText file = context.AdditionalFiles.First();
            SourceText fileText = file.GetText(context.CancellationToken);

            TextLineCollection lines = fileText.Lines;
            Debug.Assert(lines.Count == 4);

            BenchmarkSpecification specification = new();
            specification.NumPocos = int.Parse(lines[0].ToString());
            specification.UseSerializationAttributes = bool.Parse(lines[1].ToString());
            specification.Process = (JsonProcess)Enum.Parse(typeof(JsonProcess), lines[2].ToString());
            specification.Processor = (JsonProcessor)Enum.Parse(typeof(JsonProcessor), lines[3].ToString());

            if (specification.UseSerializationAttributes &&
                (specification.Processor == JsonProcessor.Reader || specification.Processor == JsonProcessor.Writer))
            {
                context.AddSource(BenchmarkSerializationLogicFileName, InvalidSepcificationSourceText);
                return;
            }

            GenerateBenchmarkImplementation(context, specification);
        }

        private static void GenerateBenchmarkImplementation(in GeneratorExecutionContext context, in BenchmarkSpecification specification)
        {
            context.AddSource("TestClasses.cs", SourceText.From(
                GetTestClasses(specification.NumPocos, specification.UseSerializationAttributes, specification.Processor == JsonProcessor.MetadataSerializer),
                Encoding.UTF8));

            context.AddSource(BenchmarkSerializationLogicFileName, SourceText.From(
                GetSerializationLogic(specification),
                Encoding.UTF8));
        }

        private static string GetTestClasses(int numPocos, bool useSerializationAttributes, bool usingMetadataSerializer)
        {
            StringBuilder sb = new();
            sb.Append(@"using System;
using System.Text.Json.Serialization;
using Runner;
");

            if (usingMetadataSerializer)
            {
                for (int i = 0; i < numPocos; i++)
                {
                    sb.Append(@$"
[assembly: JsonSerializable(typeof(MyClass{i}))]");
                }
            }

            sb.Append(@"

namespace Runner
{");

            for (int i = 0; i < numPocos; i++)
            {
                sb.Append(@$"

    internal class MyClass{i}
    {{{DisplayAttribute("[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]")}
        public int MyInt {{ get; set; }} = 5;
        {DisplayAttribute("[JsonInclude]")}
        public string MyString {{ get; internal set; }} = ""Hello world"";
        {DisplayAttribute("[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)]")}
        public float MyFloat {{ get; set; }} = 5;
        {DisplayAttribute(@"[JsonPropertyName(""Bool"")]")}
        public bool MyBool {{ get; set; }} = true;
    }}");
            }

            sb.Append(@"
}
");

            string DisplayAttribute(string attribute) => useSerializationAttributes ? $"\n        {attribute}" : "\n";

            return sb.ToString();
        }

        private static string GetSerializationLogic(in BenchmarkSpecification specification)
        {
            
            StringBuilder sb = new();
            string jsonSourceGenNamespace = specification.Processor == JsonProcessor.MetadataSerializer
                ? "\nusing Runner.JsonSourceGeneration;"
                : "";

            sb.Append(@$"using System;
using System.Text.Json;{jsonSourceGenNamespace}

namespace Runner
{{
    internal static class SerializationMechanism
    {{
        public static void RunBenchmark()
        {{");

            switch (specification.Processor)
            {
                case JsonProcessor.DynamicSerializer:
                case JsonProcessor.MetadataSerializer:
                    {
                        sb.Append(GetSerializationLogicUsingJsonSerializer(specification));
                    }
                    break;
                case JsonProcessor.Reader:
                    {
                        Debug.Assert(specification.Process == JsonProcess.Read && !specification.UseSerializationAttributes);
                        sb.Append(GetSerializationLogicUsingUtf8JsonReader(specification));
                    }
                    break;
                case JsonProcessor.Writer:
                    {
                        Debug.Assert(specification.Process == JsonProcess.Write && !specification.UseSerializationAttributes);

                        sb.Append(@"
            JsonEncodedText MyIntText = JsonEncodedText.Encode(""MyInt"", encoder: null);
            JsonEncodedText MyStringText = JsonEncodedText.Encode(""MyString"", encoder: null);
            JsonEncodedText MyFloatText = JsonEncodedText.Encode(""MyFloat"", encoder: null);
            JsonEncodedText MyBoolText = JsonEncodedText.Encode(""MyBool"", encoder: null);
");

                        for (int i = 0; i < specification.NumPocos; i++)
                        {
                            string objInstanceVarName = $"obj{i}";

                            sb.Append(@$"
            using (PooledByteBufferWriter output = new(initialCapacity: 16384))
            {{
                using (Utf8JsonWriter writer = new(output))
                {{

                    MyClass{i} {objInstanceVarName} = new();
                    writer.WriteStartObject();
                    writer.WriteNumber(MyIntText, {objInstanceVarName}.MyInt);
                    writer.WriteString(MyStringText, {objInstanceVarName}.MyString);
                    writer.WriteNumber(MyFloatText, {objInstanceVarName}.MyFloat);
                    writer.WriteBoolean(MyBoolText, {objInstanceVarName}.MyBool);
                    writer.WriteEndObject();
                }}
            }}
");
                        }
                    }
                    break;
                default:
                    throw new NotSupportedException($"Processor {specification.Processor} is not supported.");
            }

            sb.Append(@"
        }
    }
}
");

            return sb.ToString();
        }

        private static string GetSerializationLogicUsingJsonSerializer(in BenchmarkSpecification specification)
        {
            int numPocos = specification.NumPocos;
            JsonProcess process = specification.Process;
            JsonProcessor processor = specification.Processor;
            bool useSerializationAttributes = specification.UseSerializationAttributes;

            Debug.Assert(processor == JsonProcessor.DynamicSerializer || processor == JsonProcessor.MetadataSerializer);

            StringBuilder sb = new();

            switch (process)
            {
                case JsonProcess.Read:
                    {
                        sb.Append(GetJsonToRead(useSerializationAttributes));

                        for (int i = 0; i < numPocos; i++)
                        {
                            sb.Append(@$"
            _ = JsonSerializer.Deserialize<MyClass{i}>(json{SerializerOverloadPostfix(i)}");
                        }
                    }
                    break;
                case JsonProcess.Write:
                    {
                        for (int i = 0; i < numPocos; i++)
                        {
                            sb.Append(@$"
            _ = JsonSerializer.SerializeToUtf8Bytes(new MyClass{i}(){SerializerOverloadPostfix(i)}");
                        }
                    }
                    break;
            }

            return sb.ToString();

            string SerializerOverloadPostfix(int pocoIndex) => processor == JsonProcessor.DynamicSerializer ? ");" : $", JsonContext.Instance.MyClass{pocoIndex});";
        }

        private static string GetSerializationLogicUsingUtf8JsonReader(in BenchmarkSpecification specification)
        {
            StringBuilder sb = new();

            sb.Append(GetJsonToRead(useSerializationAttributes: false));

            sb.Append(@"
            const ulong MyIntKey = 360288470256154957;
            const ulong MyStringKey = 607538940040411469;
            const ulong MyFloatKey = 537161386749753677;
            const ulong MyBoolKey = 432464790091364685;

            Utf8JsonReader reader;
");

            for (int i = 0; i < specification.NumPocos; i++)
            {
                string objInstanceVarName = $"obj{i}";

                sb.Append(@$"
            MyClass{i} {objInstanceVarName} = new();
            reader = new Utf8JsonReader(json);
            
            while (true)
            {{
                reader.Read();

                if (reader.TokenType == JsonTokenType.EndObject)
                {{
                    break;
                }}

                ReadOnlySpan<byte> propertyName = reader.ValueSpan;

                reader.Read();

                switch (Helper.GetKey(propertyName))
                {{
                    case MyIntKey:
                        {objInstanceVarName}.MyInt = reader.GetInt32();
                        break;
                    case MyStringKey:
                        {objInstanceVarName}.MyString = reader.GetString();
                        break;
                    case MyFloatKey:
                        {objInstanceVarName}.MyFloat = reader.GetSingle();
                        break;
                    case MyBoolKey:
                        {objInstanceVarName}.MyBool = reader.GetBoolean();
                        break;
                    default:
                        break;
                }}
            }}
");
            }

            return sb.ToString();
        }

        private static string GetJsonToRead(bool useSerializationAttributes)
        {
            StringBuilder sb = new();

            string json;

            //if (useSerializationAttributes)
            //{
            //    json = @"{""MyFloat"":5,""Bool"":true,""MyInt"":5,""MyString"":""Hello world""}";
            //}
            //else
            //{
            //    json = @"{""MyFloat"":5,""MyBool"":true,""MyInt"":5,""MyString"":""Hello world""}";
            //}

            json = @"{""MyInt"":5}";

            byte[] jsonAsByteArray = Encoding.UTF8.GetBytes(json);

            sb.Append(@$"
            byte[] json = new byte[] {{ ");

            foreach (byte @byte in jsonAsByteArray)
            {
                sb.Append($"{@byte}, ");
            }

            sb.Append("};");

            return sb.ToString();
        }

        public void Initialize(GeneratorInitializationContext context)
        {
        }

        private struct BenchmarkSpecification
        {
            public int NumPocos { get; set; }

            public bool UseSerializationAttributes { get; set; }

            public JsonProcess Process { get; set; }

            public JsonProcessor Processor { get; set; }
        }

        private enum JsonProcess
        {
            Read,
            Write,
        }

        private enum JsonProcessor
        {
            DynamicSerializer,
            MetadataSerializer,
            Reader,
            Writer,
        }
    }
}
