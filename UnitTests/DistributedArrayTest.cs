using System.Collections.Generic;
using System.Linq;
using System;
using BigDataCollections;
using NUnit.Framework;

namespace UnitTest
{
    [TestFixture]
    public static class DistributedArrayTest
    {
        [Test]
        public static void AddAndIsert()
        {
            var distributedArray = new DistributedArray<int>();
            var count = distributedArray.MaxBlockSize*2;

            for (int i = count / 4; i < count/2; i++)
                distributedArray.Add(i);
            for (int i = 0; i < count / 4; i++)
                distributedArray.Insert(i, i);
            for (int i=count/2; i < count*3/4; i++)
                distributedArray.Add(i);
            for (int i = count*3/4; i < count; i++)
                distributedArray.Insert(i, i);

            //DA must be : 0,1,2,3...,n-1,n
            bool isOk = true;
            for(int i=0;i<distributedArray.Count - 1;i++)
                if (distributedArray[i] + 1 != distributedArray[i + 1])
                {
                    isOk = false;
                    break;
                }
            Assert.IsTrue(isOk);
        }
        [Test]
        public static void BinarySearch()
        {
            var distributedArray = new DistributedArray<int>();
            for (int i = 0; i < 512; i += 2)
                distributedArray.Add(i);

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
            var isOk = !arr.Where((t, i) => t != resultArray[i]).Any();
            Assert.IsTrue(isOk);
        }
        [Test]
        public static void FindIndex()
        {
            var distributedArray = new DistributedArray<int>();
            for (int i = 0; i < distributedArray.MaxBlockSize * 2; i++)
                distributedArray.Add(i);
            for (int i = 0; i < distributedArray.MaxBlockSize * 2; i++) //For mistakes with duplicate elements
                distributedArray.Add(i);

            //If MaxBlockSize is change, we need to change this code
            Assert.AreEqual(distributedArray.MaxBlockSize, 4096);

            Assert.AreEqual(distributedArray.FindIndex(Is5000), 5000);
            Assert.AreEqual(distributedArray.FindIndex(0, 4999, Is5000), -1);
            Assert.AreEqual(distributedArray.FindIndex(Is128000), -1);
            Assert.AreEqual(distributedArray.FindIndex(5001, 1000, Is5000), -1);
        }
        [Test]
        public static void FindLastIndex()
        {
            var distributedArray = new DistributedArray<int>();
            for (int i = 0; i < distributedArray.MaxBlockSize * 2; i++)
                distributedArray.Add(i);
            for (int i = 0; i < distributedArray.MaxBlockSize * 2; i++) //For mistakes with duplicate elements
                distributedArray.Add(i);

            //If MaxBlockSize is change, we need to change this code
            Assert.AreEqual(distributedArray.MaxBlockSize, 4096);

            Assert.AreEqual(distributedArray.FindLastIndex(Is5000), 13192);
            Assert.AreEqual(distributedArray.FindLastIndex(4999, 5000, Is5000), -1);
            Assert.AreEqual(distributedArray.FindLastIndex(Is128000), -1);
            Assert.AreEqual(distributedArray.FindLastIndex(5001, 1000, Is5000), 5000);
        }
        [Test]
        public static void IndexOf()
        {
            var distributedArray = new DistributedArray<int>();
            for (int i = 0; i < distributedArray.MaxBlockSize * 2; i++)
                distributedArray.Add(i);
            for (int i = 0; i < distributedArray.MaxBlockSize * 2; i++) //For mistakes with duplicate elements
                distributedArray.Add(i);

            //If MaxBlockSize is change, we need to change this code
            Assert.AreEqual(distributedArray.MaxBlockSize, 4096);

            Assert.AreEqual(distributedArray.IndexOf(5000), 5000);
            Assert.AreEqual(distributedArray.IndexOf(0, 1, 5000), -1);
            Assert.AreEqual(distributedArray.IndexOf(128000), -1);
            Assert.AreEqual(distributedArray.IndexOf(5001, 0, 5000), -1);
        }

        //Support functions
        private static bool Is5000(int a)
        {
            return a == 5000;
        }
        private static bool Is128000(int a)
        {
            return a == 128000;
        }
    }
}
