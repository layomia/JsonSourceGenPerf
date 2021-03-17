using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

namespace ResultProcessor
{
    class Program
    {
        private const string ResultsDir = "D:\\benchmarks\\JsonSourceGenPerf\\General\\Runner\\Results";

        static async System.Threading.Tasks.Task Main(string[] args)
        {
            StringBuilder sb = new();
            sb.AppendLine("# Results");
            
            string[] resultDirectories = Directory.GetDirectories(ResultsDir);
            foreach (string directory in resultDirectories)
            {
                sb.AppendLine();

                string[] dirComponents = directory.Split('\\');
                string specification = dirComponents[dirComponents.Length - 1];

                string[] specComponents = specification.Split("-");

                string numPocos = specComponents[0].Split("_")[1];
                string numProps = specComponents[1].Split("_")[1];
                bool useAttributes = bool.Parse(specComponents[2].Split("_")[1]);
                string process = specComponents[3].Split("_")[1];

                string processToDisplay = process == "Write"
                    ? "Serialize"
                    : "Deserialize";

                sb.AppendLine($"## {processToDisplay} | Num POCOs: {numPocos}, Num properties: {numProps}, Use attributes: {useAttributes}");

                sb.AppendLine();

                sb.AppendLine($"| Processor | Build time (ms) | Overhead start-up time (ms) | Serialization start-up time (ms) | Net start-up time (ms) | Private bytes (KB) |");
                sb.AppendLine($"|-|-|-|-|-|-|");

                string[] resultFiles = Directory.GetFiles(directory);

                foreach (string file in resultFiles)
                {
                    string[] fileComponents = file.Split('\\');
                    string processor = fileComponents[fileComponents.Length - 1];

                    string[] processorComponents = processor.Split('.');
                    string processorName = processorComponents[0];

                    using (FileStream stream = File.OpenRead(file))
                    {
                        using (JsonDocument dom = await JsonDocument.ParseAsync(stream))
                        {
                            JsonElement element = dom.RootElement.GetProperty("jobResults").GetProperty("jobs").GetProperty("application").GetProperty("results");
                            
                            try
                            {
                                string buildTime = GetDoubleAsStr(element.GetProperty("benchmarks/build-time").GetRawText());
                                string overheadStartupTime = GetDoubleAsStr(element.GetProperty("benchmarks/start-time").GetRawText());
                                string serializationStartupTime = GetDoubleAsStr(element.GetProperty("application/elapsed-time").GetRawText());
                                string netStartupTime = GetDoubleAsStr(element.GetProperty("application/net-start-time").GetRawText());
                                string privateBytes = GetDoubleAsStr(element.GetProperty("runtime/private-bytes").GetRawText());

                                sb.AppendLine($@"| {processorName} | {buildTime} | {overheadStartupTime} | {serializationStartupTime} | {netStartupTime} | {privateBytes} |");
                            }
                            catch
                            {
                                Console.WriteLine("Error parsing results:");
                                Console.WriteLine(file);
                                Console.WriteLine(element.GetRawText());
                                return;
                            }
                        }
                    }

                }
            }

            //Console.WriteLine(sb.ToString());
            await File.WriteAllTextAsync(Path.Combine(ResultsDir, "Summary.md"), sb.ToString());
        }

        private static string GetDoubleAsStr(string raw)
        {
            if (raw == @"""NaN""")
            {
                return "NaN";
            }

            return double.Parse(raw).ToString("n2");
        }
    }
}
