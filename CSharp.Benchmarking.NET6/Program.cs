using BenchmarkDotNet.Running;

namespace CSharp.Benchmarking.NET6
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
             */


            //var summary = BenchmarkRunner.Run<IntegerSumCalculator>();
            /*
            | Method        | Mean        | Error     | StdDev     | Min         | Code Size |
            |-------------- |------------:|----------:|-----------:|------------:|----------:|
            | EnumerableSum | 3,064.25 ns | 77.081 ns | 112.985 ns | 2,912.72 ns |     205 B |
            | SumNaive      |   415.45 ns |  9.059 ns |  13.279 ns |   393.81 ns |      68 B |
            | SumSIMD       |    68.48 ns |  0.714 ns |   1.001 ns |    67.41 ns |     130 B |
            */
        }
    }
}
