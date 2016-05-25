using System;
using Bigio.BigArray.Managers;
using Bigio.BigArray.Support_Classes.BlockCollection;
using Bigio.BigArray.Support_Classes.BlockStructure;
using Bigio.Common.Classes;
using NUnit.Framework;
using UnitTests.Managers;

namespace UnitTests.Bigio_Tests.BigArray_Tests.Support_Classes_Tests
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
            //Simple tests

            //Start element
            Assert.IsTrue(
                TestStructure.BlockInfo(0).Equals(new BlockInfo(0, 0, BlockSize)));
            //Simple element
            Assert.IsTrue(
                TestStructure.BlockInfo(BlockSize / 2).Equals(new BlockInfo(0, 0, BlockSize)));
            //First element of some block
            Assert.IsTrue(
                TestStructure.BlockInfo(BlockSize).Equals(new BlockInfo(1, BlockSize, BlockSize)));
            //Last element
            Assert.IsTrue(
                TestStructure.BlockInfo(CountOfBlocks * BlockSize - 1).Equals(
                new BlockInfo(CountOfBlocks - 1, (CountOfBlocks - 1) * BlockSize, BlockSize)));

            //With ranges

            //Start element in range
            Assert.IsTrue(TestStructure.BlockInfo(0, new Range(0, 1))
                .Equals(new BlockInfo(0, 0, BlockSize)));
            //Last element in range
            Assert.IsTrue(
                TestStructure.BlockInfo(CountOfBlocks * BlockSize - 1, new Range(CountOfBlocks - 1, 1))
                .Equals(new BlockInfo(CountOfBlocks - 1, (CountOfBlocks - 1) * BlockSize, BlockSize)));

            //Exceptions without ranges

            //-1 element
            Assert.IsTrue(
                ExceptionManager.IsThrowFuncException<ArgumentOutOfRangeException, int, BlockInfo>(
                TestStructure.BlockInfo, -1));
            //Element after last
            Assert.IsTrue(
                ExceptionManager.IsThrowFuncException<ArgumentOutOfRangeException, int, BlockInfo>(
                TestStructure.BlockInfo, CountOfBlocks * BlockSize));

            //Exceptions with ranges

            //Element at left towards range
            Assert.IsTrue(
                ExceptionManager.IsThrowFuncException<ArgumentOutOfRangeException, int, Range, BlockInfo>(
                TestStructure.BlockInfo, 0, new Range(1, 1)));
            //Element at right towards range
            Assert.IsTrue(
                ExceptionManager.IsThrowFuncException<ArgumentOutOfRangeException, int, Range, BlockInfo>(
                TestStructure.BlockInfo, 2, new Range(1, 1)));
            //Wrong left-side range with right location of element
            Assert.IsTrue(
                ExceptionManager.IsThrowFuncException<ArgumentOutOfRangeException, int, Range, BlockInfo>(
                TestStructure.BlockInfo, 0, new Range(-1, 2)));
            //Wrong right-side range with right location of element
            Assert.IsTrue(
                ExceptionManager.IsThrowFuncException<ArgumentOutOfRangeException, int, Range, BlockInfo>(
                TestStructure.BlockInfo, CountOfBlocks * BlockSize - 1, new Range(CountOfBlocks - 1, 2)));
        }

        [Test]
        public static void MultyblockRange()
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
                TestStructure.MultyblockRange(new Range(BlockSize / 2, 2 * BlockSize))
                .Equals(multyblockRange));

            //Exceptions

            //Left overlap
            Assert.IsTrue(
                ExceptionManager.IsThrowFuncException<ArgumentOutOfRangeException, Range, MultyblockRange>
                (TestStructure.MultyblockRange, new Range(-1, 2)));
            //Rigth overlap
            Assert.IsTrue(
                ExceptionManager.IsThrowFuncException<ArgumentOutOfRangeException, Range, MultyblockRange>
                (TestStructure.MultyblockRange, new Range(CountOfBlocks * BlockSize - 1, 2)));
            //Wrong count
            Assert.IsTrue(
                ExceptionManager.IsThrowFuncException<ArgumentOutOfRangeException, Range, MultyblockRange>
                (TestStructure.MultyblockRange, new Range(BlockSize, -2)));
        }

        [Test]
        public static void ReverseMultyblockRange()
        {
            //Simple test
            var reverseMultyblockRange = new MultyblockRange(2, 3,
                new[]
                {
                    new BlockRange(BlockSize*5/2, BlockSize/2, 2*BlockSize), 
                    new BlockRange(2*BlockSize - 1, BlockSize, BlockSize),
                    new BlockRange(BlockSize -1, BlockSize, 0)
                });

            Assert.IsTrue(
                TestStructure.ReverseMultyblockRange(new Range(BlockSize * 5 / 2, BlockSize * 5 / 2 + 1))
                .Equals(reverseMultyblockRange));

            //Exceptions

            //Left overlap
            Assert.IsTrue(
                ExceptionManager.IsThrowFuncException<ArgumentOutOfRangeException, Range, MultyblockRange>
                (TestStructure.ReverseMultyblockRange, new Range(0, 2)));
            //Rigth overlap
            Assert.IsTrue(
                ExceptionManager.IsThrowFuncException<ArgumentOutOfRangeException, Range, MultyblockRange>
                (TestStructure.ReverseMultyblockRange, new Range(CountOfBlocks * BlockSize, 2)));
            //Wrong count
            Assert.IsTrue(
                ExceptionManager.IsThrowFuncException<ArgumentOutOfRangeException, Range, MultyblockRange>
                (TestStructure.ReverseMultyblockRange, new Range(BlockSize, -2)));
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
