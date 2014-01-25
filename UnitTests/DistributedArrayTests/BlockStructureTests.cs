using System;
using BigDataCollections.DistributedArray.Managers;
using BigDataCollections.DistributedArray.SupportClasses;
using BigDataCollections.DistributedArray.SupportClasses.BlockCollection;
using BigDataCollections.DistributedArray.SupportClasses.BlockStructure;
using NUnit.Framework;
using UnitTests.Managers;

namespace UnitTests.DistributedArrayTests
{
    [TestFixture]
    static class BlockStructureTests
    {
        //API
        static BlockStructureTests()
        {
            BlockSize = DefaultValuesManager.DefaultBlockSize;
            TestStructure = CteareTestStructure();
        }
        [Test]
        public static void BlockInfo()
        {
            BlockInfo(SearchMod.BinarySearch);
            BlockInfo(SearchMod.LinearSearch);
        }
        [Test]
        public static void MultyblockRange()
        {
            MultyblockRange(SearchMod.BinarySearch);
            MultyblockRange(SearchMod.LinearSearch);
        }
        [Test]
        public static void ReverseMultyblockRange()
        {
            ReverseMultyblockRange(SearchMod.BinarySearch);
            ReverseMultyblockRange(SearchMod.LinearSearch);
        }

        //Support
        private static void BlockInfo(SearchMod mod)
        {
            //Simple tests

            //Start element
            Assert.IsTrue(
                TestStructure.BlockInfo(0, mod).Equals(new BlockInfo(0, 0, BlockSize)));
            //Simple element
            Assert.IsTrue(
                TestStructure.BlockInfo(BlockSize / 2, mod).Equals(new BlockInfo(0, 0, BlockSize)));
            //First element of some block
            Assert.IsTrue(
                TestStructure.BlockInfo(BlockSize, mod).Equals(new BlockInfo(1, BlockSize, BlockSize)));
            //Last element
            Assert.IsTrue(
                TestStructure.BlockInfo(CountOfBlocks*BlockSize - 1, mod).Equals(
                new BlockInfo(CountOfBlocks - 1, (CountOfBlocks - 1)*BlockSize, BlockSize)));

            //With ranges

            //Start element in range
            Assert.IsTrue(TestStructure.BlockInfo(0, new Range(0, 1), mod)
                .Equals(new BlockInfo(0, 0, BlockSize)));
            //Last element in range
            Assert.IsTrue(
                TestStructure.BlockInfo(CountOfBlocks * BlockSize - 1,new Range(CountOfBlocks-1, 1), mod)
                .Equals(new BlockInfo(CountOfBlocks - 1, (CountOfBlocks - 1) * BlockSize, BlockSize)));

            //Exceptions without ranges

            //-1 element
            Assert.IsTrue(
                ExceptionManager.IsThrowException<ArgumentOutOfRangeException, int, SearchMod, BlockInfo>(
                TestStructure.BlockInfo, -1, mod));
            //Element after last
            Assert.IsTrue(
                ExceptionManager.IsThrowException<ArgumentOutOfRangeException, int, SearchMod, BlockInfo>(
                TestStructure.BlockInfo, CountOfBlocks*BlockSize, mod));

            //Exceptions with ranges

            //Element at left towards range
            Assert.IsTrue(
                ExceptionManager.IsThrowException<ArgumentOutOfRangeException, int, Range, SearchMod, BlockInfo>(
                TestStructure.BlockInfo, 0, new Range(1, 1),  mod));
            //Element at right towards range
            Assert.IsTrue(
                ExceptionManager.IsThrowException<ArgumentOutOfRangeException, int, Range, SearchMod, BlockInfo>(
                TestStructure.BlockInfo, 2, new Range(1, 1), mod));
            //Wrong left-side range with right location of element
            Assert.IsTrue(
                ExceptionManager.IsThrowException<ArgumentOutOfRangeException, int, Range, SearchMod, BlockInfo>(
                TestStructure.BlockInfo, 0, new Range(-1, 2), mod));
            //Wrong right-side range with right location of element
            Assert.IsTrue(
                ExceptionManager.IsThrowException<ArgumentOutOfRangeException, int, Range, SearchMod, BlockInfo>(
                TestStructure.BlockInfo, CountOfBlocks * BlockSize - 1, new Range(CountOfBlocks - 1, 2), mod));
        }
        private static void MultyblockRange(SearchMod mod)
        {
            //Simple test
            var multyblockRange = new MultyblockRange(0, 3, 
                new[]
                {
                    new BlockRange(BlockSize/2, BlockSize/2, 0),
                    new BlockRange(0, BlockSize, BlockSize),
                    new BlockRange(0, BlockSize/2, 2*BlockSize)
                });
            Assert.IsTrue(
                TestStructure.MultyblockRange(new Range(BlockSize/2, 2*BlockSize), mod)
                .Equals(multyblockRange));

            //Exceptions

            //Left overlap
            Assert.IsTrue(
                ExceptionManager.IsThrowException<ArgumentOutOfRangeException, Range, SearchMod, MultyblockRange>
                (TestStructure.MultyblockRange, new Range(-1, 2), mod));
            //Rigth overlap
            Assert.IsTrue(
                ExceptionManager.IsThrowException<ArgumentOutOfRangeException, Range, SearchMod, MultyblockRange>
                (TestStructure.MultyblockRange, new Range(CountOfBlocks*BlockSize - 1, 2), mod));
            //Wrong count
            Assert.IsTrue(
                ExceptionManager.IsThrowException<ArgumentOutOfRangeException, Range, SearchMod, MultyblockRange>
                (TestStructure.MultyblockRange, new Range(BlockSize, -2), mod));
        }
        private static void ReverseMultyblockRange(SearchMod mod)
        {
            //Simple test
            var reverseMultyblockRange = new MultyblockRange(0, 3,
                new[]
                {
                    new BlockRange(BlockSize*5/2, BlockSize/2, 2*BlockSize), 
                    new BlockRange(2*BlockSize - 1, BlockSize, BlockSize),
                    new BlockRange(BlockSize -1, BlockSize, 0)
                });
            Assert.IsTrue(
                TestStructure.ReverseMultyblockRange(new Range(BlockSize * 5 / 2, BlockSize * 5 / 2 + 1), mod)
                .Equals(reverseMultyblockRange));

            //Exceptions

            //Left overlap
            Assert.IsTrue(
                ExceptionManager.IsThrowException<ArgumentOutOfRangeException, Range, SearchMod, MultyblockRange>
                (TestStructure.ReverseMultyblockRange, new Range(0, 2), mod));
            //Rigth overlap
            Assert.IsTrue(
                ExceptionManager.IsThrowException<ArgumentOutOfRangeException, Range, SearchMod, MultyblockRange>
                (TestStructure.ReverseMultyblockRange, new Range(CountOfBlocks * BlockSize, 2), mod));
            //Wrong count
            Assert.IsTrue(
                ExceptionManager.IsThrowException<ArgumentOutOfRangeException, Range, SearchMod, MultyblockRange>
                (TestStructure.ReverseMultyblockRange, new Range(BlockSize, -2), mod));
        }
        private static BlockStructure<int> CteareTestStructure()
        {
            //Prepare block collection
            var blockCollection = new BlockCollection<int>();

            for (int i = 0; i < CountOfBlocks; i++)
            {
                var block = new Block<int>();
                for (int element = 0; element < BlockSize; element++)
                {
                    block.Add(element);
                }
                blockCollection.Add(block);
            }

            //Create block structure
            return new BlockStructure<int>(blockCollection);
        }

        //Data
        private static readonly BlockStructure<int> TestStructure;
        private static readonly int BlockSize;
        private const int CountOfBlocks = 4;
    }
}
