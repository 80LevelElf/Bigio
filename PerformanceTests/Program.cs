using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Bigio;
using Bigio.BigArray;
using Bigio.BigArray.Support_Classes.BlockCollection;
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

			BigArray<int> array = new BigArray<int>();

	        for (int i = 0; i < 64 * 1024; i++)
	        {
		        array.Add(i);
	        }

	        for (int i = 0; i < 100; i++)
	        {
		        for (int j = 0; j < 64 * 1024; j++)
		        {
			        var a = array[j];
		        }
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
                Task.Factory.StartNew(TestManager<T>.TestFindAll),
                Task.Factory.StartNew(TestManager<T>.TestReverse),
				Task.Factory.StartNew(TestManager<T>.TestContains)
			};

			Task.WaitAll(threads.ToArray());
        }

        static void Main(string[] args)
        {
			StartСomparativeTests<string>();
	        //StopwatchEstimation();

			Console.WriteLine("Press Enter to close window...");
            Console.ReadLine();
        }
    }
}
