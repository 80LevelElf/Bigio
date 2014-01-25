using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BigDataCollections;
using NUnit.Framework;
using UnitTests.Managers;

namespace UnitTests.DistributedArrayTests
{
    [TestFixture]
    public static class DistributedArrayTest
    {
        [Test]
        public static void AddAndIsert()
        {
            var distributedArray = new DistributedArray<int>();
            var size = distributedArray.MaxBlockSize*2;

            for (int i = size/4; i < size/2; i++)
            {
                distributedArray.Add(i);
            }
            Assert.AreEqual(distributedArray.Count, size/4);
            for (int i = 0; i < size/4; i++)
            {
                distributedArray.Insert(i, i);
            }
            Assert.AreEqual(distributedArray.Count, size / 2);
            for (int i = size/2; i < size*3/4; i++)
            {
                distributedArray.Add(i);
            }
            Assert.AreEqual(distributedArray.Count, size * 3 / 4);
            for (int i = size*3/4; i < size; i++)
            {
                distributedArray.Insert(i, i);
            }
            Assert.AreEqual(distributedArray.Count, size);

            //DA must be : 0,1,2,3...,n-1,n
            for (int i = 0; i < distributedArray.Count - 1; i++)
            {
                Assert.IsTrue(distributedArray[i] + 1 == distributedArray[i + 1]);
            }

            //Exceptions
            Assert.IsTrue(ExceptionManager.IsThrowException
                <ArgumentOutOfRangeException, int, int>
                (distributedArray.Insert, -1, 0));
            Assert.IsTrue(ExceptionManager.IsThrowException
                <ArgumentOutOfRangeException, int, int>
                (distributedArray.Insert, distributedArray.Count + 1, 0));
        }
        [Test]
        public static void AddRangeAndInsertRange()
        {
            var distributedArray = new DistributedArray<int>();
            var size = distributedArray.MaxBlockSize * 2;

            var array1 = new int[size / 4];
            var array2 = new int[size / 4];
            var array3 = new int[size / 4];
            var array4 = new int[size / 4];

            //1
            for (int i = size/4; i < size/2; i++)
            {
                array1[i - size/4] = i;
            }
            distributedArray.AddRange(array1);
            Assert.AreEqual(distributedArray.Count, size / 4);
            //2
            for (int i = 0; i < size/4; i++)
            {
                array2[i] = i;
            }
            distributedArray.InsertRange(0, array2);
            Assert.AreEqual(distributedArray.Count, size / 2);
            //3
            for (int i = size/2; i < size*3/4; i++)
            {
                array3[i - size/2] = i;
            }
            distributedArray.AddRange(array3);
            Assert.AreEqual(distributedArray.Count, size *3 / 4);
            //4
            for (int i = size*3/4; i < size; i++)
            {
                array4[i - size*3/4] = i;
            }
            distributedArray.InsertRange(distributedArray.Count, array4);
            Assert.AreEqual(distributedArray.Count, size);

            //DA must be : 0,1,2,3...,n-1,n
            for (int i = 0; i < distributedArray.Count - 1; i++)
            {
                Assert.IsTrue(distributedArray[i] + 1 == distributedArray[i + 1]);
            }

            //Exceptions
            Assert.IsTrue(ExceptionManager.IsThrowException
                <ArgumentNullException, int, ICollection<int>>
                (distributedArray.InsertRange, 0, null));

            Assert.IsTrue(ExceptionManager.IsThrowException
                <ArgumentOutOfRangeException, int, ICollection<int>>
                (distributedArray.InsertRange, -1, new Collection<int>()));
            Assert.IsTrue(ExceptionManager.IsThrowException
                <ArgumentOutOfRangeException, int, ICollection<int>>
                (distributedArray.InsertRange, distributedArray.Count + 1, new Collection<int>()));
        }
        [Test]
        public static void BinarySearch()
        {
            var distributedArray = new DistributedArray<int>();
            for (int i = 0; i < 512; i += 2)
            {
                distributedArray.Add(i);
            }

            Assert.AreEqual(distributedArray.BinarySearch(128), 64);
            Assert.AreEqual(~distributedArray.BinarySearch(0, 64, 130, Comparer<int>.Default), 64);
            Assert.AreEqual(~distributedArray.BinarySearch(-100), 0);
            Assert.AreEqual(~distributedArray.BinarySearch(1), 1);

            var emptyArray = new DistributedArray<int>();
            Assert.AreEqual(~emptyArray.BinarySearch(1), 0);
        }
        [Test]
        public static void Contains()
        {
            var distributedArray = new DistributedArray<int> {1};

            Assert.IsFalse(distributedArray.Contains(0));
            Assert.IsTrue(distributedArray.Contains(1));
            Assert.IsFalse(distributedArray.Contains(2));

            var emptyArray = new DistributedArray<int>();
            Assert.AreEqual(emptyArray.Contains(0), false);
        }
        [Test]
        public static void CopyTo()
        {
            var distibutedArray = new DistributedArray<int> {1,2,3};
            var array = new int[8];

            distibutedArray.CopyTo(array);
            distibutedArray.CopyTo(array, 3);
            distibutedArray.CopyTo(1, array, 6, 2);

            var resultArray = new[] {1, 2, 3, 1, 2, 3, 2, 3};

            var emptyArray = new DistributedArray<int>();
            emptyArray.CopyTo(array);

            //Arr must be equal resultArray
            Assert.IsFalse(array.Where((t, i) => t != resultArray[i]).Any());

            //Exceptions
            Assert.IsTrue(ExceptionManager.IsThrowException
                <ArgumentNullException, int[]>
                (distibutedArray.CopyTo, null));

            Assert.IsTrue(ExceptionManager.IsThrowException
                <ArgumentOutOfRangeException, int[], int>
                (distibutedArray.CopyTo, array, array.Length - distibutedArray.Count + 1));
            Assert.IsTrue(ExceptionManager.IsThrowException
                <ArgumentOutOfRangeException, int[], int>
                (distibutedArray.CopyTo, array, -1));

            Assert.IsTrue(ExceptionManager.IsThrowException
                <ArgumentOutOfRangeException, int, int[], int, int>
                (distibutedArray.CopyTo, 0, array, array.Length - distibutedArray.Count + 1, 3));
            Assert.IsTrue(ExceptionManager.IsThrowException
                <ArgumentOutOfRangeException, int, int[], int, int>
                (distibutedArray.CopyTo, 0, array, 0, -1));
        }
        [Test]
        public static void Find()
        {
            var distributedArray = new DistributedArray<int> { 1, 2, 3, 4 };
            Assert.AreEqual(distributedArray.Find(IsEqual0), 0);
            Assert.AreEqual(distributedArray.Find(IsEqual2), 2);

            var emptyArray = new DistributedArray<int>();
            Assert.AreEqual(emptyArray.Find(IsEqual0), 0);

            //Exceptions
            ExceptionManager.IsThrowException<ArgumentNullException, Predicate<int>, int>
                (distributedArray.Find, null);
        }
        [Test]
        public static void FindAll()
        {
            var distributedArray = new DistributedArray<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            distributedArray = distributedArray.FindAll(IsMultipleOf2);
            var resultArray = new DistributedArray<int> { 2, 4, 6, 8, 10 };

            //distributedArray must be equal resultArray
            Assert.IsFalse(distributedArray.Where((t, i) => t != resultArray[i]).Any());

            var emptyArray = new DistributedArray<int>();
            Assert.IsEmpty(emptyArray.FindAll(IsMultipleOf2));

            //Exceptions
            ExceptionManager.IsThrowException
                <ArgumentNullException, Predicate<int>, DistributedArray<int>>
                (distributedArray.FindAll, null);
        }
        [Test]
        public static void FindIndex()
        {
            var distributedArray = new DistributedArray<int>();
            for (int i = 0; i < distributedArray.MaxBlockSize*2; i++)
            {
                distributedArray.Add(i);
            }
            for (int i = 0; i < distributedArray.MaxBlockSize*2; i++) //For mistakes with duplicate elements
            {
                distributedArray.Add(i);
            }

            //If MaxBlockSize is change, we need to change this code
            Assert.AreEqual(distributedArray.MaxBlockSize, distributedArray.MaxBlockSize);

            Assert.AreEqual(distributedArray.FindIndex(IsEqual5000), 5000);
            Assert.AreEqual(distributedArray.FindIndex(0, 4999, IsEqual5000), -1);
            Assert.AreEqual(distributedArray.FindIndex(IsEqual128000), -1);
            Assert.AreEqual(distributedArray.FindIndex(5001, 1000, IsEqual5000), -1);
            
            var emptyArray = new DistributedArray<int>();
            Assert.AreEqual(emptyArray.FindIndex(IsEqual0), -1);

            //Exceptions
            Assert.IsTrue(ExceptionManager.IsThrowException
                <ArgumentNullException, Predicate<int>, int>
                (distributedArray.FindIndex, null));

            Assert.IsTrue(ExceptionManager.IsThrowException
                <ArgumentOutOfRangeException, int, Predicate<int>, int>
                (distributedArray.FindIndex, distributedArray.Count, IsEqual0));
            Assert.IsTrue(ExceptionManager.IsThrowException
                <ArgumentOutOfRangeException, int, Predicate<int>, int>
                (distributedArray.FindIndex, -1, IsEqual0));

            Assert.IsTrue(ExceptionManager.IsThrowException
                <ArgumentOutOfRangeException, int, int, Predicate<int>, int>
                (distributedArray.FindIndex, distributedArray.Count - 1, 2, IsEqual0));
            Assert.IsTrue(ExceptionManager.IsThrowException
                <ArgumentOutOfRangeException, int, int, Predicate<int>, int>
                (distributedArray.FindIndex, -1, 1, IsEqual0));
            Assert.IsTrue(ExceptionManager.IsThrowException
                <ArgumentOutOfRangeException, int, int, Predicate<int>, int>
                (distributedArray.FindIndex, 1, -1, IsEqual0));
        }
        [Test]
        public static void FindLastIndex()
        {
            var distributedArray = new DistributedArray<int>();
            for (int i = 0; i < distributedArray.MaxBlockSize*2; i++)
            {
                distributedArray.Add(i);
            }
            for (int i = 0; i < distributedArray.MaxBlockSize*2; i++) //For mistakes with duplicate elements
            {
                distributedArray.Add(i);
            }

            //If MaxBlockSize is change, we need to change this code
            Assert.AreEqual(distributedArray.MaxBlockSize, distributedArray.MaxBlockSize);

            Assert.AreEqual(distributedArray.FindLastIndex(IsEqual5000), 13192);
            Assert.AreEqual(distributedArray.FindLastIndex(4999, 5000, IsEqual5000), -1);
            Assert.AreEqual(distributedArray.FindLastIndex(IsEqual128000), -1);
            Assert.AreEqual(distributedArray.FindLastIndex(5001, 1000, IsEqual5000), 5000);

            var emptyArray = new DistributedArray<int>();
            Assert.AreEqual(emptyArray.FindLastIndex(IsEqual0), -1);

            //Exceptions
            Assert.IsTrue(ExceptionManager.IsThrowException
                <ArgumentNullException, Predicate<int>, int>
                (distributedArray.FindLastIndex, null));

            Assert.IsTrue(ExceptionManager.IsThrowException
                <ArgumentOutOfRangeException, int, Predicate<int>, int>
                (distributedArray.FindLastIndex, distributedArray.Count, IsEqual0));
            Assert.IsTrue(ExceptionManager.IsThrowException
                <ArgumentOutOfRangeException, int, Predicate<int>, int>
                (distributedArray.FindLastIndex, -1, IsEqual0));

            Assert.IsTrue(ExceptionManager.IsThrowException
                <ArgumentOutOfRangeException, int, int, Predicate<int>, int>
                (distributedArray.FindLastIndex, distributedArray.Count - 1
                , distributedArray.Count + 1, IsEqual0));
            Assert.IsTrue(ExceptionManager.IsThrowException
                <ArgumentOutOfRangeException, int, int, Predicate<int>, int>
                (distributedArray.FindLastIndex, distributedArray.Count + 1
                , 1, IsEqual0));
            Assert.IsTrue(ExceptionManager.IsThrowException
                <ArgumentOutOfRangeException, int, int, Predicate<int>, int>
                (distributedArray.FindLastIndex, 1, -1, IsEqual0));
        }
        [Test]
        public static void GetEnumerator()
        {
            var distributedArray = new DistributedArray<int>();
            int size = 4 * distributedArray.MaxBlockSize;

            for (int i = 0; i < size; i++)
            {
                distributedArray.Add(i);
            }

            var newArray = new DistributedArray<int>();
            foreach (var i in distributedArray)
            {
                newArray.Add(i);
            }

            Assert.IsTrue(distributedArray.Count == newArray.Count);

            //array must be equal newArray
            Assert.IsFalse(distributedArray.Where((t, i) => t != newArray[i]).Any());
        }
        [Test]
        public static void GetRange()
        {
            var distributedArray = new DistributedArray<int>();
            int size = 4 * distributedArray.MaxBlockSize;
            int rangeCount = distributedArray.DefaultBlockSize;

            //Fill array
            for (int i = 0; i < size; i++)
            {
                distributedArray.Add(i);
            }

            for (int i = 0; i < size / rangeCount; i++)
            {
                var range = distributedArray.GetRange(i * rangeCount, rangeCount);
                for (int j = 0; j < rangeCount; j++)
                {
                    Assert.IsTrue(range[j] == i * rangeCount + j);
                }
            }

            var emptyArray = new DistributedArray<int>();
            Assert.IsEmpty(emptyArray.GetRange(0, 0));

            //Exceptions
            Assert.IsTrue(ExceptionManager.IsThrowException
                <ArgumentOutOfRangeException, int, int, DistributedArray<int>>
                (distributedArray.GetRange, -1, 1));
            Assert.IsTrue(ExceptionManager.IsThrowException
                <ArgumentOutOfRangeException, int, int, DistributedArray<int>>
                (distributedArray.GetRange, 0, distributedArray.Count + 1));
        }
        [Test]
        public static void Indexer()
        {
            var distributedArray = new DistributedArray<int>();
            for (int i = 0; i < distributedArray.MaxBlockSize * 2; i++)
            {
                distributedArray.Add(i);
            }

            for (int i = 0; i < distributedArray.Count; i++)
            {
                Assert.AreEqual(distributedArray[i], i);
            }

        }
        [Test]
        public static void IndexOf()
        {
            var distributedArray = new DistributedArray<int>();
            for (int i = 0; i < distributedArray.MaxBlockSize*2; i++)
            {
                distributedArray.Add(i);
            }
            for (int i = 0; i < distributedArray.MaxBlockSize*2; i++) //For mistakes with duplicate elements
            {
                distributedArray.Add(i);
            }

            //If MaxBlockSize is change, we need to change this code
            Assert.AreEqual(distributedArray.MaxBlockSize, 4096);

            Assert.AreEqual(distributedArray.IndexOf(5000), 5000);
            Assert.AreEqual(distributedArray.IndexOf(0, 1, 5000), -1);
            Assert.AreEqual(distributedArray.IndexOf(128000), -1);
            Assert.AreEqual(distributedArray.IndexOf(5001, 0, 5000), -1);

            var emptyArray = new DistributedArray<int>();
            Assert.AreEqual(emptyArray.IndexOf(0), -1);

            //Exceptions
            Assert.IsTrue(ExceptionManager.IsThrowException
                <ArgumentOutOfRangeException, int, int, int>
                (distributedArray.IndexOf, 0, distributedArray.Count));
            Assert.IsTrue(ExceptionManager.IsThrowException
                <ArgumentOutOfRangeException, int, int, int>
                (distributedArray.IndexOf, 0, -1));

            Assert.IsTrue(ExceptionManager.IsThrowException
                <ArgumentOutOfRangeException, int, int, int, int>
                (distributedArray.IndexOf, 0, -1, 1));
            Assert.IsTrue(ExceptionManager.IsThrowException
                <ArgumentOutOfRangeException, int, int, int, int>
                (distributedArray.IndexOf, 0, distributedArray.Count - 1, 2));
            Assert.IsTrue(ExceptionManager.IsThrowException
                <ArgumentOutOfRangeException, int, int, int, int>
                (distributedArray.IndexOf, 0, 1, -1));
        }
        [Test]
        public static void LastIndexOf()
        {
            var distributedArray = new DistributedArray<int>();
            for (int i = 0; i < distributedArray.MaxBlockSize*2; i++)
            {
                distributedArray.Add(i);
            }
            for (int i = 0; i < distributedArray.MaxBlockSize*2; i++) //For mistakes with duplicate elements
            {
                distributedArray.Add(i);
            }

            //If MaxBlockSize is change, we need to change this code
            Assert.AreEqual(distributedArray.MaxBlockSize, 4096);

            Assert.AreEqual(distributedArray.LastIndexOf(5000), 13192);
            Assert.AreEqual(distributedArray.LastIndexOf(5000, 4999, 5000), -1);
            Assert.AreEqual(distributedArray.LastIndexOf(128000), -1);
            Assert.AreEqual(distributedArray.LastIndexOf(5000, 5001, 1000), 5000);

            var emptyArray = new DistributedArray<int>();
            Assert.AreEqual(emptyArray.LastIndexOf(0), -1);

            //Exceptions
            Assert.IsTrue(ExceptionManager.IsThrowException
                <ArgumentOutOfRangeException, int, int, int>
                (distributedArray.LastIndexOf, 0, distributedArray.Count));
            Assert.IsTrue(ExceptionManager.IsThrowException
                <ArgumentOutOfRangeException, int, int, int>
                (distributedArray.LastIndexOf, 0, -1));

            Assert.IsTrue(ExceptionManager.IsThrowException
                <ArgumentOutOfRangeException, int, int, int, int>
                (distributedArray.LastIndexOf, 0, 0, 2));
            Assert.IsTrue(ExceptionManager.IsThrowException
                <ArgumentOutOfRangeException, int, int, int, int>
                (distributedArray.LastIndexOf, 0, distributedArray.Count, 1));
            Assert.IsTrue(ExceptionManager.IsThrowException
                <ArgumentOutOfRangeException, int, int, int, int>
                (distributedArray.LastIndexOf, 0, 2, -1));
        }
        [Test]
        public static void Remove()
        {
            var distributedArray = new DistributedArray<int>();

            int size = 4 * distributedArray.MaxBlockSize;
            var checkList = new List<int>(size);
            for (int i = 0; i < size; i++)
            {
                distributedArray.Add(i);
                checkList.Add(i);
            }

            //Remove last element of first block
            Assert.IsTrue(distributedArray.Remove(distributedArray.MaxBlockSize - 1));
            checkList.Remove(distributedArray.MaxBlockSize - 1);
            Assert.AreEqual(distributedArray.Count, checkList.Count);

            //Remove first element of second block
            Assert.IsTrue(distributedArray.Remove(distributedArray.MaxBlockSize));
            checkList.Remove(distributedArray.MaxBlockSize);
            Assert.AreEqual(distributedArray.Count, checkList.Count);

            Assert.IsTrue(distributedArray.Remove(0));
            checkList.Remove(0);
            Assert.AreEqual(distributedArray.Count, checkList.Count);

            Assert.IsTrue(distributedArray.Remove(distributedArray.Count-1));
            checkList.Remove(checkList.Count - 1);
            Assert.AreEqual(distributedArray.Count, checkList.Count);

            //Try to remove nonexistent elements
            Assert.IsFalse(distributedArray.Remove(0));
            Assert.IsFalse(distributedArray.Remove(size));
            Assert.IsFalse(distributedArray.Remove(distributedArray.MaxBlockSize));
            Assert.IsFalse(distributedArray.Remove(-1));

            //distributedArray must be equal list
            Assert.IsFalse(distributedArray.Where((t, i) => t != checkList[i]).Any());
        }
        [Test]
        public static void RemoveAt()
        {
            var distributedArray = new DistributedArray<int>();

            int size = 4 * distributedArray.MaxBlockSize;
            var checkList = new List<int>(size);
            for (int i = 0; i < size; i++)
            {
                distributedArray.Add(i);
                checkList.Add(i);
            }

            //Remove last element of first block
            distributedArray.RemoveAt(distributedArray.MaxBlockSize - 1);
            checkList.RemoveAt(distributedArray.MaxBlockSize - 1);
            Assert.AreEqual(distributedArray.Count, checkList.Count);

            //Remove first element of second block
            distributedArray.RemoveAt(distributedArray.MaxBlockSize + 1);
            checkList.RemoveAt(distributedArray.MaxBlockSize + 1);
            Assert.AreEqual(distributedArray.Count, checkList.Count);

            distributedArray.RemoveAt(0);
            checkList.RemoveAt(0);
            Assert.AreEqual(distributedArray.Count, checkList.Count);

            distributedArray.RemoveAt(distributedArray.Count - 1);
            checkList.RemoveAt(checkList.Count - 1);
            Assert.AreEqual(distributedArray.Count, checkList.Count);

            //distributedArray must be equal list
            Assert.IsFalse(distributedArray.Where((t, i) => t != checkList[i]).Any());

            //Exceptions
            Assert.IsTrue(ExceptionManager.IsThrowException
                <ArgumentOutOfRangeException, int>
                (distributedArray.RemoveAt, -1));
            Assert.IsTrue(ExceptionManager.IsThrowException
                <ArgumentOutOfRangeException, int>
                (distributedArray.RemoveAt, distributedArray.Count));
        }
        [Test]
        public static void RemoveLast()
        {
            var distributedArray = new DistributedArray<int>();
            int size = 4*distributedArray.MaxBlockSize;

            var checkList = new List<int>(size);
            //Add
            for (int i = 0; i < size; i++)
            {
                distributedArray.Add(i);
                checkList.Add(i);
            }

            //Remove
            for (int i = 0; i < size; i++)
            {
                distributedArray.RemoveLast();
                checkList.RemoveAt(checkList.Count - 1);

                Assert.AreEqual(distributedArray.Count, checkList.Count);
            }

            //array must be equal checkList
            Assert.IsFalse(distributedArray.Where((t, i) => t != checkList[i]).Any());
        }
        [Test]
        public static void RemoveRange()
        {
            var distributedArray = new DistributedArray<int>();

            int size = 4 * distributedArray.MaxBlockSize;
            var checkList = new List<int>(size);
            for (int i = 0; i < size; i++)
            {
                distributedArray.Add(i);
                checkList.Add(i);
            }

            //Remove elements from different blocks
            distributedArray.RemoveRange(distributedArray.MaxBlockSize / 2, distributedArray.MaxBlockSize);
            checkList.RemoveRange(distributedArray.MaxBlockSize / 2, distributedArray.MaxBlockSize);
            Assert.AreEqual(distributedArray.Count, checkList.Count);

            distributedArray.RemoveRange(0, 1);
            checkList.RemoveRange(0, 1);
            Assert.AreEqual(distributedArray.Count, checkList.Count);

            distributedArray.RemoveRange(distributedArray.Count - 1, 1);
            checkList.RemoveRange(checkList.Count - 1, 1);
            Assert.AreEqual(distributedArray.Count, checkList.Count);

            //distributedArray must be equal list
            Assert.IsFalse(distributedArray.Where((t, i) => t != checkList[i]).Any());

            //Clear distibutedArray
            distributedArray.RemoveRange(0, distributedArray.Count);
            Assert.IsTrue(distributedArray.Count == 0);

            var emptyArray = new DistributedArray<int>();
            emptyArray.RemoveRange(0, 0);
            Assert.IsEmpty(emptyArray);

            //Exceptions
            Assert.IsTrue(ExceptionManager.IsThrowException
                <ArgumentOutOfRangeException, int, int>
                (distributedArray.RemoveRange, -1, 1));
            Assert.IsTrue(ExceptionManager.IsThrowException
                <ArgumentOutOfRangeException, int, int>
                (distributedArray.RemoveRange, distributedArray.Count - 1, 2));
        }
        [Test]
        public static void Reverse()
        {
            var distributedArray = new DistributedArray<int>();
            for (int i = 0; i < distributedArray.MaxBlockSize*2; i++)
            {
                distributedArray.Add(i);
            }
            distributedArray.Reverse();

            //Check
            int count = distributedArray.Count;
            for (int i = 0; i < count; i++)
            {
                Assert.AreEqual(i, distributedArray[count - 1 - i]);
            }

            //Empty array
            var newArray = new DistributedArray<int>();
            newArray.Reverse();

            newArray.Add(0); // This items are in the
            newArray.Add(1); // insuring block

            newArray.Reverse();

            Assert.AreEqual(newArray[0], 1);
            Assert.AreEqual(newArray[1], 0);
        }
        [Test]
        public static void ToArray()
        {
            var distributedArray = new DistributedArray<int>();
            for (int i = 0; i < distributedArray.MaxBlockSize*2; i++)
            {
                distributedArray.Add(i);
            }

            var array = distributedArray.ToArray();
            for (int i = 0; i < distributedArray.Count; i++)
            {
                Assert.AreEqual(distributedArray[i], array[i]);
            }
        }

        //Support functions
        private static bool IsEqual0(int number)
        {
            return number == 0;
        }
        private static bool IsEqual128000(int number)
        {
            return number == 128000;
        }
        private static bool IsEqual2(int number)
        {
            return number == 2;
        }
        private static bool IsEqual5000(int number)
        {
            return number == 5000;
        }
        private static bool IsMultipleOf2(int number)
        {
            return number%2 == 0;
        }
    }
}
