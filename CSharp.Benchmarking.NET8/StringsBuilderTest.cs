using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Text;

// See also a benchmarking of the StringBuilder with/without StringBuilderCache inside
// https://andrewlock.net/a-deep-dive-on-stringbuilder-part-5-reducing-allocations-by-caching-stringbuilders-with-stringbuildercache/ 
namespace CSharp.Benchmarking.NET8
{
    /*
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
