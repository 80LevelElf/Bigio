using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using BigDataCollections;
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
            BlockStructureTests.ReverseMultyblockRange();
            Console.ReadKey();
        }
    }
}
