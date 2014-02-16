using System;
using BigDataCollections;

namespace UnitTests
{
    class Program
    {
        static void Main()
        {
            var array = new DistributedArray<int>();
            const int size = 1000000;

            for (int i = 0; i < size; i++)
            {
                array.Add(i);
            }

            var oldTime = DateTime.Now;

            for (int i = 0; i < size; i++)
            {
                var a = array[i];
            }

            Console.WriteLine((DateTime.Now - oldTime).TotalMilliseconds);
            Console.ReadKey();
        }
    }
}
