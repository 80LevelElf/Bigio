using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Bigio;
using PerformanceTests.СomparativeTests;

namespace PerformanceTests
{
    class Program
    {
        static void StopwatchEstimation()
        {
            var list = new BigArray<int>();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Random random = new Random();
            //Write some test here

            for (int i = 0; i < 1000000; i++)
            {
                list.Insert(random.Next(i), i);
            }

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
                Task.Factory.StartNew(TestManager.TestAdd),
                Task.Factory.StartNew(TestManager.TestInsertInRandomPosition),
                Task.Factory.StartNew(TestManager.TestInsertInMiddlePosition),
                Task.Factory.StartNew(TestManager.TestInsertInStartPosition),
                Task.Factory.StartNew(TestManager.TestInsertRangeInRandom),
                Task.Factory.StartNew(TestManager.TestAddRange),
                Task.Factory.StartNew(TestManager.TestBinarySearch),
                Task.Factory.StartNew(TestManager.TestFindLast),
                Task.Factory.StartNew(TestManager.TestFind),
                Task.Factory.StartNew(TestManager.TestFindAll),
                Task.Factory.StartNew(TestManager.TestReverse),
            };

            Task.WaitAll(threads.ToArray());

            //StopwatchEstimation();
            Console.WriteLine("Press Enter to close window...");
            Console.ReadLine();
        }
    }
}
