using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Bigio;
using PerformanceTests.EngineMeasuringTest;
using PerformanceTests.СomparativeTests;

namespace PerformanceTests
{
    class Program
    {
        static void StopwatchEstimation()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            //Write some test code here
            var list = new BigArray<int>();
            Random _random = new Random();

            for (int i = 0; i < 1000000; i++)
            {
                list.Insert(_random.Next(list.Count), i);
            }

            list.Insert(1000000 - 1, 1);
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
        }

        static void StartBlockStructureTest()
        {
            Console.WriteLine("BlockStructure performance test");
            var threads = new List<Task>()
            {
                Task.Factory.StartNew(BlockStructureTest.TestBinarySearch),
                Task.Factory.StartNew(BlockStructureTest.TestLinearSearch),
            };

            Task.WaitAll(threads.ToArray());
        }

        static void StartСomparativeTests()
        {
            Console.WriteLine("Сomparative estimation of Bigio BigArray, Wintellect BigList and .Net List");
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
        }

        static void Main(string[] args)
        {
            StopwatchEstimation();
            Console.WriteLine("Press Enter to close window...");
            Console.ReadLine();
        }
    }
}
