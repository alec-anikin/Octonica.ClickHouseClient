```

BenchmarkDotNet v0.13.11, Windows 10 (10.0.19045.3803/22H2/2022Update)
11th Gen Intel Core i7-1185G7 3.00GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 6.0.25 (6.0.2523.51912), X64 RyuJIT AVX2
  DefaultJob : .NET 6.0.25 (6.0.2523.51912), X64 RyuJIT AVX2


```
| Method         | Mean     | Error   | StdDev  | Allocated  |
|--------------- |---------:|--------:|--------:|-----------:|
| Array          | 102.6 ms | 1.61 ms | 1.50 ms |  200.64 KB |
| Memory         | 101.9 ms | 1.52 ms | 1.35 ms | 1169.16 KB |
| ReadonlyMemory | 101.9 ms | 1.68 ms | 1.57 ms | 1169.83 KB |
