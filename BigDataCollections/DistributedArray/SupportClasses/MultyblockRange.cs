using System;

namespace BigDataCollections.DistributedArray.SupportClasses
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
        /// <param name="ranges">Block ranges of blocks from the start block.</param>
        public MultyblockRange(int indexOfStartBlock, BlockRange[] ranges)
        {
            IndexOfStartBlock = indexOfStartBlock;
            _ranges = ranges;
        }

        //Data
        /// <summary>
        /// Indexator allowes you get and set BlockRange at specified index.
        /// </summary>
        /// <param name="index">Zero-based index of specified BlockRange to set and get it.</param>
        /// <returns>BlockRange object at specified index.</returns>
        public BlockRange this[int index]
        {
            get
            {
                return Ranges[index];
            }
            set
            {
                Ranges[index] = value;
            }
        }
        /// <summary>
        /// Count of BlockRanges containing in MultyblockRange.
        /// </summary>
        public int Count
        {
            get
            {
                return Ranges.Length;
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
            set
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
        public BlockRange[] Ranges
        {
            get
            {
                return _ranges;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                _ranges = value;
            }
        }
        /// <summary>
        /// Internal value of IndexOfStartBlock field. Dont use it out of IndexOfStartBlock
        /// set and get functions.
        /// </summary>
        private int _indexOfStartBlock;
        /// <summary>
        /// Internal value of Ranges field. Dont use it out of Ranges
        /// set and get functions.
        /// </summary>
        private BlockRange[] _ranges;
    }
}
