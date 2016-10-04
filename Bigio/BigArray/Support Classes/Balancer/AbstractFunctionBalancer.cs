using System;
using System.Collections.Generic;
using Bigio.BigArray.Interfaces;

namespace Bigio.BigArray.Support_Classes.Balancer
{
    /// <summary>
    /// Function balancer with some function inside. We distribute block sizes during that function
    /// </summary>
    public abstract class AbstractFunctionBalancer : IBalancer
    {
        /// <summary>
        /// Possible sizes of blocks, where index is index of block and value is count of elements
        /// </summary>
        protected abstract List<int> PrecalculatedSizeList { get; }

        /// <summary>
        /// Index of last block which is really exists in <see cref="PrecalculatedSizeList"/>. If we want to get size of block with bigger index - 
        /// returns index of last existent block.
        /// </summary>
        protected abstract int MaxExistentIndex { get;}

        public virtual int GetNewBlockSize(int indexOfBlock)
        {
            return PrecalculatedSizeList[Math.Min(indexOfBlock, MaxExistentIndex)];
        }

        public virtual int GetDefaultBlockSize(int indexOfBlock)
        {
            return PrecalculatedSizeList[Math.Min(indexOfBlock, MaxExistentIndex)];
        }

        public virtual int GetMaxBlockSize(int indexOfBlock)
        {
            return PrecalculatedSizeList[Math.Min(indexOfBlock, MaxExistentIndex)] * 4;
        }
    }
}
