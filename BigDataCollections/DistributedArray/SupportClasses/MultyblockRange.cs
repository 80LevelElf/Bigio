using System;

namespace BigDataCollections.DistributedArray.SupportClasses
{
    class MultyblockRange
    {
        //API
        public MultyblockRange(int indexOfStartBlock, BlockRange[] ranges)
        {
            IndexOfStartBlock = indexOfStartBlock;
            _ranges = ranges;
        }
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
        public int Count
        {
            get
            {
                return Ranges.Length;
            }
        }
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
        public BlockRange this[int index]
        {
            get
            {
                return Ranges[index];
            }
        }

        //Data
        private int _indexOfStartBlock;
        private BlockRange[] _ranges;
    }
}
