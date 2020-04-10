# Instructions

Run benchmarks
```
src/benchmark/dotnet run -c release -- --filter *_create_schema

src/benchmark/dotnet run -c release -- --filter *_query
```


## Latest

``` ini

BenchmarkDotNet=v0.12.0, OS=Windows 10.0.19041
Intel Core i7-8650U CPU 1.90GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.300-preview-015048
  [Host]     : .NET Core 3.1.3 (CoreCLR 4.700.20.11803, CoreFX 4.700.20.12001), X64 RyuJIT
  DefaultJob : .NET Core 3.1.3 (CoreCLR 4.700.20.11803, CoreFX 4.700.20.12001), X64 RyuJIT


```
|               Method |     Mean |    Error |   StdDev | Ratio |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
|--------------------- |---------:|---------:|---------:|------:|-------:|-------:|------:|----------:|
|  tanka_graphql_query | 43.90 us | 0.492 us | 0.460 us |  0.63 | 7.5684 |      - |     - |  31.12 KB |
| graphql_dotnet_query | 69.32 us | 1.176 us | 1.042 us |  1.00 | 6.5918 | 0.1221 |     - |  27.52 KB |
