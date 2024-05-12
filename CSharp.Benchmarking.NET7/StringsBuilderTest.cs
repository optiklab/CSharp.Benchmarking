using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Text;

// See also a benchmarking of the StringBuilder with/without StringBuilderCache inside
// https://andrewlock.net/a-deep-dive-on-stringbuilder-part-5-reducing-allocations-by-caching-stringbuilders-with-stringbuildercache/ 
namespace CSharp.Benchmarking.NET7
{
    /*
    // * Summary *

    BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3447/23H2/2023Update/SunValley3)
    11th Gen Intel Core i9-11900H 2.50GHz, 1 CPU, 16 logical and 8 physical cores
    .NET SDK 8.0.101
      [Host]     : .NET 7.0.18 (7.0.1824.16914), X64 RyuJIT AVX2
      DefaultJob : .NET 7.0.18 (7.0.1824.16914), X64 RyuJIT AVX2


    | Method                      | Mean       | Error      | StdDev     | Gen0         | Gen1         | Gen2         | Allocated   |
    |---------------------------- |-----------:|-----------:|-----------:|-------------:|-------------:|-------------:|------------:|
    | TestConcatWithStrings       | 945.926 ms | 12.9235 ms | 12.0886 ms | 3384000.0000 | 3356000.0000 | 3350000.0000 | 10961.77 MB |
    | TestConcatWithStringBuilder |   1.330 ms |  0.0251 ms |  0.0235 ms |     332.0313 |     322.2656 |     166.0156 |     4.48 MB |

    // * Hints *
    Outliers
      StringsBuilderTest.TestConcatWithStringBuilder: Default -> 2 outliers were removed, 4 outliers were detected (1.28 ms, 1.28 ms, 1.38 ms, 1.42 ms)

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
