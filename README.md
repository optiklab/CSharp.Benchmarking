# CSharp.Benchmarking

Benchmarking same program on .NET 5, .NET 6, .NET 7, .NET 8 to find interesting observations in performance optimizations made on compiler level.
This repo also includes some examples of how to benchmark CPU usage for multithreaded applications and using Windows PDH API (aka Performance Counters API).

# Tests

- *IntegerSumCalculator* shows how to optimize simple Sum counter knowning concepts of CPU pipelining (conveyor), unsafe code and even SIMD coding
- *IntegerConditionalCountCalculator* shows how to optimize simple Counter with understanding of concept of CPU branch predictions and even SIMD coding
- *StringsBuilderTest* shows how StringBuilder helps to significantly optimize strings concatenation
- *FibonacciRecursiveTest* shows how amount of recoursive calls grows exponentially with N

# Observations found

- .NET team is using SIMD optimizations in .NET 7 and .NET 8, so the same LINQ code works WAY FASTER than on .NET 7 and even MORE FASTER on .NET 8

# .NET 6
BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3447/23H2/2023Update/SunValley3)
11th Gen Intel Core i9-11900H 2.50GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.101
    [Host]   : .NET 6.0.29 (6.0.2924.17105), X64 RyuJIT AVX2
    ShortRun : .NET 6.0.29 (6.0.2924.17105), X64 RyuJIT AVX2

| Method        | Mean        | Error     | StdDev    | Min         | Code Size |
|-------------- |------------:|----------:|----------:|------------:|----------:|
| EnumerableSum | 2,991.48 ns | 35.765 ns | 51.293 ns | 2,914.29 ns |     205 B |
| SumNative     |   398.70 ns |  5.051 ns |  7.244 ns |   391.04 ns |      68 B |
| SumSIMD       |    68.68 ns |  0.667 ns |  0.998 ns |    67.43 ns |     130 B |

| Method                               | Mean         | Error        | StdDev       | BranchMispredictions/Op |
|------------------------------------- |-------------:|-------------:|-------------:|------------------------:|
| GetProblemsCount                     |  7,100.24 ns |  1,232.81 ns |    67.574 ns |                     446 |
| GetProblemsCountNative               |    622.57 ns |    545.45 ns |    29.898 ns |                       2 |
| GetProblemsCountOrderedNative        | 63,793.64 ns | 16,361.53 ns |   896.831 ns |                   2,946 |
| GetProblemsCountPatternOrderedNative | 72,594.66 ns | 33,643.16 ns | 1,844.095 ns |                   3,515 |
| SumSIMD                              |     80.52 ns |     32.20 ns |     1.765 ns |                       1 |

| Method             | N  | Mean         | Error        | StdDev     | Allocated |
|------------------- |--- |-------------:|-------------:|-----------:|----------:|
| FibonacciRecursive | 5  |     11.72 ns |     6.127 ns |   0.336 ns |         - |
| FibonacciRecursive | 10 |    141.70 ns |    71.953 ns |   3.944 ns |         - |
| FibonacciRecursive | 20 | 18,013.15 ns | 9,771.963 ns | 535.634 ns |         - |

| Method                      | Mean       | Error      | StdDev     | Gen0         | Gen1         | Gen2         | Allocated   |
|---------------------------- |-----------:|-----------:|-----------:|-------------:|-------------:|-------------:|------------:|
| TestConcatWithStrings       | 928.517 ms | 18.3297 ms | 21.8202 ms | 3384000.0000 | 3355000.0000 | 3350000.0000 | 10961.77 MB |
| TestConcatWithStringBuilder |   1.426 ms |  0.0276 ms |  0.0359 ms |     332.0313 |     166.0156 |     166.0156 |     4.48 MB |

# .NET 7

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3447/23H2/2023Update/SunValley3)
11th Gen Intel Core i9-11900H 2.50GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.101
    [Host]   : .NET 7.0.18 (7.0.1824.16914), X64 RyuJIT AVX2
    ShortRun : .NET 7.0.18 (7.0.1824.16914), X64 RyuJIT AVX2

| Method        | Mean      | Error     | StdDev    | Min       | Code Size |
|-------------- |----------:|----------:|----------:|----------:|----------:|
| EnumerableSum | 354.55 ns | 14.566 ns | 21.350 ns | 335.23 ns |     315 B |
| SumNative     | 351.93 ns |  3.594 ns |  5.155 ns | 345.58 ns |      68 B |
| SumSIMD       |  60.64 ns |  0.512 ns |  0.751 ns |  59.34 ns |     135 B |

| Method                               | Mean         | Error        | StdDev       | BranchMispredictions/Op |
|------------------------------------- |-------------:|-------------:|-------------:|------------------------:|
| GetProblemsCount                     |  9,320.59 ns |  6,020.56 ns |   330.007 ns |                     490 |
| GetProblemsCountNative               |    630.31 ns |     84.58 ns |     4.636 ns |                       1 |
| GetProblemsCountOrderedNative        | 71,994.59 ns | 11,087.12 ns |   607.723 ns |                   3,229 |
| GetProblemsCountPatternOrderedNative | 74,971.32 ns | 35,333.55 ns | 1,936.751 ns |                   3,120 |
| SumSIMD                              |     85.39 ns |     98.51 ns |     5.400 ns |                       1 |

| Method             | N  | Mean         | Error       | StdDev     | Allocated |
|------------------- |--- |-------------:|------------:|-----------:|----------:|
| FibonacciRecursive | 5  |     10.48 ns |    13.60 ns |   0.746 ns |         - |
| FibonacciRecursive | 10 |    128.61 ns |    28.41 ns |   1.557 ns |         - |
| FibonacciRecursive | 20 | 16,167.85 ns | 2,643.48 ns | 144.898 ns |         - |

| Method                      | Mean       | Error      | StdDev     | Gen0         | Gen1         | Gen2         | Allocated   |
|---------------------------- |-----------:|-----------:|-----------:|-------------:|-------------:|-------------:|------------:|
| TestConcatWithStrings       | 945.926 ms | 12.9235 ms | 12.0886 ms | 3384000.0000 | 3356000.0000 | 3350000.0000 | 10961.77 MB |
| TestConcatWithStringBuilder |   1.330 ms |  0.0251 ms |  0.0235 ms |     332.0313 |     322.2656 |     166.0156 |     4.48 MB |

# .NET 8

Job=MediumRun-.NET 8.0-RyuJit-X64  Jit=RyuJit  Platform=X64
Runtime=.NET 8.0  IterationCount=15  LaunchCount=2
| Method        | Mean      | Error    | StdDev   | Min       | Code Size |
|-------------- |----------:|---------:|---------:|----------:|----------:|
| EnumerableSum |  81.63 ns | 1.983 ns | 2.780 ns |  78.11 ns |     869 B |
| SumNative     | 344.24 ns | 1.874 ns | 2.747 ns | 340.18 ns |      68 B |
| SumSIMD       |  54.17 ns | 0.417 ns | 0.584 ns |  53.21 ns |     136 B |
            
Job=ShortRun-.NET 8.0-RyuJit-X64  Jit=RyuJit  Platform=X64
Runtime=.NET 8.0  IterationCount=3  LaunchCount=1
| Method                               | Mean         | Error         | StdDev       | BranchMispredictions/Op |
|------------------------------------- |-------------:|--------------:|-------------:|------------------------:|
| GetProblemsCount                     |  1,494.72 ns |  1,546.905 ns |    84.791 ns |                       2 |
| GetProblemsCountNative               |    630.26 ns |     52.757 ns |     2.892 ns |                       1 |
| GetProblemsCountOrderedNative        | 36,887.01 ns |  2,933.775 ns |   160.810 ns |                   1,339 |
| GetProblemsCountPatternOrderedNative | 42,206.03 ns | 28,246.569 ns | 1,548.290 ns |                      NA |
| SumSIMD                              |     72.27 ns |      3.342 ns |     0.183 ns |                       1 |

| Method             | N  | Mean          | Error        | StdDev     | Allocated |
|------------------- |--- |--------------:|-------------:|-----------:|----------:|
| FibonacciRecursive | 5  |      9.451 ns |     2.126 ns |  0.1165 ns |         - |
| FibonacciRecursive | 10 |    127.021 ns |    22.148 ns |  1.2140 ns |         - |
| FibonacciRecursive | 20 | 15,882.424 ns | 1,439.537 ns | 78.9059 ns |         - |

| Method                      | Mean       | Error      | StdDev     | Gen0         | Gen1         | Gen2         | Allocated   |
|---------------------------- |-----------:|-----------:|-----------:|-------------:|-------------:|-------------:|------------:|
| TestConcatWithStrings       | 920.990 ms | 11.7740 ms | 11.0134 ms | 3384000.0000 | 3356000.0000 | 3350000.0000 | 10961.77 MB |
| TestConcatWithStringBuilder |   1.275 ms |  0.0130 ms |  0.0108 ms |     332.0313 |     322.2656 |     166.0156 |     4.48 MB |