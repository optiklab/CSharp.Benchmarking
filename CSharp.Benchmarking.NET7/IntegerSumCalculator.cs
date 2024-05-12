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
        [Host]                        : .NET 7.0.18 (7.0.1824.16914), X64 RyuJIT AVX2
        MediumRun-.NET 7.0-RyuJit-X64 : .NET 7.0.18 (7.0.1824.16914), X64 RyuJIT AVX2

    Job=MediumRun-.NET 7.0-RyuJit-X64  Jit=RyuJit  Platform=X64
    Runtime=.NET 7.0  IterationCount=15  LaunchCount=2
    WarmupCount=10

    | Method        | Mean      | Error     | StdDev    | Min       | Code Size |
    |-------------- |----------:|----------:|----------:|----------:|----------:|
    | EnumerableSum | 354.55 ns | 14.566 ns | 21.350 ns | 335.23 ns |     315 B |
    | SumNative     | 351.93 ns |  3.594 ns |  5.155 ns | 345.58 ns |      68 B |
    | SumSIMD       |  60.64 ns |  0.512 ns |  0.751 ns |  59.34 ns |     135 B |

    // * Hints *
    Outliers
        IntegerSumCalculator.EnumerableSum: MediumRun-.NET 7.0-RyuJit-X64 -> 4 outliers were removed (392.96 ns..408.96 ns)
        IntegerSumCalculator.SumNative: MediumRun-.NET 7.0-RyuJit-X64     -> 1 outlier  was  removed (384.92 ns)
        IntegerSumCalculator.SumSIMD: MediumRun-.NET 7.0-RyuJit-X64       -> 2 outliers were removed (64.41 ns, 66.87 ns)

    // * Legends *
        Mean      : Arithmetic mean of all measurements
        Error     : Half of 99.9% confidence interval
        StdDev    : Standard deviation of all measurements
        Min       : Minimum
        Code Size : Native code size of the disassembled method(s)
        1 ns      : 1 Nanosecond (0.000000001 sec)
     */

    [MinColumn]
    [DisassemblyDiagnoser]
    [MediumRunJob(RuntimeMoniker.Net70, Jit.RyuJit, Platform.X64)]
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
