using System;
using System.Collections.Generic;
using BigDataCollections.DistributedArray.Managers.StructureManager;
using BigDataCollections.DistributedArray.SupportClasses;
using BigDataCollections.DistributedArray.SupportClasses.BlockCollection;

namespace BigDataCollections.DistributedArray.Managers
{
    public enum SearchMod
    {
        BinarySearch,
        LinearSearch
    }
    class StructureManager<T>
    {
        //API
        public StructureManager(BlockCollection<T> blockCollection)
        {
            BlockCollection = blockCollection;
            TryToUpdateStructureInfo();
        }
        /// <summary>
        /// Calculate indexOfBlock and blockStartIndex by index. 
        /// </summary>
        /// <param name="index">Common index of element in DistributedArray(T). index = [0; Count).</param>
        public BlockInformation<T> BlockInformation
            (int index, SearchMod searchMod = SearchMod.BinarySearch)
        {
            return BlockInformation(index, 0, searchMod);
        }
        public BlockInformation<T> BlockInformation
            (int index, int statBlock, SearchMod searchMod = SearchMod.BinarySearch)
        {
            return BlockInformation
                (index, new Range(statBlock, _blockCollection.Count - statBlock), searchMod);
        }
        public BlockInformation<T> BlockInformation
            (int index, Range searchBlockRange, SearchMod searchMod = SearchMod.BinarySearch)
        {
            switch (searchMod)
            {
                case SearchMod.BinarySearch:
                    return BinaryBlockInformation(index, searchBlockRange);
                case SearchMod.LinearSearch:
                    return LinearBlockInformation(index, searchBlockRange);
                default:
                    throw new ArgumentOutOfRangeException("searchMod");
            }
        }
        /// <summary>
        /// Calculate start zero-based common index of specified block.
        /// </summary>
        /// <param name="indexOfBlock">Index of block we want to get start index.</param>
        /// <returns>Start zero-based common index</returns>
        public int BlockStartIndex(int indexOfBlock)
        {
            TryToUpdateStructureInfo();

            return _blockCollection[indexOfBlock].StartIndex;
        }
        /// <summary>
        /// Calculate index of block witch containt element with specified zero-base index.
        /// </summary>
        /// <param name="index">Zero-base index of element situated in the block to find.</param>
        /// <returns>Index of block witch containt element with specified zero-base index.</returns>
        public int IndexOfBlock(int index, SearchMod searchMod = SearchMod.BinarySearch)
        {
            return BlockInformation(index, searchMod).IndexOfBlock;
        }
        public int IndexOfBlock(int index, int startBlock, SearchMod searchMod = SearchMod.BinarySearch)
        {
            return BlockInformation(index, startBlock, searchMod).IndexOfBlock;
        }
        public int IndexOfBlock
            (int index, Range searchBlockRange, SearchMod searchMod = SearchMod.BinarySearch)
        {
            return BlockInformation(index, searchBlockRange, searchMod).IndexOfBlock;
        }
        /// <summary>
        /// Calculate a block range for all blocks that overlap with specified range.
        /// Block range provide information about overlapping specified range and block.
        /// </summary>
        /// <param name="index">The zero-based starting index of range of the DistributedArray(T) to check.</param>
        /// <param name="count">The number of elements of the range to check.</param>
        /// <returns>Return MultyblockRange object provides information about overlapping of specified range and block.</returns>
        public MultyblockRange MultyblockRange
            (Range calcRange, SearchMod searchMod = SearchMod.BinarySearch)
        {
            TryToUpdateStructureInfo();

            if (!ValidationManager.IsValidRange(_countOfElements, calcRange))
            {
                throw new ArgumentOutOfRangeException();
            }

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
                , CalculateBlockRanges(calcRange, indexOfStartBlock, indexOfEndBlock));
        }
        /// <summary>
        /// Calculate a reverse block range for all blocks that overlap with specified range.
        /// Block range provide information about overlapping specified range and block.
        /// Reverse MultyblockRange start with last BlockRange(IndexOfStartBlock is index of last overlap block).
        /// </summary>
        /// <param name="index">The zero-based starting index of the backward calculation of overlapping.</param>
        /// <param name="count">The number of elements to overlapping calculate.</param>
        /// <returns>Return reverse MultyblockRange object provides information about reverse overlapping of specified range and block.</returns>
        public MultyblockRange ReverseMultyblockRange
            (Range calcRange, SearchMod searchMod = SearchMod.BinarySearch)
        {
            if (calcRange.Index < 0 || calcRange.Count < 0) //Other checks are in the MultyblockRange() 
            {
                throw new ArgumentOutOfRangeException();
            }

            int reverseIndex = (calcRange.Index == 0 && calcRange.Count == 0)
                ? 0 : calcRange.Index - calcRange.Count + 1;
            var range = MultyblockRange(new Range(reverseIndex, calcRange.Count), searchMod);

            int indexOfStartBlock = range.IndexOfStartBlock + range.Count - 1;
            if (indexOfStartBlock < 0)
            {
                indexOfStartBlock = 0;
            }

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
        public BlockCollection<T> BlockCollection
        {
            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                _blockCollection = value;
            }
            get
            {
                return _blockCollection;
            }
        }
        public void DataChanged()
        {
            _isDataChanged = true;
        }

        //Support classes
        private void TryToUpdateStructureInfo()
        {
            if (!_isDataChanged)
            {
                return;
            }

            int blockStartIndex = 0;
            _countOfElements = 0;

            for (int i = 0; i < _blockCollection.Count; i++)
            {
                var block = _blockCollection[i];
                var count = block.Count;

                //Get information
                block.StartIndex = blockStartIndex;
                block.IndexOfBlock = i;

                blockStartIndex += count;
                _countOfElements += count;
            }

            _isDataChanged = false;
        }
        private IEnumerable<BlockRange> CalculateBlockRanges
            (Range calcRange, int indexOfStartBlock, int indexOfEndBlock)
        {
            var infoOfStartBlock = _blockCollection[indexOfStartBlock];
            var currentStartIndex = infoOfStartBlock.StartIndex;
            var currentEndIndex = infoOfStartBlock.StartIndex;
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
        private BlockInformation<T> BinaryBlockInformation(int index, Range searchBlockRange)
        {
            TryToUpdateStructureInfo();

            //Check for validity
            if (!ValidationManager.IsValidIndex(_countOfElements, index))
            {
                throw new ArgumentOutOfRangeException("index");
            }

            if (!ValidationManager.IsValidRange(_blockCollection.Count, searchBlockRange))
            {
                throw new ArgumentOutOfRangeException("searchBlockRange");
            }

            if (searchBlockRange.Count == 0)
            {
                return new BlockInformation<T>();
            }

            //We use some kind if binary search to find block with specified idex
            int indexOfStartBlock = searchBlockRange.Index;
            int indexOfEndBlock = indexOfStartBlock + searchBlockRange.Count - 1;

            while (indexOfStartBlock <= indexOfEndBlock)
            {
                int middlePosition = (indexOfStartBlock + indexOfEndBlock) / 2;

                //Compare
                var middleBlock = _blockCollection[middlePosition];
                int result = middleBlock.Compare(index);
                switch (result)
                {
                    case -1:
                        indexOfEndBlock = middlePosition - 1;
                        break;
                    case 0:
                        return new BlockInformation<T>(middleBlock);
                    case 1:
                        indexOfStartBlock = middlePosition + 1;
                        break;
                }
            }

            //We should not reach this string!
            throw new InvalidOperationException("There is no such index in specified range!");
        }
        private BlockInformation<T> LinearBlockInformation(int index, Range searchBlockRange)
        {
            int indexOfBlock = 0;
            int blockStartIndex = 0;
            int blockCount = 0;

            //Move to start
            for (int i = 0; i < searchBlockRange.Index; i++)
            {
                blockStartIndex += _blockCollection[i].Count;
            }

            //Calc
            for (int i = searchBlockRange.Index; i < searchBlockRange.Index + searchBlockRange.Count; i++)
            {
                var block = _blockCollection[i];
                blockCount = block.Count;

                //If there is needed block
                if (index >= blockStartIndex && index < blockStartIndex + blockCount)
                {
                    indexOfBlock = i;
                    break;
                }

                blockStartIndex += blockCount;
            }

            return new BlockInformation<T>(indexOfBlock, blockStartIndex, blockCount);
        }

        //Data
        private BlockCollection<T> _blockCollection;
        private bool _isDataChanged = true;
        private int _countOfElements;
    }
}
