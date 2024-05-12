using BenchmarkDotNet.Attributes;
using System.Text;

// See also a benchmarking of the StringBuilder with/without StringBuilderCache inside
// https://andrewlock.net/a-deep-dive-on-stringbuilder-part-5-reducing-allocations-by-caching-stringbuilders-with-stringbuildercache/ 
namespace CSharp.Benchmarking.NET6
{
    /* NOTEBOOK HP OMEN 2022

        // * Summary *

        BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3447/23H2/2023Update/SunValley3)
        11th Gen Intel Core i9-11900H 2.50GHz, 1 CPU, 16 logical and 8 physical cores
        .NET SDK 8.0.101
          [Host]     : .NET 6.0.29 (6.0.2924.17105), X64 RyuJIT AVX2
          DefaultJob : .NET 6.0.29 (6.0.2924.17105), X64 RyuJIT AVX2


        | Method                      | Mean       | Error     | StdDev    | Gen0         | Gen1         | Gen2         | Allocated   |
        |---------------------------- |-----------:|----------:|----------:|-------------:|-------------:|-------------:|------------:|
        | TestConcatWithStrings       | 891.036 ms | 9.6164 ms | 8.0301 ms | 3384000.0000 | 3355000.0000 | 3350000.0000 | 10961.77 MB |
        | TestConcatWithStringBuilder |   1.410 ms | 0.0274 ms | 0.0293 ms |     332.0313 |     166.0156 |     166.0156 |     4.48 MB |

        // * Hints *
        Outliers
          StringsBuilderTest.TestConcatWithStrings: Default       -> 2 outliers were removed (929.28 ms, 939.42 ms)
          StringsBuilderTest.TestConcatWithStringBuilder: Default -> 2 outliers were removed (1.53 ms, 1.57 ms)

        // * Legends *
          Mean      : Arithmetic mean of all measurements
          Error     : Half of 99.9% confidence interval
          StdDev    : Standard deviation of all measurements
          Gen0      : GC Generation 0 collects per 1000 operations
          Gen1      : GC Generation 1 collects per 1000 operations
          Gen2      : GC Generation 2 collects per 1000 operations
          Allocated : Allocated memory per single operation (managed only, inclusive, 1KB = 1024B)
          1 ms      : 1 Millisecond (0.001 sec)
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
