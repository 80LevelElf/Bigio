namespace Bigio.BigArray.Support_Classes.BlockStructure
{
    /// <summary>
    /// Object of <see cref="BlockRange"/> class contain information of some range inside the block.
    /// </summary>
    struct BlockRange
    {
        //API
        /// <summary>
        /// Create new instance of <see cref="BlockRange"/>.
        /// </summary>
        /// <param name="subindex">Subindex(internal block index) of starting of range.</param>
        /// <param name="count">Count of elements of the block.</param>
        /// <param name="commonStartIndex">Common zero-based index(across all collection) of block start of the block.</param>
        public BlockRange(int subindex, int count, int commonStartIndex)
        {
            Subindex = subindex;
            CommonStartIndex = commonStartIndex;
            Count = count;
        }

        /// <summary>
        /// Check equal of current <see cref="BlockRange"/> and other <see cref="BlockRange"/>.
        /// </summary>
        /// <param name="other">Other <see cref="BlockRange"/> to check.</param>
        /// <returns>If <see cref="BlockRange"/>s are the same in all data members
        /// (<see cref="CommonStartIndex"/>, <see cref="Count"/> and <see cref="Subindex"/>) return true, otherwise return false.</returns>
        public bool Equals(BlockRange other)
        {
            return (CommonStartIndex == other.CommonStartIndex
                    && Count == other.Count
                    && Subindex == other.Subindex);
        }

        /// <summary>
        /// Common zero-based index(across all collection) of block start of the block.
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
