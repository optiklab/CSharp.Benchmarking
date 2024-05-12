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

    [MinColumn]
    [DisassemblyDiagnoser]
    [MediumRunJob(RuntimeMoniker.Net80, Jit.RyuJit, Platform.X64)]
    public class IntegerSumCalculator
    {
        private const int SIZE = 1000;
        private const int MAX_VALUE = 10;

        private int[] _array;

        public IntegerSumCalculator()
        {
            _array = new int[SIZE];
            Random random = new();

            for (int i = 0; i < _array.Length; i++)
            {
                _array[i] = random.Next(MAX_VALUE);
            }
        }

        [Benchmark]
        public int EnumerableSum() => _array.Sum();

        [Benchmark]
        public int SumNative()
        {
            int result = 0;
            var bound = _array.Length;
            for (int i = 0; i < bound; i++)
                result += _array[i];

            return result;
        }

        //[Benchmark]
        //public unsafe int SumNativeUnsafe()
        //{
        //    int result = 0;
        //    int length = _array.Length;

        //    fixed (int* ptr = _array)
        //    {
        //        var pointer = ptr;
        //        var bound = pointer + length;
        //        while (pointer != bound)
        //        {
        //            result += *pointer;
        //            pointer += 1;
        //        }
        //    }

        //    return result;
        //}

        ///// <summary>
        ///// Use idea of unwiding of FOR LOOP. x4 less operations.
        ///// </summary>
        ///// <returns></returns>
        //[Benchmark]
        //public unsafe int SumTrickyUnsafe()
        //{
        //    int result = 0;
        //    int length = _array.Length;

        //    fixed (int* ptr = _array)
        //    {
        //        var pointer = ptr;
        //        var bound = pointer + length;
        //        while (pointer != bound)
        //        {
        //            result += *pointer;
        //            result += *(pointer + 1);
        //            result += *(pointer + 2);
        //            result += *(pointer + 3);
        //            pointer += 4;
        //        }
        //    }

        //    return result;
        //}

        ///// <summary>
        ///// CPU is using pipelineing (conveyor): it starts to work on next operation as soon
        ///// as current operation moved from reading to decoding (and next).
        ///// This method uses this knowledge to execute as quickly as possible by putting results
        ///// in 4 different variables.
        ///// </summary>
        ///// <returns></returns>
        //[Benchmark]
        //public unsafe int SumPipeliningUnsafe()
        //{
        //    int x = 0, y = 0, z = 0, w = 0;
        //    int length = _array.Length;

        //    fixed (int* ptr = _array)
        //    {
        //        var pointer = ptr;
        //        var bound = pointer + length;
        //        while (pointer != bound)
        //        {
        //            x += *pointer;
        //            y += *(pointer + 1);
        //            z += *(pointer + 2);
        //            w += *(pointer + 3);
        //            pointer += 4;
        //        }
        //    }

        //    x += y;
        //    z += w;
        //    return x + z;
        //}

        [Benchmark]
        public int SumSIMD()
        {
            Vector<int> vectorSum = Vector<int>.Zero;
            Span<Vector<int>> vectorsArray = MemoryMarshal.Cast<int, Vector<int>>(_array);

            for (int i = 0; i < vectorsArray.Length; i++)
                vectorSum += vectorsArray[i];

            return Vector.Dot(vectorSum, Vector<int>.One);
        }
    }
}
