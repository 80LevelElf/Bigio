using System;
using System.Collections.Generic;
using BigDataCollections;
using UnitTests;

namespace UnitTests
{
    class Some
    {
        public Some(int v)
        {
            Value = v;
        }
        public int Value;
    }
    class Program
    {
        static void Main(string[] args)
        {
            var list = new DistributedArray<int>(10000){1, 2, 3, 4};
            Console.ReadKey();
        }
    }
}
