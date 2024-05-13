using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CSharp.Benchmarking.NET8.PdhApi.Multithreaded
{
    // Taken from
    // http://www.philosophicalgeek.com/2009/01/03/determine-cpu-usage-of-current-process-c-and-c/
    class Program
    {
        static CpuUsage usage = new CpuUsage();

        static void Main(string[] args)
        {
            
            ThreadPool.QueueUserWorkItem(EatItThreadProc);
            ThreadPool.QueueUserWorkItem(EatItThreadProc);

            ThreadPool.QueueUserWorkItem(WatchItThreadProc);
            ThreadPool.QueueUserWorkItem(WatchItThreadProc);

            while (true)
            {
                Thread.Sleep(1000);
            }
        }

        private static void WatchItThreadProc(object obj)
        {
            while (true)
            {
                short cpuUsage = usage.GetUsage();
                Console.WriteLine("Thread id {0}: {1}% cpu usage",Thread.CurrentThread.ManagedThreadId, cpuUsage);
                Thread.Sleep(1000);
            }
        }

        private static void EatItThreadProc(object obj)
        {
            UInt64 accum = 0;
            while (true)
            {
                accum++;
            }
            Console.WriteLine("{0}", accum);
        }
    }
}
