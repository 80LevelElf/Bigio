using System;

namespace Bigio.BigArray.Support_Classes.BlockStructure
{
    /// <summary>
    /// BlockInfo is object with iformation about block and its location.
    /// </summary>
    struct BlockInfo
    {
        /// <summary>
        /// Create new instance of BlockInfo with specified data.
        /// </summary>
        /// <param name="indexOfBlock">Index of block inside block collection.</param>
        /// <param name="commonStartIndex">Common start index of current block.</param>
        /// <param name="count">Count of elements inside current block.</param>
        public BlockInfo(int indexOfBlock, int commonStartIndex, int count)
        {
            IndexOfBlock = indexOfBlock;
            this.CommonStartIndex = commonStartIndex;
            Count = count;
        }

        /// <summary>
        /// Defines location specified index relative the block.
        /// </summary>
        /// <param name="index">Specified index to define relative location.</param>
        /// <returns>        
        /// If index locate before block, it will return -1.
        /// If index locate inside block, it will return 0.
        /// If index locate after block, it will return 1.</returns>
        public int Compare(int index)
        {
            if (index < CommonStartIndex)
                return -1;

            if (index >= CommonStartIndex + Count)
                return 1;

            return 0;
        }

        /// <summary>
        /// Check equal of current BlockInfo and other BlockIfo.
        /// </summary>
        /// <param name="other">Other BlockInfo to check.</param>
        /// <returns>If BlockInfos are the same in all data members
        /// (IndexOfBlock, CommonStartIndex and Count) return true, otherwise return false.</returns>
        public bool Equals(BlockInfo other)
        {
            return IndexOfBlock == other.IndexOfBlock && CommonStartIndex == other.CommonStartIndex && Count == other.Count;
        }

        /// <summary>
        /// Index of block inside block collection.
        /// </summary>
        public int IndexOfBlock;

        /// <summary>
        /// Common start index of current block. For example:
        /// if there is 2 blocks(both of 100 elements), then CommonStartIndex of first block is 0 and
        /// CommonStartIndex of second block is 100(because it starts by 100 element).
        /// </summary>
        public int CommonStartIndex;

        /// <summary>
        /// Count of elements inside current block.
        /// </summary>
        public int Count;
    }
}
