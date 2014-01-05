using BigDataCollections.DistributedArray.Managers;
using System;
using System.Collections.Generic;

namespace BigDataCollections.DistributedArray.SupportClasses.BlockCollection
{
    public class Block<T> : List<T>
    {
        //API
        public Block(BlockCollection<T> parent)
            : base(DefaultValuesManager.DefaultBlockSize)
        {
            Parent = parent;
        }
        public int Compare(int index)
        {
            if (index < StartIndex)
            {
                return -1;
            }
            if (index >= StartIndex + Count)
            {
                return 1;
            }
            return 0;
        }
        public int IndexOfBlock
        {
            get
            {
                return _indexOfBlock;
            }
            set
            {
                if (value < 0 || value >= Parent.Count)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                _indexOfBlock = value;
            }
        }
        public BlockCollection<T> Parent
        {
            get
            {
                return _parent;
            }
            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                _parent = value;
            }
        }
        public int StartIndex
        {
            get
            {
                return _startIndex;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                _startIndex = value;
            }
        }

        //Data
        private int _indexOfBlock;
        private BlockCollection<T> _parent;
        private int _startIndex;
    }
}