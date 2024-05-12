using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using System;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;

namespace CSharp.Benchmarking.NET6
{
    /*
    // * Summary *

    BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3447/23H2/2023Update/SunValley3)
    11th Gen Intel Core i9-11900H 2.50GHz, 1 CPU, 16 logical and 8 physical cores
    .NET SDK 8.0.101
    [Host]                        : .NET 6.0.29 (6.0.2924.17105), X64 RyuJIT AVX2
    MediumRun-.NET 6.0-RyuJit-X64 : .NET 6.0.29 (6.0.2924.17105), X64 RyuJIT AVX2

    Job=MediumRun-.NET 6.0-RyuJit-X64  Jit=RyuJit  Platform=X64
    Runtime=.NET 6.0  IterationCount=15  LaunchCount=2
    WarmupCount=10

    | Method        | Mean        | Error     | StdDev    | Min         | Code Size |
    |-------------- |------------:|----------:|----------:|------------:|----------:|
    | EnumerableSum | 2,991.48 ns | 35.765 ns | 51.293 ns | 2,914.29 ns |     205 B |
    | SumNative     |   398.70 ns |  5.051 ns |  7.244 ns |   391.04 ns |      68 B |
    | SumSIMD       |    68.68 ns |  0.667 ns |  0.998 ns |    67.43 ns |     130 B |

    // * Hints *
    Outliers
    IntegerSumCalculator.EnumerableSum: MediumRun-.NET 6.0-RyuJit-X64 -> 2 outliers were removed (3.15 us, 3.20 us)
    IntegerSumCalculator.SumNative: MediumRun-.NET 6.0-RyuJit-X64     -> 2 outliers were removed (426.00 ns, 431.83 ns)
    IntegerSumCalculator.SumSIMD: MediumRun-.NET 6.0-RyuJit-X64       -> 1 outlier  was  removed (72.37 ns)

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
    [MediumRunJob(RuntimeMoniker.Net60, Jit.RyuJit, Platform.X64)]
    public class IntegerSumCalculator
    {
        private int[] _array;

        public IntegerSumCalculator()
        {
            _array = new int[1000];
            Random random = new();

            for (int i = 0; i < _array.Length; i++)
            {
                _array[i] = random.Next(10);
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
