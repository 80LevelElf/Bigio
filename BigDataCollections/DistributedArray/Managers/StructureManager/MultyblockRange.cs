using System;
using System.Collections.Generic;

namespace BigDataCollections.DistributedArray.Managers.StructureManager
{
    /// <summary>
    /// Object of MultyblockRange contain information of range that can overlap many blocks. 
    /// </summary>
    class MultyblockRange
    {
        //API
        /// <summary>
        /// Create new instance of MultyblockRange.
        /// </summary>
        /// <param name="indexOfStartBlock">Zero-based index of the start block in 
        /// BlockCollection.</param>
        /// <param name="count"></param>
        /// <param name="ranges">Block ranges of blocks from the start block.</param>
        public MultyblockRange(int indexOfStartBlock, int count, IEnumerable<BlockRange> ranges)
        {
            IndexOfStartBlock = indexOfStartBlock;
            Ranges = ranges;
            Count = count;
        }
        /// <summary>
        /// Count of BlockRanges containing in MultyblockRange.
        /// </summary>
        public int Count
        {
            get
            {
                return _count;
            }
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                _count = value;
            }
        }
        /// <summary>
        /// Zero-based index of the start block in BlockCollection.
        /// </summary>
        public int IndexOfStartBlock
        {
            get
            {
                return _indexOfStartBlock;
            }
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                _indexOfStartBlock = value;
            }
        }
        /// <summary>
        /// Block ranges of blocks from the start block.
        /// </summary>
        public IEnumerable<BlockRange> Ranges
        {
            get
            {
                return _ranges;
            }
            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                _ranges = value;
            }
        }

        //Data
        /// <summary>
        /// Internal value of IndexOfStartBlock field. Dont use it out of IndexOfStartBlock
        /// set and get functions.
        /// </summary>
        private int _indexOfStartBlock;
        /// <summary>
        /// Internal value of Ranges field. Dont use it out of Ranges
        /// set and get functions.
        /// </summary>
        private IEnumerable<BlockRange> _ranges;
        private int _count;
    }
}
