using System;
using System.Collections.Generic;
using BigDataCollections.DistributedArray.Managers.StructureManager;
using BigDataCollections.DistributedArray.SupportClasses;
using BigDataCollections.DistributedArray.SupportClasses.BlockCollection;

namespace BigDataCollections.DistributedArray.Managers
{
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
        public BlockInformation BlockInformation(int index)
        {
            TryToUpdateStructureInfo();

            if (!ValidationManager.IsValidIndex(_structureInfo.CountOfElements, index))
            {
                throw new ArgumentOutOfRangeException();
            }
            
            //We use some kind if binary search to find block with specified idex
            int indexOfStartBlock = 0;
            int indexOfEndBlock = _blockCollection.Count - 1;

            while (indexOfStartBlock <= indexOfEndBlock)
            {
                int middlePosition = (indexOfStartBlock + indexOfEndBlock) / 2;

                //Compare
                int result = _structureInfo[middlePosition].Compare(index);
                switch (result)
                {
                    case -1:
                        indexOfEndBlock = middlePosition - 1;
                        break;
                    case 0:
                        return _structureInfo[middlePosition];
                    case 1:
                        indexOfStartBlock = middlePosition + 1;
                        break;
                }
            }

            //We should not reach this string!
            throw new InvalidOperationException("There is BlockInformation error!");
        }
        /// <summary>
        /// Calculate start zero-based common index of specified block.
        /// </summary>
        /// <param name="indexOfBlock">Index of block we want to get start index.</param>
        /// <returns>Start zero-based common index</returns>
        public int BlockStartIndex(int indexOfBlock)
        {
            TryToUpdateStructureInfo();

            return _structureInfo[indexOfBlock].BlockStartIndex;
        }
        /// <summary>
        /// Calculate index of block witch containt element with specified zero-base index.
        /// </summary>
        /// <param name="index">Zero-base index of element situated in the block to find.</param>
        /// <returns>Index of block witch containt element with specified zero-base index.</returns>
        public int IndexOfBlock(int index)
        {
            var blockInfo = BlockInformation(index);
            return blockInfo.IndexOfBlock;
        }
        /// <summary>
        /// Calculate a block range for all blocks that overlap with specified range.
        /// Block range provide information about overlapping specified range and block.
        /// </summary>
        /// <param name="index">The zero-based starting index of range of the DistributedArray(T) to check.</param>
        /// <param name="count">The number of elements of the range to check.</param>
        /// <returns>Return MultyblockRange object provides information about overlapping of specified range and block.</returns>
        public MultyblockRange MultyblockRange(int index, int count)
        {
            TryToUpdateStructureInfo();

            if (!ValidationManager.IsValidRange(_structureInfo.CountOfElements, index, count))
            {
                throw new ArgumentOutOfRangeException();
            }

            var ranges = new List<BlockRange>();
            var currentStartIndex = 0;
            var currentEndIndex = -1;
            var endIndex = index + count - 1;

            int indexOfStartBlock = -1;

            //If user want to select empty block
            if (count == 0)
            {
                return (index == 0)
                    ? new MultyblockRange(0, new BlockRange[0])
                    : new MultyblockRange(BlockStartIndex(IndexOfBlock(index)), new BlockRange[0]);
            }

            for (int i = 0; i < _structureInfo.CountOfBlocks; i++)
            {
                var block = _structureInfo[i];
                currentEndIndex += block.Count;

                bool isLeftOverlap = (index <= currentStartIndex && currentStartIndex <= endIndex);
                bool isRightOverlap = (index <= currentEndIndex && currentEndIndex <= endIndex);
                bool isContain = (currentStartIndex <= index && endIndex <= currentEndIndex);

                // if ranges overlap
                if (isLeftOverlap || isRightOverlap || isContain)
                {
                    bool isRangeStartInThisBlock = index >= currentStartIndex;
                    bool isRangeEndInThisBlock = endIndex >= currentEndIndex;

                    int startSubindex = (isRangeStartInThisBlock) ? index - currentStartIndex : 0;
                    int rangeCount = (isRangeEndInThisBlock) ? block.Count - startSubindex
                        : endIndex - currentStartIndex - startSubindex + 1;

                    if (rangeCount >= 0)
                    {
                        ranges.Add(new BlockRange(startSubindex, rangeCount, currentStartIndex));

                        //We find indexOfStartBlock
                        if (indexOfStartBlock == -1)
                        {
                            indexOfStartBlock = i;
                        }
                    }
                }

                currentStartIndex += block.Count;
            }

            return new MultyblockRange(indexOfStartBlock, ranges.ToArray());
        }
        /// <summary>
        /// Calculate a reverse block range for all blocks that overlap with specified range.
        /// Block range provide information about overlapping specified range and block.
        /// Reverse MultyblockRange start with last BlockRange(IndexOfStartBlock is index of last overlap block)
        /// , but blocks in array are in the right order.
        /// </summary>
        /// <param name="index">The zero-based starting index of the backward calculation of overlapping.</param>
        /// <param name="count">The number of elements to overlapping calculate.</param>
        /// <returns>Return reverse MultyblockRange object provides information about reverse overlapping of specified range and block.</returns>
        public MultyblockRange ReverseMultyblockRange(int index, int count)
        {
            if (index < 0 || count < 0) //Other checks are in the MultyblockRange() 
            {
                throw new ArgumentOutOfRangeException();
            }

            int normalIndex = (index == 0 && count == 0) ? 0 : index - count + 1;
            var range = MultyblockRange(normalIndex, count);

            int indexOfStartBlock = range.IndexOfStartBlock + range.Count - 1;
            if (indexOfStartBlock < 0)
            {
                indexOfStartBlock = 0;
            }
            var reverseRange = new MultyblockRange(indexOfStartBlock, new BlockRange[range.Count]);

            //Reverse all block ranges
            for (int i = 0; i < range.Count; i++)
            {
                var currentBlockRange = range[i];
                var reverseBlockRange = new BlockRange(currentBlockRange.Subindex + currentBlockRange.Count - 1
                    , currentBlockRange.Count, currentBlockRange.CommonBlockStartIndex);

                reverseRange[i] = reverseBlockRange;
            }

            return reverseRange;
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

            var informationOfBlocks = new BlockInformation[_blockCollection.Count];
            int blockStartIndex = 0;

            for (int i = 0; i < _blockCollection.Count; i++)
            {
                var blockCount = _blockCollection[i].Count;

                //Get information
                informationOfBlocks[i].BlockStartIndex = blockStartIndex;
                informationOfBlocks[i].IndexOfBlock = i;
                informationOfBlocks[i].Count = blockCount;

                blockStartIndex += blockCount;
            }

            // Last value of blockStartIndex is index of first element of
            // non-existing block after last block. That's why last value of
            // blockStartIndex equal to count of elements.
            _structureInfo = new StructureInfo(informationOfBlocks, blockStartIndex);
            _isDataChanged = false;
        }

        //Data
        private StructureInfo _structureInfo;
        private BlockCollection<T> _blockCollection;
        private bool _isDataChanged = true;
    }
}
