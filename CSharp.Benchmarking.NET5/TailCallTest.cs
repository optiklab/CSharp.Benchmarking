using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Text;

namespace CSharp.Benchmarking.NET5
{
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