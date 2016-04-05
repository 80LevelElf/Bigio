using System;
using BenchmarkDotNet.Running;

//Specify namespace of test
using PerformanceTests.BigioTests;

namespace PerformanceTests
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<AddAndAddRangeTest>();
            BenchmarkRunner.Run<IndexOfAndSearchTest>();
            BenchmarkRunner.Run<InsertAndInsertRangeTest>();
            BenchmarkRunner.Run<LoopsTest>();

            Console.WriteLine("Press Enter to close window...");
            Console.ReadLine();
        }
    }
}
