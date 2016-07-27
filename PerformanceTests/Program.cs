using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            int[] arr = new int[10000000];

            for (int i = 0; i < 10000000; i++)
            {
                arr[i] = i;
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            //Write some test code here
            var list = new BigArray<int>();

            for (int i = 0; i < 10; i++)
            {
                list.AddRange(arr);
            }

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
            StartСomparativeTests();
            Console.WriteLine("Press Enter to close window...");
            Console.ReadLine();
        }
    }
}
