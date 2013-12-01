using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BigDataCollections.DistributedArray.SupportClasses.BlockCollection;
using NUnit.Framework;
using UnitTests.Managers;

namespace UnitTests.DistributedArrayTests
{
    [TestFixture]
    public static class BlockCollectionTest
    {
        [Test]
        public static void AddAndInsert()
        {
            var blockCollection = new BlockCollection<int>();
            blockCollection.Add(new List<int> { 1 });
            blockCollection.Insert(0, new List<int> { 0 });
            blockCollection.Insert(2, new List<int> { 2 });
            blockCollection.Add(new List<int> { 3 });

            var arrayToCheck = new[] {0, 1, 2, 3};
            Assert.AreEqual(blockCollection.Count, arrayToCheck.Length);

            for (int i = 0; i < arrayToCheck.Length; i++)
            {
                Assert.AreEqual(blockCollection[i][0], arrayToCheck[i]);
            }

            //Exceptions
            Assert.IsTrue(ExceptionManager.IsThrowException<ArgumentNullException, List<int>>
                (blockCollection.Add, null));
            Assert.IsTrue(ExceptionManager.IsThrowException<ArgumentNullException, int, List<int>>
                (blockCollection.Insert, 0, null));

            Assert.IsTrue(ExceptionManager.IsThrowException<ArgumentOutOfRangeException, int, List<int>>
                (blockCollection.Insert, -1, new List<int>()));
            Assert.IsTrue(ExceptionManager.IsThrowException<ArgumentOutOfRangeException, int, List<int>>
                (blockCollection.Insert, blockCollection.Count + 1, new List<int>()));
        }
        [Test]
        public static void AddNewBlockAndInsertNewBlock()
        {
            var blockCollection = new BlockCollection<int>();

            //Add
            blockCollection.AddNewBlock();
            Assert.AreEqual(blockCollection.Count, 1);
            //Add
            blockCollection.AddNewBlock();
            Assert.AreEqual(blockCollection.Count, 2);
            //Insert
            blockCollection.InsertNewBlock(1);
            Assert.AreEqual(blockCollection.Count, 3);
        }
        [Test]
        public static void AddRangeAndInsertRange()
        {
            var blockCollection = new BlockCollection<int>();
            blockCollection.AddRange(new List<List<int>>
            {
                new List<int> {1},
                new List<int> {2}
            });
            blockCollection.InsertRange(0, new List<List<int>>
            {
                new List<int> {0}
            });
            blockCollection.AddRange(new List<List<int>>
            {
                new List<int> {3}
            });
            blockCollection.InsertRange(4, new List<List<int>>
            {
                new List<int> {4},
                new List<int> {5}
            });

            var arrayToCheck = new[] { 0, 1, 2, 3, 4, 5 };
            Assert.AreEqual(blockCollection.Count, arrayToCheck.Length);

            for (int i = 0; i < arrayToCheck.Length; i++)
            {
                Assert.AreEqual(blockCollection[i][0], arrayToCheck[i]);
            }

            //Exceptions
            ExceptionManager.IsThrowException
                <ArgumentNullException, ICollection<List<int>>>
                (blockCollection.AddRange, null);
            ExceptionManager.IsThrowException
                <ArgumentNullException, int, ICollection<List<int>>>
                (blockCollection.InsertRange, 0, null);

            ExceptionManager.IsThrowException
                <ArgumentNullException, int, ICollection<List<int>>>
                (blockCollection.InsertRange, -1, new Collection<List<int>>());
            ExceptionManager.IsThrowException
                <ArgumentNullException, int, ICollection<List<int>>>
                (blockCollection.InsertRange, blockCollection.Count + 1, new Collection<List<int>>());
        }
        [Test]
        public static void Clear()
        {
            var blockCollection = CreateNewCollection();

            blockCollection.Clear();
            Assert.AreEqual(blockCollection.Count, 1); //It must have 1 empty block
        }
        [Test]
        public static void Contains()
        {
            var blocksCollection = CreateNewCollection();

            Assert.IsTrue(blocksCollection.Contains(blocksCollection[0]));
            Assert.IsTrue(blocksCollection.Contains(blocksCollection[blocksCollection.Count - 1]));

            Assert.IsFalse(blocksCollection.Contains(new List<int>()));
        }
        [Test]
        public static void CopyTo()
        {
            var blockCollection = new BlockCollection<int>
            {
                new List<int> {0,1,2},
                new List<int> {3,4,5},
                new List<int> {6,7,8}
            };

            //Without shift
            var list1 = new List<int>[3];
            blockCollection.CopyTo(list1, 0);
            for (int i = 0; i < blockCollection.Count; i++)
            {
                Assert.AreEqual(blockCollection[i], list1[i]);
            }

            //With shift
            var list2 = new List<int>[4];
            blockCollection.CopyTo(list2, 1);
            for (int i = 0; i < blockCollection.Count; i++)
            {
                Assert.AreEqual(blockCollection[i], list2[i + 1]);
            }

            //Exceptions
            Assert.IsTrue(ExceptionManager.IsThrowException
                <ArgumentNullException, List<int>[], int>
                (blockCollection.CopyTo, null, 0));

            Assert.IsTrue(ExceptionManager.IsThrowException
                <ArgumentOutOfRangeException, List<int>[], int>
                (blockCollection.CopyTo, list1, list1.Length - blockCollection.Count + 1));
            Assert.IsTrue(ExceptionManager.IsThrowException
                <ArgumentOutOfRangeException, List<int>[], int>
                (blockCollection.CopyTo, list1, -1));
        }
        [Test]
        public static void GetEnumerator()
        {
            var blockCollection = CreateNewCollection();
            var array = blockCollection.ToArray();

            int counter = 0;
            foreach (var block in blockCollection)
            {
                Assert.AreEqual(block, array[counter++]);
            }
        }
        [Test]
        public static void Indexer()
        {
            var blockCollection = CreateNewCollection();
            var array = blockCollection.ToArray();

            for (int i = 0; i < blockCollection.Count; i++)
            {
                Assert.AreEqual(blockCollection[i], array[i]);
            }
        }
        [Test]
        public static void Remove()
        {
            var blockCollection = CreateNewCollection();
            
            Assert.IsTrue(blockCollection.Remove(blockCollection[0]));
            Assert.IsTrue(blockCollection.Remove(blockCollection[blockCollection.Count - 1]));
            Assert.IsFalse(blockCollection.Remove(new List<int>()));
        }
        [Test]
        public static void RemoveAt()
        {
            var blockCollection = CreateNewCollection();
            var list = blockCollection.ToList();

            blockCollection.RemoveAt(0);
            list.RemoveAt(0);

            blockCollection.RemoveAt(blockCollection.Count - 1);
            list.RemoveAt(list.Count - 1);

            //Check for equal
            for (int i = 0; i < blockCollection.Count; i++)
            {
                Assert.AreEqual(blockCollection[i], list[i]);
            }

            Assert.IsTrue(ExceptionManager.IsThrowException<ArgumentOutOfRangeException, int>
                (blockCollection.RemoveAt, -1));
            Assert.IsTrue(ExceptionManager.IsThrowException<ArgumentOutOfRangeException, int>
                (blockCollection.RemoveAt, blockCollection.Count));
        }
        [Test]
        public static void TryToDivideBlock()
        {
            var blockCollection = new BlockCollection<int>();
            blockCollection.AddNewBlock();

            //Fill block 2 time
            for (int i = 0; i < blockCollection.MaxBlockSize*2; i++)
            {
                blockCollection[0].Add(i);
            }

            Assert.AreEqual(blockCollection.Count, 1);

            blockCollection.TryToDivideBlock(0);

            Assert.AreEqual(blockCollection.Count, 
                2 * blockCollection.MaxBlockSize / blockCollection.DefaultBlockSize);
        }

        //Support functions
        private static BlockCollection<int> CreateNewCollection(int countOfBlocks = 8)
        {
            var blockCollection = new BlockCollection<int>();
            for (int i = 0; i < countOfBlocks; i++)
            {
                blockCollection.Add(new List<int>{i});
            }

            return blockCollection;
        }
    }
}
