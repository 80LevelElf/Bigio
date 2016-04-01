using System;
using BenchmarkDotNet.Running;
using PerformanceTests.BigArray;

namespace PerformanceTests
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<TemplateTest_JustToCheckSmth>();

            Console.WriteLine("Press Enter to close window...");
            Console.ReadLine();
        }
    }
}
