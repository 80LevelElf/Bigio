namespace Bigio.BigArray.Support_Classes.BlockStructure
{
    /// <summary>
    /// Object of BlockRange class contain information of some range inside the block.
    /// </summary>
    struct BlockRange
    {
        //API
        /// <summary>
        /// Create new instance of BlockRange.
        /// </summary>
        /// <param name="subindex">Subindex(internal block index) of starting of range.</param>
        /// <param name="count">Count of elements of the block.</param>
        /// <param name="commonStartIndex">Common start index(ot subindex)
        /// of the block.</param>
        public BlockRange(int subindex, int count, int commonStartIndex)
        {
            Subindex = subindex;
            CommonStartIndex = commonStartIndex;
            Count = count;
        }

        /// <summary>
        /// Check equal of current BlockRange and other BlockRange.
        /// </summary>
        /// <param name="other">Other BlockRange to check.</param>
        /// <returns>If BlockRanges are the same in all data members
        /// (CommonStartIndex, Count and Subindex) return true, otherwise return false.</returns>
        public bool Equals(BlockRange other)
        {
            return (CommonStartIndex == other.CommonStartIndex
                    && Count == other.Count
                    && Subindex == other.Subindex);
        }

        /// <summary>
        /// Common start index(not subindex) of the block.
        /// </summary>
        public int CommonStartIndex;

        /// <summary>
        /// Count of elements of the block.
        /// </summary>
        public int Count;

        /// <summary>
        /// Subindex(internal block index) of start of range.
        /// </summary>
        public int Subindex;
    }
}
