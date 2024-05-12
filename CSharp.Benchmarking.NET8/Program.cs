using BenchmarkDotNet.Running;

namespace CSharp.Benchmarking.NET8
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
             */





            //var summary = BenchmarkRunner.Run<IntegerSumCalculator>();

            /*
            */
        }
    }
}
