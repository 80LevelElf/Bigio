using System;

namespace PerformanceTests
{
    class Program
    {
        static void Main(string[] args)
        {
            TestManager.TestAdd();

            Console.WriteLine("Press Enter to close window...");
            Console.ReadLine();
        }
    }
}
