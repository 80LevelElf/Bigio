using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Bigio.BigArray.Support_Classes.BlockCollection;
using NUnit.Framework;
using UnitTests.Managers;

namespace UnitTests.Bigio_Tests.BigArray_Tests.Support_Classes_Tests
{
    [TestFixture]
    public static class BlockCollectionTest
    {
        [Test]
        public static void AddAndInsert()
        {
            var blockCollection = new BlockCollection<int>();
            blockCollection.Add(new Block<int> { 1 });
            blockCollection.Insert(0, new Block<int> { 0 });
            blockCollection.Insert(2, new Block<int> { 2 });
            blockCollection.Add(new Block<int> { 3 });

            var checkArray = new[] { 0, 1, 2, 3 };
            Assert.AreEqual(blockCollection.Count, checkArray.Length);

            for (int i = 0; i < checkArray.Length; i++)
            {
                Assert.AreEqual(blockCollection[i][0], checkArray[i]);
            }

            //Exceptions
            Assert.IsTrue(ExceptionManager.IsThrowActionException<ArgumentNullException, Block<int>>
                (blockCollection.Add, null));
            Assert.IsTrue(ExceptionManager.IsThrowActionException<ArgumentNullException, int, Block<int>>
                (blockCollection.Insert, 0, null));

            Assert.IsTrue(ExceptionManager.IsThrowActionException<ArgumentOutOfRangeException, int, Block<int>>
                (blockCollection.Insert, -1, new Block<int>()));
            Assert.IsTrue(ExceptionManager.IsThrowActionException<ArgumentOutOfRangeException, int, Block<int>>
                (blockCollection.Insert, blockCollection.Count + 1, new Block<int>()));
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
            blockCollection.AddRange(new List<Block<int>>
            {
                new Block<int> {1},
                new Block<int> {2}
            });
            blockCollection.InsertRange(0, new List<Block<int>>
            {
                new Block<int> {0}
            });
            blockCollection.AddRange(new List<Block<int>>
            {
                new Block<int> {3}
            });
            blockCollection.InsertRange(4, new List<Block<int>>
            {
                new Block<int> {4},
                new Block<int> {5}
            });

            var checkArray = new[] { 0, 1, 2, 3, 4, 5 };
            Assert.AreEqual(blockCollection.Count, checkArray.Length);

            for (int i = 0; i < checkArray.Length; i++)
            {
                Assert.AreEqual(blockCollection[i][0], checkArray[i]);
            }

            //Exceptions
            ExceptionManager.IsThrowActionException
                <ArgumentNullException, ICollection<Block<int>>>
                (blockCollection.AddRange, null);
            ExceptionManager.IsThrowActionException
                <ArgumentNullException, int, ICollection<Block<int>>>
                (blockCollection.InsertRange, 0, null);

            ExceptionManager.IsThrowActionException
                <ArgumentNullException, int, ICollection<Block<int>>>
                (blockCollection.InsertRange, -1, new Collection<Block<int>>());
            ExceptionManager.IsThrowActionException
                <ArgumentNullException, int, ICollection<Block<int>>>
                (blockCollection.InsertRange, blockCollection.Count + 1, new Collection<Block<int>>());
        }

        [Test]
        public static void Clear()
        {
            var blockCollection = CreateNewCollection();

            blockCollection.Clear();
            Assert.AreEqual(blockCollection.Count, 0); //It must have 0 empty blocks
        }

        [Test]
        public static void Contains()
        {
            var blockCollection = CreateNewCollection();

            Assert.IsTrue(blockCollection.Contains(blockCollection[0]));
            Assert.IsTrue(blockCollection.Contains(blockCollection[blockCollection.Count - 1]));

            Assert.IsFalse(blockCollection.Contains(new Block<int>()));
        }

        [Test]
        public static void CopyTo()
        {
            var blockCollection = new BlockCollection<int>();
            blockCollection.Add(new Block<int> {0, 1, 2});
            blockCollection.Add(new Block<int> {3, 4, 5});
            blockCollection.Add(new Block<int> {6, 7, 8});

            //Without shift
            var array1 = new Block<int>[3];
            blockCollection.CopyTo(array1, 0);
            for (int i = 0; i < blockCollection.Count; i++)
            {
                Assert.AreEqual(blockCollection[i], array1[i]);
            }

            //With shift
            var array2 = new Block<int>[4];
            blockCollection.CopyTo(array2, 1);
            for (int i = 0; i < blockCollection.Count; i++)
            {
                Assert.AreEqual(blockCollection[i], array2[i + 1]);
            }

            //Exceptions
            Assert.IsTrue(ExceptionManager.IsThrowActionException
                <ArgumentNullException, Block<int>[], int>
                (blockCollection.CopyTo, null, 0));

            Assert.IsTrue(ExceptionManager.IsThrowActionException
                <ArgumentOutOfRangeException, Block<int>[], int>
                (blockCollection.CopyTo, array1, array1.Length - blockCollection.Count + 1));
            Assert.IsTrue(ExceptionManager.IsThrowActionException
                <ArgumentOutOfRangeException, Block<int>[], int>
                (blockCollection.CopyTo, array1, -1));
        }

        [Test]
        public static void GetEnumerator()
        {
            var blockCollection = CreateNewCollection();
            var blockCollectionArray = blockCollection.ToArray();

            int counter = 0;
            foreach (var block in blockCollection)
            {
                Assert.AreEqual(block, blockCollectionArray[counter++]);
            }
        }

        [Test]
        public static void Indexer()
        {
            var blockCollection = CreateNewCollection();
            var blockCollectionArray = blockCollection.ToArray();

            for (int i = 0; i < blockCollection.Count; i++)
            {
                Assert.AreEqual(blockCollection[i], blockCollectionArray[i]);
            }
        }

        [Test]
        public static void Remove()
        {
            var blockCollection = CreateNewCollection();

            Assert.IsTrue(blockCollection.Remove(blockCollection[0]));
            Assert.IsTrue(blockCollection.Remove(blockCollection[blockCollection.Count - 1]));
            Assert.IsFalse(blockCollection.Remove(new Block<int>()));
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

            Assert.IsTrue(ExceptionManager.IsThrowActionException<ArgumentOutOfRangeException, int>
                (blockCollection.RemoveAt, -1));
            Assert.IsTrue(ExceptionManager.IsThrowActionException<ArgumentOutOfRangeException, int>
                (blockCollection.RemoveAt, blockCollection.Count));
        }

        [Test]
        public static void TryToDivideBlock()
        {
            var blockCollection = new BlockCollection<int>();
            blockCollection.AddNewBlock();

            //Fill block 2 time
            for (int i = 0; i < blockCollection.MaxBlockSize * 2; i++)
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
                blockCollection.Add(new Block<int> { i });
            }

            return blockCollection;
        }
    }
}
