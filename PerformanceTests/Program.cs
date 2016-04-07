using System;

namespace PerformanceTests
{
    class Program
    {
        static void Main(string[] args)
        {
            TestManager.TestIndexOf();

            Console.WriteLine("Press Enter to close window...");
            Console.ReadLine();
        }
    }
}
