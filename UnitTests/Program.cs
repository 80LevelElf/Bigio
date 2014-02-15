using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using BigDataCollections;
using BigDataCollections.DistributedArray.SupportClasses;
using BigDataCollections.LevelSparseArray;
using BigDataCollections.LevelSparseArray.Managers;
using BigDataCollections.LevelSparseArray.SupportClasses;
using UnitTests.DistributedArrayTests;

namespace UnitTests
{
    class Program
    {
        static void Main()
        {
            var array = new DistributedArray<int>();
            for (int i = 0; i < 1000000; i++)
            {
                array.Add(i);
            }

            for (int i = 0; i < 1000000; i++)
            {
                var a = array[i];
            }

            Console.WriteLine(1);
            Console.ReadKey();
        }
    }
}
