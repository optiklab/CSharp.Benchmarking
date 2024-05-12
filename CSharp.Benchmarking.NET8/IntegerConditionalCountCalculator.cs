using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using System;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;

namespace CSharp.Benchmarking.NET8
{
    /*
     */

    [HardwareCounters(BenchmarkDotNet.Diagnosers.HardwareCounter.BranchMispredictions)]
    [ShortRunJob(RuntimeMoniker.Net80, Jit.RyuJit, Platform.X64)]
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
