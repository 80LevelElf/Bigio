using System;
using BigDataCollections.DistributedArray.SupportClasses;

namespace BigDataCollections.DistributedArray.Managers.StructureManager
{
    class StructureInfo
    {
        //API
        public StructureInfo(int countOfBlocks, int countOfElements)
            : this(new BlockInformation[countOfBlocks], countOfElements)
        {
        }
        public StructureInfo(BlockInformation[] informationOfBlocks, int countOfElements)
        {
            _blocks = informationOfBlocks;
            CountOfElements = countOfElements;
        }
        public BlockInformation this[int index]
        {
            get
            {
                if (index < 0 || index > _blocks.Length - 1)
                {
                    throw new ArgumentOutOfRangeException("index");
                }

                return _blocks[index];
            }
            set
            {
                if (index < 0 || index > _blocks.Length - 1)
                {
                    throw new ArgumentOutOfRangeException("index");
                }

                _blocks[index] = value;
            }
        }
        public int CountOfElements
        {
            get
            {
                return _countOfElements;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                _countOfElements = value;
            }
        }
        public int CountOfBlocks
        {
            get
            {
                return _blocks.Length;
            }
        }

        //Data
        private readonly BlockInformation[] _blocks;
        private int _countOfElements;
    }
}
