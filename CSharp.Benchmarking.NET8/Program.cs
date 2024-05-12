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
             */





            //var summary = BenchmarkRunner.Run<IntegerSumCalculator>();

            /*
            */
        }
    }
}
