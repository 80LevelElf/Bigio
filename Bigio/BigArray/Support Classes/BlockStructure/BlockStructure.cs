using System;
using System.Collections.Generic;
using Bigio.BigArray.Support_Classes.BlockCollection;
using Bigio.Common.Classes;
using Bigio.Common.Managers;

namespace Bigio.BigArray.Support_Classes.BlockStructure
{
    /// <summary>
    /// SearchMod is way to find needed data.
    /// </summary>
    enum SearchMod
    {
        /// <summary>
        /// This way to find block with specified index have log(2,n) performance,
        /// where n is count of blocks. But this way use information of blocks 
        /// and if we want to use this way, we'll must to calculate structure information before.
        /// It is good way to get BlockInfo if you won't change data in BigArray(T)
        /// after getting BlockInfo
        /// (For example if you want to get element in BigArray(T) with specified index).
        /// </summary>
        BinarySearch,
        /// <summary>
        /// This way to find block with specified index have n performance,
        /// where n is count of blocks. This way don't need any information about
        /// structure as BinarySearch and you must use it, if you will change data in BigArray(T)
        /// for example if you wany to find info about block to delete element inside this block.
        /// </summary>
        LinearSearch
    }

    /// <summary>
    /// This class used for get information about BlockCollection(T) structure,
    /// for example for searching block with specified index.
    /// </summary>
    partial class BlockStructure<T>
    {
        //API
        public BlockStructure(BlockCollection<T> blockCollection)
        {
            BlockCollection = blockCollection;

            _blocksInfo = new BlockInfo[blockCollection.Count];
            TryToUpdateStructureInfo();
        }

        /// <summary>
        /// Calculate information about block and location of block inside BlockCollection(T).
        /// </summary>
        /// <param name="index">Common zero-based index of element in BigArray(T).
        ///  It's to find parent block.</param>
        /// <param name="searchMod">SearchMod is way to find needed data.</param>
        public BlockInfo BlockInfo
            (int index, SearchMod searchMod = SearchMod.BinarySearch)
        {
            return BlockInfo(index, 0, searchMod);
        }

        /// <summary>
        /// Calculate information about block and location of block inside BlockCollection(T).
        /// </summary>
        /// <param name="index">Common zero-based index of element in BigArray(T). 
        /// It's to find parent block.</param>
        /// <param name="statBlockIndex">Function will try to find block, which containes
        /// specified index from statBlockIndex to last block of BlockCollection(T).
        /// It use to get better performance.</param>
        /// <param name="searchMod">SearchMod is way to find needed data.</param>
        public BlockInfo BlockInfo
            (int index, int statBlockIndex, SearchMod searchMod = SearchMod.BinarySearch)
        {
            return BlockInfo
                (index, new Range(statBlockIndex, _blockCollection.Count - statBlockIndex), searchMod);
        }

        /// <summary>
        /// Calculate information about block and location of block inside BlockCollection(T).
        /// </summary>
        /// <param name="index">Common zero-based index of element in BigArray(T).
        ///  It's to find parent block.</param>
        /// <param name="searchBlockRange">Function will try to find block, which containes
        /// specified index in this range of BlockCollection(T).
        /// It use to get better performance.</param>
        /// <param name="searchMod">SearchMod is way to find needed data.</param>
        public BlockInfo BlockInfo
            (int index, Range searchBlockRange, SearchMod searchMod = SearchMod.BinarySearch)
        {
            if (!ValidationManager.IsValidRange(_blockCollection.Count, searchBlockRange))
                throw new ArgumentOutOfRangeException("searchBlockRange");

            switch (searchMod)
            {
                case SearchMod.BinarySearch:
                    return BinaryBlockInfo(index, searchBlockRange);
                case SearchMod.LinearSearch:
                    if (!_isDataChanged)
                        return BinaryBlockInfo(index, searchBlockRange);

                    return LinearBlockInfo(index, searchBlockRange);
                default:
                    throw new ArgumentOutOfRangeException("searchMod");
            }
        }

        /// <summary>
        /// Calculate start zero-based common index of specified block.
        /// </summary>
        /// <param name="indexOfBlock">Index of block to get it's start index.</param>
        /// <returns>Start zero-based common index.</returns>
        public int BlockStartIndex(int indexOfBlock)
        {
            TryToUpdateStructureInfo();

            return _blocksInfo[indexOfBlock].StartIndexOfBlock;
        }

        /// <summary>
        /// Calculate index of block, witch containt element with specified zero-base index.
        /// </summary>
        /// <param name="index">Zero-base index of element to find parent block.</param>
        /// <param name="searchMod">SearchMod is way to find needed data.</param>
        /// <returns>Index of block witch containt element with specified zero-base index.</returns>
        public int IndexOfBlock(int index, SearchMod searchMod = SearchMod.BinarySearch)
        {
            return BlockInfo(index, searchMod).IndexOfBlock;
        }

        /// <summary>
        /// Calculate index of block, witch containt element with specified zero-base index.
        /// Function try to find it from startBlock block to last block of BlockCollection(T). 
        /// </summary>
        /// <param name="index">Zero-base index of element to find parent block.</param>
        /// <param name="startBlock">Start block of range to find block in it.
        /// It use to ger better performance.</param>
        /// <param name="searchMod">SearchMod is way to find needed data.</param>
        /// <returns>Index of block witch containt element with specified zero-base index.</returns>
        public int IndexOfBlock(int index, int startBlock, SearchMod searchMod = SearchMod.BinarySearch)
        {
            return BlockInfo(index, startBlock, searchMod).IndexOfBlock;
        }

        /// <summary>
        /// Calculate index of block witch containt element with specified zero-base index.
        /// Function try to find it in searchBlockRange range of BlockCollection(T). 
        /// </summary>
        /// <param name="index">Zero-base index of element to find parent block.</param>
        /// <param name="searchBlockRange">Range to find in it.</param>
        /// <param name="searchMod">SearchMod is way to find needed data.</param>
        /// <returns>Index of block witch containt element with specified zero-base index.</returns>
        public int IndexOfBlock
            (int index, Range searchBlockRange, SearchMod searchMod = SearchMod.BinarySearch)
        {
            return BlockInfo(index, searchBlockRange, searchMod).IndexOfBlock;
        }

        /// <summary>
        /// Calculate a block range for all blocks that overlap with specified range.
        /// Block range provide information about overlapping specified range and block.
        /// </summary>
        /// <param name="calcRange">Range to get multyblock range.</param>
        /// <param name="searchMod">SearchMod is way to find needed data.</param>
        /// <returns>Return MultyblockRange object provides information about overlapping of specified range and block.</returns>
        public MultyblockRange MultyblockRange
            (Range calcRange, SearchMod searchMod = SearchMod.BinarySearch)
        {
            TryToUpdateStructureInfo();

            if (!ValidationManager.IsValidRange(_countOfElements, calcRange))
                throw new ArgumentOutOfRangeException();

            //If user want to select empty block
            if (calcRange.Count == 0)
            {
                return (calcRange.Index == 0)
                    ? new MultyblockRange(0, 0, new BlockRange[0])
                    : new MultyblockRange
                        (BlockStartIndex(IndexOfBlock(calcRange.Index, searchMod)), 0, new BlockRange[0]);
            }

            int indexOfStartBlock = IndexOfBlock(calcRange.Index, searchMod);
            int indexOfEndBlock
                = IndexOfBlock(calcRange.Index + calcRange.Count - 1, indexOfStartBlock, searchMod);
            int countOfBlocks = indexOfEndBlock - indexOfStartBlock + 1;

            return new MultyblockRange(indexOfStartBlock, countOfBlocks
                , CalculateMultyblockRanges(calcRange, indexOfStartBlock, indexOfEndBlock));
        }

        /// <summary>
        /// Calculate a reverse block range for all blocks that overlap with specified range.
        /// Block range provide information about overlapping specified range and block.
        /// ReverseMultyblockRange start with last BlockRange(IndexOfStartBlock is index of last overlap block)
        /// so first element in ReverseMultyblockRange is information about last overlapping block.
        /// Every blockInformation object also contain REVERSE data(for example: blockInfo object of
        /// full overlapping of block with 100 element has Subindex as 99(index of last overlapping element).
        /// </summary>
        /// <param name="calcRange">REVERSE range to get multyblock range.
        /// For example: if you want to get ReverseMultyblockRange of array with one block with 100 element,
        /// you will must write this: structureObject.ReverseMultyblockRange(99, 100);
        /// </param>
        /// <param name="searchMod">SearchMod is way to find needed data.</param>
        /// <returns>Return reverse MultyblockRange object provides information about reverse overlapping of specified range and block.</returns>
        public MultyblockRange ReverseMultyblockRange
            (Range calcRange, SearchMod searchMod = SearchMod.BinarySearch)
        {
            if (calcRange.Index < 0 || calcRange.Count < 0) //Other checks are in the MultyblockRange() 
                throw new ArgumentOutOfRangeException();

            int reverseIndex = (calcRange.Index == 0 && calcRange.Count == 0)
                ? 0 : calcRange.Index - calcRange.Count + 1;
            var range = MultyblockRange(new Range(reverseIndex, calcRange.Count), searchMod);

            int indexOfStartBlock = range.IndexOfStartBlock + range.Count - 1;
            if (indexOfStartBlock < 0)
                indexOfStartBlock = 0;

            //Reverse all block in range
            var reverseBlockRanges = new BlockRange[range.Count];
            int counter = 0;
            foreach (var blockRange in range.Ranges)
            {
                var reverseBlockRange = new BlockRange(blockRange.Subindex + blockRange.Count - 1
                    , blockRange.Count, blockRange.CommonStartIndex);

                reverseBlockRanges[range.Count - counter - 1] = reverseBlockRange;
                counter++;
            }

            return new MultyblockRange(indexOfStartBlock, range.Count, reverseBlockRanges);
        }

        /// <summary>
        /// Parent collection of blocks, which structure is BlockStructure.
        /// </summary>
        public BlockCollection<T> BlockCollection
        {
            private set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _blockCollection = value;
            }
            get
            {
                return _blockCollection;
            }
        }

        /// <summary>
        /// You need to call this function after you changed data in BigArray
        /// to update structure of BlockStructure when it will be need to do.
        /// </summary>
        public void DataChanged()
        {
            _isDataChanged = true;
        }

        //Support classes
        /// <summary>
        /// Updates information about structure to use BinaryBlockInfo.
        /// </summary>
        private void TryToUpdateStructureInfo()
        {
            if (!_isDataChanged)
                return;

            int count = _blockCollection.Count;
            if (_blocksInfo.Length != count)
                _blocksInfo = new BlockInfo[count];

            int blockStartIndex = 0;
            _countOfElements = 0;

            for (int i = 0; i < count; i++)
            {
                var blockCount = _blockCollection[i].Count;

                //Get information
                _blocksInfo[i].StartIndexOfBlock = blockStartIndex;
                _blocksInfo[i].IndexOfBlock = i;
                _blocksInfo[i].Count = blockCount;

                blockStartIndex += blockCount;
                _countOfElements += blockCount;
            }

            _isDataChanged = false;
        }

        /// <summary>
        /// Performs lazy calculations to get blocks of MultyblockRange.
        /// </summary>
        /// <param name="calcRange">Range to get multyblock range.</param>
        /// <param name="indexOfStartBlock">Index of start block of range, which contain calcRange.</param>
        /// <param name="indexOfEndBlock">Index of end block of range, which contain calcRange.</param>
        /// <returns></returns>
        private IEnumerable<BlockRange> CalculateMultyblockRanges
            (Range calcRange, int indexOfStartBlock, int indexOfEndBlock)
        {
            var infoOfStartBlock = _blocksInfo[indexOfStartBlock];
            var currentStartIndex = infoOfStartBlock.StartIndexOfBlock;
            var currentEndIndex = infoOfStartBlock.StartIndexOfBlock;
            var endIndex = calcRange.Index + calcRange.Count - 1;

            for (int i = indexOfStartBlock; i <= indexOfEndBlock; i++)
            {
                var block = _blockCollection[i];
                currentEndIndex += block.Count;

                bool isLeftOverlap = (calcRange.Index <= currentStartIndex && currentStartIndex <= endIndex);
                bool isRightOverlap = (calcRange.Index <= currentEndIndex && currentEndIndex <= endIndex);
                bool isContain = (currentStartIndex <= calcRange.Index && endIndex <= currentEndIndex);

                // if ranges overlap
                if (isLeftOverlap || isRightOverlap || isContain)
                {
                    bool isRangeStartInThisBlock = calcRange.Index >= currentStartIndex;
                    bool isRangeEndInThisBlock = endIndex >= currentEndIndex;

                    int startSubindex = (isRangeStartInThisBlock) ? calcRange.Index - currentStartIndex : 0;
                    int rangeCount = (isRangeEndInThisBlock) ? block.Count - startSubindex
                        : endIndex - currentStartIndex - startSubindex + 1;

                    if (rangeCount >= 0)
                    {
                        yield return new BlockRange(startSubindex, rangeCount, currentStartIndex);
                    }
                }

                currentStartIndex += block.Count;
            }
        }

        /// <summary>
        /// This way to find block with specified index have log(2,n) performance,
        /// where n is count of blocks. But this way use information of blocks 
        /// and if we want to use this way, we'll must to calculate structure information before.
        /// It is good way to get BlockInfo if you won't change data in BigArray(T)
        /// after getting BlockInfo
        /// (For example if you want to get element in BigArray(T) with specified index).
        /// </summary>
        /// <param name="index">Index to find information about block, which containes it.</param>
        /// <param name="searchBlockRange">Range to find index in it. It use to get better performance.</param>
        /// <returns>Information about block, which contain specified index.</returns>
        private BlockInfo BinaryBlockInfo(int index, Range searchBlockRange)
        {
            TryToUpdateStructureInfo();

            //Check for validity
            if (!ValidationManager.IsValidIndex(_countOfElements, index))
                throw new ArgumentOutOfRangeException("index");

            if (searchBlockRange.Count == 0)
            {
                if (!_blocksInfo.IsValidIndex(searchBlockRange.Index))
                    throw new ArgumentOutOfRangeException("searchBlockRange");

                return new BlockInfo();
            }

            //We use some kind if binary search to find block with specified idex

            int indexOfStartBlock = searchBlockRange.Index;
            int indexOfEndBlock = indexOfStartBlock + searchBlockRange.Count - 1;

            // In code below we check is index in range. We don't do it before cycle 
            // to get better performance(suggestingPosition often is right and in this way we get
            // very bad performance)
            bool isIndexInCheckRange = false;

            while (indexOfStartBlock <= indexOfEndBlock)
            {
                //Calc middle position
                var startBlockInfo = _blocksInfo[indexOfStartBlock];
                var endBlockInfo = _blocksInfo[indexOfEndBlock];

                double startIndex = startBlockInfo.StartIndexOfBlock;
                double endIndex = endBlockInfo.StartIndexOfBlock + endBlockInfo.Count - 1;
                double countOfBlocks = endBlockInfo.IndexOfBlock - startBlockInfo.IndexOfBlock + 1;

                //We do it to check it only once 
                if (!isIndexInCheckRange)
                {
                    if (index < startIndex || index > endIndex)
                        throw new ArgumentOutOfRangeException("searchBlockRange");

                    isIndexInCheckRange = true;
                }

                double suggestingBlockPosition;
                if (index == startBlockInfo.StartIndexOfBlock)
                {
                    suggestingBlockPosition = startBlockInfo.IndexOfBlock;
                }
                else
                {
                    suggestingBlockPosition = indexOfStartBlock
                        + (index - startIndex) * countOfBlocks / (endIndex - startIndex + 1);
                }

                //Compare
                var newBlockPosition = (int)suggestingBlockPosition;
                var blockInfo = _blocksInfo[newBlockPosition];
                int result = blockInfo.Compare(index);
                switch (result)
                {
                    case -1:
                        indexOfEndBlock = newBlockPosition - 1;
                        break;
                    case 0:
                        return blockInfo;
                    case 1:
                        indexOfStartBlock = newBlockPosition + 1;
                        break;
                }
            }

            //We should not reach this string!
            throw new InvalidOperationException("There is no such index in specified range!");
        }

        /// <summary>
        /// This way to find block with specified index have n performance,
        /// where n is count of blocks. This way don't need any information about
        /// structure as BinarySearch and you must use it, if you will change data in BigArray(T)
        /// for example if you wany to find info about block to delete element inside this block.
        /// </summary>
        /// <param name="index">Index to find information about block, which containes it.</param>
        /// <param name="searchBlockRange">Range to find index in it. It use to get better performance.</param>
        /// <returns>Information about block, which contain specified index.</returns>
        private BlockInfo LinearBlockInfo(int index, Range searchBlockRange)
        {
            int indexOfBlock = 0;
            int blockStartIndex = 0;
            int blockCount = 0;
            bool isFind = false;

            //Move to start
            for (int i = 0; i < searchBlockRange.Index; i++)
            {
                blockStartIndex += _blockCollection[i].Count;
            }

            //Calc
            for (int i = searchBlockRange.Index; i < searchBlockRange.Index + searchBlockRange.Count; i++)
            {
                blockCount = _blockCollection[i].Count;

                //If there is needed block
                if (index >= blockStartIndex && index < blockStartIndex + blockCount)
                {
                    indexOfBlock = i;
                    isFind = true;
                    break;
                }

                blockStartIndex += blockCount;
            }

            if (!isFind)
                throw new ArgumentOutOfRangeException("index");

            return new BlockInfo(indexOfBlock, blockStartIndex, blockCount);
        }

        #region BinaryBlockInfo

        /*private BlockInfo BinaryBlockInfo_Multythread(int index, Range searchBlockRange)
        {
            TryToUpdateStructureInfo();

            //Check for validity
            if (!ValidationManager.IsValidIndex(_countOfElements, index))
                throw new ArgumentOutOfRangeException("index");

            if (searchBlockRange.Count == 0)
            {
                if (!_blocksInfo.IsValidIndex(searchBlockRange.Index))
                    throw new ArgumentOutOfRangeException("searchBlockRange");

                return new BlockInfo();
            }
        }*/

        #endregion

        //Data

        /// <summary>
        /// Parent _blockCollection to define structure of it.
        /// </summary>
        private BlockCollection<T> _blockCollection;

        /// <summary>
        /// Information about each block, which contain in _blockCollection.
        /// It can be defferent from real information if data changed and user don't update information.
        /// </summary>
        private BlockInfo[] _blocksInfo;

        /// <summary>
        /// If information changed and it is need to be update flag will be true, otherwise false.
        /// </summary>
        private bool _isDataChanged = true;

        /// <summary>
        /// It is a cache information of count of elements in BigArray(T).
        /// If data changed, it can be different from real count.
        /// </summary>
        private int _countOfElements;
    }
}
