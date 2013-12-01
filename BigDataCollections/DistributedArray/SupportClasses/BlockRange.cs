namespace BigDataCollections.DistributedArray.SupportClasses
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
        /// <param name="commonBlockStartIndex">Common start index(ot subindex)
        /// of the block.</param>
        public BlockRange(int subindex, int count, int commonBlockStartIndex)
        {
            Subindex = subindex;
            CommonBlockStartIndex = commonBlockStartIndex;
            Count = count;
        }
        /// <summary>
        /// Count of elements of the block.
        /// </summary>
        public int CommonBlockStartIndex;
        /// <summary>
        /// Common start index(ot subindex) of the block.
        /// </summary>
        public int Count;
        /// <summary>
        /// Subindex(internal block index) of starting of range.
        /// </summary>
        public int Subindex;
    }
}
