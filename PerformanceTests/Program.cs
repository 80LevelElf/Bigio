using System;
using BenchmarkDotNet.Running;
using PerformanceTests.BigioTests;

namespace PerformanceTests
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<AddTest>();

            Console.WriteLine("Press Enter to close window...");
            Console.ReadLine();
        }
    }
}
