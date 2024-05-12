using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Text;

namespace CSharp.Benchmarking.NET6
{
    /*
    // * Summary *

    BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3447/23H2/2023Update/SunValley3)
    11th Gen Intel Core i9-11900H 2.50GHz, 1 CPU, 16 logical and 8 physical cores
    .NET SDK 8.0.101
      [Host]   : .NET 6.0.29 (6.0.2924.17105), X64 RyuJIT AVX2
      ShortRun : .NET 6.0.29 (6.0.2924.17105), X64 RyuJIT AVX2

    Job=ShortRun  IterationCount=3  LaunchCount=1
    WarmupCount=3

    | Method             | N  | Mean         | Error        | StdDev     | Allocated |
    |------------------- |--- |-------------:|-------------:|-----------:|----------:|
    | FibonacciRecursive | 5  |     11.72 ns |     6.127 ns |   0.336 ns |         - |
    | FibonacciRecursive | 10 |    141.70 ns |    71.953 ns |   3.944 ns |         - |
    | FibonacciRecursive | 20 | 18,013.15 ns | 9,771.963 ns | 535.634 ns |         - |

    // * Legends *
      N         : Value of the 'N' parameter
      Mean      : Arithmetic mean of all measurements
      Error     : Half of 99.9% confidence interval
      StdDev    : Standard deviation of all measurements
      Allocated : Allocated memory per single operation (managed only, inclusive, 1KB = 1024B)
      1 ns      : 1 Nanosecond (0.000000001 sec)
     */
    [ShortRunJob]
    [MemoryDiagnoser]
    public class TailCallTest
    {
        [Params(5, 10, 20)]
        public int N { get; set; }

        [Benchmark]
        public long FibonacciRecursive()
        {
            return FibonacciRecursiveHelper(N);
        }
        private long FibonacciRecursiveHelper(long n)
        {
            if (n < 3)
                return 1;
            return FibonacciRecursiveHelper(n-2) + FibonacciRecursiveHelper(n-1);
        }
    }
}