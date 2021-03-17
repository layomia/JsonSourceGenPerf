#if RUNNING_CRANK
using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.Crank.EventSources;
#endif

namespace Runner
{
    class Program
    {
        static void Main(string[] args)
        {
#if RUNNING_CRANK
            Console.WriteLine("Application started.");
            Stopwatch sw = new();
            sw.Start();
#endif
            SerializationMechanism.RunBenchmark();
#if RUNNING_CRANK
            sw.Stop();

            Process process = Process.GetCurrentProcess();
            process.Refresh();

            //Console.WriteLine(process.PrivateMemorySize64 / 1024);
            //Console.WriteLine(sw.ElapsedMilliseconds);

            BenchmarksEventSource.Register("runtime/private-bytes", Operations.First, Operations.First, "Private bytes (KB)", "Private bytes (KB)", "n0");
            BenchmarksEventSource.Measure("runtime/private-bytes", process.PrivateMemorySize64 / 1024);

            BenchmarksEventSource.Register("application/elapsed-time", Operations.First, Operations.First, "Elapsed time (ms)", "Elasped time (ms)", "n0");
            BenchmarksEventSource.Measure("application/elapsed-time", sw.ElapsedMilliseconds);

            //Thread.Sleep(2000);
#endif
        }
    }
}
