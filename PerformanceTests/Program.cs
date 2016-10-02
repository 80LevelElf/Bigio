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

        static void StartArrayMapTest()
        {
            Console.WriteLine("ArrayMap performance test");
            var threads = new List<Task>()
            {
                Task.Factory.StartNew(ArrayMapTests.TestBinarySearch),
                Task.Factory.StartNew(ArrayMapTests.TestLinearSearch),
            };

            Task.WaitAll(threads.ToArray());
        }

        static void StartСomparativeTests<T>()
        {
            Console.WriteLine("Comparative estimation of Bigio BigArray, Wintellect BigList and .Net List");
			Console.WriteLine("Type: " + typeof(T).Name);
            var threads = new List<Task>()
            {
				Task.Factory.StartNew(TestManager<T>.TestFor),
				Task.Factory.StartNew(TestManager<T>.TestForeach),
				Task.Factory.StartNew(TestManager<T>.TestIndexOf),
				Task.Factory.StartNew(TestManager<T>.TestLastIndexOf),
				Task.Factory.StartNew(TestManager<T>.TestAdd),
				Task.Factory.StartNew(TestManager<T>.TestInsertInRandomPosition),
				Task.Factory.StartNew(TestManager<T>.TestInsertInMiddlePosition),
				Task.Factory.StartNew(TestManager<T>.TestInsertInStartPosition),
				Task.Factory.StartNew(TestManager<T>.TestInsertRangeInRandom),
				Task.Factory.StartNew(TestManager<T>.TestAddRange),
				Task.Factory.StartNew(TestManager<T>.TestBinarySearch),
				Task.Factory.StartNew(TestManager<T>.TestFindLast),
				Task.Factory.StartNew(TestManager<T>.TestFind),
                //Task.Factory.StartNew(TestManager<T>.TestFindAll),
                //Task.Factory.StartNew(TestManager<T>.TestReverse),
            };

            Task.WaitAll(threads.ToArray());
        }

        static void Main(string[] args)
        {
            StartСomparativeTests<string>();
            Console.WriteLine("Press Enter to close window...");
            Console.ReadLine();
        }
    }
}
