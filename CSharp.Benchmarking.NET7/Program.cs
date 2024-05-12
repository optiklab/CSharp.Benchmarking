using BenchmarkDotNet.Running;

namespace CSharp.Benchmarking.NET7
{
    /// <summary>
    /// dotnet new console CSharp.Benchmarking
    /// dotnet add package BenchmarkDotNet
    /// dotnet retore
    /// dotnet build
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run(typeof(Program).Assembly);

            /*
            BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3447/23H2/2023Update/SunValley3)
            11th Gen Intel Core i9-11900H 2.50GHz, 1 CPU, 16 logical and 8 physical cores
            .NET SDK 8.0.101
              [Host]   : .NET 7.0.18 (7.0.1824.16914), X64 RyuJIT AVX2

            Job=MediumRun-.NET 7.0-RyuJit-X64  Jit=RyuJit  Platform=X64
            Runtime=.NET 7.0  IterationCount=15  LaunchCount=2
            | Method        | Mean      | Error     | StdDev    | Min       | Code Size |
            |-------------- |----------:|----------:|----------:|----------:|----------:|
            | EnumerableSum | 354.55 ns | 14.566 ns | 21.350 ns | 335.23 ns |     315 B |
            | SumNative     | 351.93 ns |  3.594 ns |  5.155 ns | 345.58 ns |      68 B |
            | SumSIMD       |  60.64 ns |  0.512 ns |  0.751 ns |  59.34 ns |     135 B |

            Job=ShortRun-.NET 7.0-RyuJit-X64  Jit=RyuJit  Platform=X64
            Runtime=.NET 7.0  IterationCount=3  LaunchCount=1
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
             */

            //var summary = BenchmarkRunner.Run<IntegerSumCalculator>();

        }
    }
}
