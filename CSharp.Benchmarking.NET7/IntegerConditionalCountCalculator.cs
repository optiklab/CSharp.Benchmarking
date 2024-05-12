using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using System;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;

namespace CSharp.Benchmarking.NET7
{
    /*
    // * Summary *

    BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3447/23H2/2023Update/SunValley3)
    11th Gen Intel Core i9-11900H 2.50GHz, 1 CPU, 16 logical and 8 physical cores
    .NET SDK 8.0.101
      [Host]                       : .NET 7.0.18 (7.0.1824.16914), X64 RyuJIT AVX2
      ShortRun-.NET 7.0-RyuJit-X64 : .NET 7.0.18 (7.0.1824.16914), X64 RyuJIT AVX2

    Job=ShortRun-.NET 7.0-RyuJit-X64  Jit=RyuJit  Platform=X64
    Runtime=.NET 7.0  IterationCount=3  LaunchCount=1
    WarmupCount=3

    | Method                               | Mean         | Error        | StdDev       | BranchMispredictions/Op |
    |------------------------------------- |-------------:|-------------:|-------------:|------------------------:|
    | GetProblemsCount                     |  9,320.59 ns |  6,020.56 ns |   330.007 ns |                     490 |
    | GetProblemsCountNative               |    630.31 ns |     84.58 ns |     4.636 ns |                       1 |
    | GetProblemsCountOrderedNative        | 71,994.59 ns | 11,087.12 ns |   607.723 ns |                   3,229 |
    | GetProblemsCountPatternOrderedNative | 74,971.32 ns | 35,333.55 ns | 1,936.751 ns |                   3,120 |
    | SumSIMD                              |     85.39 ns |     98.51 ns |     5.400 ns |                       1 |

    // * Legends *
      Mean                    : Arithmetic mean of all measurements
      Error                   : Half of 99.9% confidence interval
      StdDev                  : Standard deviation of all measurements
      BranchMispredictions/Op : Hardware counter 'BranchMispredictions' per single operation
      1 ns                    : 1 Nanosecond (0.000000001 sec)
     */

    [HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.BranchMispredictions)]
    [ShortRunJob(RuntimeMoniker.Net70, Jit.RyuJit, Platform.X64)]
    public class IntegerConditionalCountCalculator
    {
        private const int SIZE = 1000;
        private const int MAX_VALUE = 10;
        private const int TARGET = 5;

        private int[] _array;

        public IntegerConditionalCountCalculator()
        {
            _array = new int[SIZE];
            Random random = new();

            for (int i = 0; i < _array.Length; i++)
            {
                _array[i] = random.Next(MAX_VALUE);
            }
        }

        [Benchmark]
        public int GetProblemsCount() => _array.Count(t => t < TARGET);

        [Benchmark]
        public int GetProblemsCountNative() => GetProblemsCountNativeInternal(_array);

        /// <summary>
        /// To trick branch prediction, let's group elements that are True (<5) and False (>5) by sorting.
        /// </summary>
        [Benchmark]
        public int GetProblemsCountOrderedNative()
        {
            return GetProblemsCountNativeInternal(_array.OrderBy(x => x).ToArray());
        }

        /// <summary>
        /// Branch predictors are smart enough to detect patterns like TFTFTF...
        /// </summary>
        /// <returns></returns>
        [Benchmark]
        public int GetProblemsCountPatternOrderedNative()
        {
            var ordered = _array.OrderBy(x => x).ToArray();
            int[] ar = new int[ordered.Length];

            for (int i = 0, j = ordered.Length - 1; i <= ordered.Length / 2 && j >= 0; i++, j--)
            {
                ar[i] = ordered[i];

                if (ordered[i] != ordered[j] && i + 1 < ordered.Length)
                {
                    ar[i + 1] = ordered[j];
                }
            }

            return GetProblemsCountNativeInternal(ar);
        }

        private int GetProblemsCountNativeInternal(int[] array)
        {
            int result = 0;
            for (int i = 0; i < array.Length; i++)
                if (array[i] < TARGET)
                    result++;

            return result;
        }

        [Benchmark]
        public int SumSIMD()
        {
            Vector<int> vectorSum = Vector<int>.Zero, compareOperand = new(SIZE);
            Span<Vector<int>> vectorsArray = MemoryMarshal.Cast<int, Vector<int>>(_array);

            for (int i = 0; i < vectorsArray.Length; i++)
            {
                var lessVector = Vector.LessThan(vectorsArray[i], compareOperand);
                vectorSum += lessVector;
            }

            return - Vector.Dot(vectorSum, Vector<int>.One);
        }
    }
}
