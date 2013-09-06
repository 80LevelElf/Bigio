using System.Collections.Generic;
using System.Linq;
using System;
using BigDataCollections;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public static class DistributedArrayTest
    {
        [Test]
        public static void AddAndIsert()
        {
            var distributedArray = new DistributedArray<int>();
            var count = distributedArray.MaxBlockSize*2;

            for (int i = count/4; i < count/2; i++)
            {
                distributedArray.Add(i);
            }
            for (int i = 0; i < count/4; i++)
            {
                distributedArray.Insert(i, i);
            }
            for (int i = count/2; i < count*3/4; i++)
            {
                distributedArray.Add(i);
            }
            for (int i = count*3/4; i < count; i++)
            {
                distributedArray.Insert(i, i);
            }

            //DA must be : 0,1,2,3...,n-1,n
            for (int i = 0; i < distributedArray.Count - 1; i++)
            {
                Assert.IsTrue(distributedArray[i] + 1 == distributedArray[i + 1]);
            }
        }
        [Test]
        public static void AddRangeAndInsertRange()
        {
            var distributedArray = new DistributedArray<int>();
            var count = distributedArray.MaxBlockSize * 2;

            var array1 = new int[count / 4];
            var array2 = new int[count / 4];
            var array3 = new int[count / 4];
            var array4 = new int[count / 4];

            //1
            for (int i = count/4; i < count/2; i++)
            {
                array1[i - count/4] = i;
            }
            distributedArray.AddRange(array1);
            //2
            for (int i = 0; i < count/4; i++)
            {
                array2[i] = i;
            }
            distributedArray.InsertRange(0, array2);
            //3
            for (int i = count/2; i < count*3/4; i++)
            {
                array3[i - count/2] = i;
            }
            distributedArray.AddRange(array3);
            //4
            for (int i = count*3/4; i < count; i++)
            {
                array4[i - count*3/4] = i;
            }
            distributedArray.InsertRange(distributedArray.Count, array4);

            //DA must be : 0,1,2,3...,n-1,n
            for (int i = 0; i < distributedArray.Count - 1; i++)
            {
                Assert.IsTrue(distributedArray[i] + 1 == distributedArray[i + 1]);
            }
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
        }
        [Test]
        public static void Contains()
        {
            var distributedArray = new DistributedArray<int> {1};

            Assert.IsFalse(distributedArray.Contains(0));
            Assert.IsTrue(distributedArray.Contains(1));
            Assert.IsFalse(distributedArray.Contains(2));
        }
        [Test]
        public static void CopyTo()
        {
            var distibutedArray = new DistributedArray<int> {1,2,3};
            var arr = new int[8];

            distibutedArray.CopyTo(arr);
            distibutedArray.CopyTo(arr, 3);
            distibutedArray.CopyTo(1, arr, 6, 2);

            var resultArray = new[] {1, 2, 3, 1, 2, 3, 2, 3};
            //Arr must be equal resultArray
            Assert.IsFalse(arr.Where((t, i) => t != resultArray[i]).Any());
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
            Assert.AreEqual(distributedArray.MaxBlockSize, 4096);

            Assert.AreEqual(distributedArray.FindIndex(IsEqual5000), 5000);
            Assert.AreEqual(distributedArray.FindIndex(0, 4999, IsEqual5000), -1);
            Assert.AreEqual(distributedArray.FindIndex(IsEqual128000), -1);
            Assert.AreEqual(distributedArray.FindIndex(5001, 1000, IsEqual5000), -1);
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
            Assert.AreEqual(distributedArray.MaxBlockSize, 4096);

            Assert.AreEqual(distributedArray.FindLastIndex(IsEqual5000), 13192);
            Assert.AreEqual(distributedArray.FindLastIndex(4999, 5000, IsEqual5000), -1);
            Assert.AreEqual(distributedArray.FindLastIndex(IsEqual128000), -1);
            Assert.AreEqual(distributedArray.FindLastIndex(5001, 1000, IsEqual5000), 5000);
        }
        [Test]
        public static void Find()
        {
            var distributeArray = new DistributedArray<int> {1, 2, 3, 4};
            var a = distributeArray.Find(IsEqual0);
            Assert.IsTrue(distributeArray.Find(IsEqual0) == 0);
            Assert.IsTrue(distributeArray.Find(IsEqual2) == 2);
        }
        [Test]
        public static void FindAll()
        {
            var distributedArray = new DistributedArray<int> {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
            distributedArray = distributedArray.FindAll(IsMultipleOf2);
            var resultArray = new DistributedArray<int> {2, 4, 6, 8, 10};

            //distributedArray must be equal resultArray
            Assert.IsFalse(distributedArray.Where((t, i) => t != resultArray[i]).Any());
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
        }
        [Test]
        public static void GetEnumerator()
        {
            const int size = 5000;
            var array = new DistributedArray<int>();
            for (int i = 0; i < size; i++)
            {
                array.Add(i);
            }

            var newArray = new DistributedArray<int>();
            foreach (var i in array)
            {
                newArray.Add(i);
            }

            Assert.IsTrue(array.Count == newArray.Count);

            //array must be equal newArray
            Assert.IsFalse(array.Where((t, i) => t != newArray[i]).Any());
        }
        [Test]
        public static void GetRange()
        {
            const int size = 5000;
            const int rangeCount = 1000;
            var array = new DistributedArray<int>();
            //Fill array
            for (int i = 0; i < size; i++)
            {
                array.Add(i);
            }

            for (int i = 0; i < size/rangeCount; i++)
            {
                var range = array.GetRange(i*rangeCount, rangeCount);
                for (int j = 0; j < rangeCount; j++)
                {
                    Assert.IsTrue(range[j] == i*rangeCount + j);
                }
            }
        }

        //Support functions
        private static bool IsEqual5000(int number)
        {
            return number == 5000;
        }
        private static bool IsEqual128000(int number)
        {
            return number == 128000;
        }
        private static bool IsEqual0(int number)
        {
            return number == 0;
        }
        private static bool IsEqual2(int number)
        {
            return number == 2;
        }
        private static bool IsMultipleOf2(int number)
        {
            return number%2 == 0;
        }
    }
}
