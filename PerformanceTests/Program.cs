using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PerformanceTests
{

    class Program
    {
        static void StopwatchEstimation()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            //Write some test here

            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
        }

        static void Main(string[] args)
        {
            var threads = new List<Task>()
            {
                Task.Factory.StartNew(TestManager.TestFor),
                Task.Factory.StartNew(TestManager.TestForeach),
                Task.Factory.StartNew(TestManager.TestIndexOf),
                Task.Factory.StartNew(TestManager.TestLastIndexOf),
                Task.Factory.StartNew(TestManager.TestInsertInMiddlePosition),
                Task.Factory.StartNew(TestManager.TestInsertInStartPosition),
                Task.Factory.StartNew(TestManager.TestAdd),
                Task.Factory.StartNew(TestManager.TestInsertInRandomPosition),
                Task.Factory.StartNew(TestManager.TestInsertRangeInRandom),
                Task.Factory.StartNew(TestManager.TestAddRange),
                Task.Factory.StartNew(TestManager.TestBinarySearch),
                Task.Factory.StartNew(TestManager.TestFindLast),
                Task.Factory.StartNew(TestManager.TestFind),
                Task.Factory.StartNew(TestManager.TestFindAll),
                Task.Factory.StartNew(TestManager.TestReverse),
            };

            Task.WaitAll(threads.ToArray());

            Console.WriteLine("Press Enter to close window...");
            Console.ReadLine();
        }
    }
}
