using System;
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
        protected abstract int[] PrecalculatedSizeArray { get; }

        public virtual int GetNewBlockSize(int indexOfBlock)
        {
			return PrecalculatedSizeArray[Math.Min(indexOfBlock, PrecalculatedSizeArray.Length)];
        }

        public virtual int GetDefaultBlockSize(int indexOfBlock)
        {
			return PrecalculatedSizeArray[Math.Min(indexOfBlock, PrecalculatedSizeArray.Length)];
        }

        public virtual int GetMaxBlockSize(int indexOfBlock)
        {
			return PrecalculatedSizeArray[Math.Min(indexOfBlock, PrecalculatedSizeArray.Length)] * 4;
        }
    }
}
