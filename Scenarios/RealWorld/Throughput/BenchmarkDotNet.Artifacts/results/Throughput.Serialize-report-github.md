``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.867 (2004/?/20H1)
Intel Core i7-7700 CPU 3.60GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=6.0.100-preview.3.21153.9
  [Host] : .NET Core 6.0.0 (CoreCLR 6.0.21.15201, CoreFX 6.0.21.15201), X64 RyuJIT


```
|       Method | Mean | Error | Ratio | RatioSD |
|------------- |-----:|------:|------:|--------:|
| SerializeOld |   NA |    NA |     ? |       ? |
| SerializeNew |   NA |    NA |     ? |       ? |

Benchmarks with issues:
  Serialize.SerializeOld: DefaultJob
  Serialize.SerializeNew: DefaultJob
