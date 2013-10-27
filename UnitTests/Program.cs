using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using BigDataCollections;

namespace UnitTests
{
    class Program
    {
        static void Main(string[] args)
        {
            DistributedArrayTest.RemoveAt();
            Console.ReadKey();
        }
    }
}
