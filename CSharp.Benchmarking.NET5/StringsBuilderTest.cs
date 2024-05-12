using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Text;

// See also a benchmarking of the StringBuilder with/without StringBuilderCache inside
// https://andrewlock.net/a-deep-dive-on-stringbuilder-part-5-reducing-allocations-by-caching-stringbuilders-with-stringbuildercache/ 
namespace CSharp.Benchmarking.NET5
{
    /* NOTEBOOK ACER ASPIRE 2014
     * 
     * // * Summary *
     * 
     * BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1682 (21H2)
     * Intel Core i7-4702MQ CPU 2.20GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
     * .NET SDK=6.0.202
     * [Host]     : .NET 5.0.16 (5.0.1622.16705), X64 RyuJIT  [AttachedDebugger]
     * DefaultJob : .NET 5.0.16 (5.0.1622.16705), X64 RyuJIT
     * 
     * |                      Method |         Mean |      Error |      StdDev |        Gen 0 |        Gen 1 |        Gen 2 | Allocated |
     * |---------------------------- |-------------:|-----------:|------------:|-------------:|-------------:|-------------:|----------:|
     * |       TestConcatWithStrings | 2,140.537 ms | 58.6966 ms | 173.0682 ms | 2962000.0000 | 2819000.0000 | 2819000.0000 | 10,969 MB |
     * | TestConcatWithStringBuilder |     2.712 ms |  0.0542 ms |   0.1360 ms |     816.4063 |     453.1250 |     164.0625 |      4 MB |
     * 
     * // * Hints *
     * Outliers
     *   StringsBuilderTest.TestConcatWithStrings: Default       -> 1 outlier  was  detected (1.71 s)
     *   StringsBuilderTest.TestConcatWithStringBuilder: Default -> 2 outliers were removed (3.14 ms, 3.15 ms)
     * 
     * // * Legends *
     *   Mean      : Arithmetic mean of all measurements
     *   Error     : Half of 99.9% confidence interval
     *   StdDev    : Standard deviation of all measurements
     *   Gen 0     : GC Generation 0 collects per 1000 operations
     *   Gen 1     : GC Generation 1 collects per 1000 operations
     *   Gen 2     : GC Generation 2 collects per 1000 operations
     *   Allocated : Allocated memory per single operation (managed only, inclusive, 1KB = 1024B)
     */

    [MemoryDiagnoser]
    public class StringsBuilderTest
    {
        [Benchmark]
        public string TestConcatWithStrings()
        {
            string strValue = "";
            for (var i=0; i < 50000; i++)
            {
                strValue = strValue + i + "";
            }
            return strValue;
        }

        [Benchmark]
        public string TestConcatWithStringBuilder()
        {
            StringBuilder sb = new StringBuilder();
            for (var i=0; i < 50000; i++)
            {
                sb.Append(i + " ");
            }
            return sb.ToString();
        }
    }
}
