namespace BigDataCollections.DistributedArray.SupportClasses
{
    /// <summary>
    /// Object of this class contain information of position of block in BlockCollection.
    /// </summary>
    struct BlockInformation
    {
        //API
        /// <summary>
        /// Create new istance of BlockIformation.
        /// </summary>
        /// <param name="indexOfBlock">Zero-based index of block in the BlockCollection.</param>
        /// <param name="blockStartIndex">Zero-based start common index(not subindex) of block.</param>
        public BlockInformation(int indexOfBlock, int blockStartIndex)
        {
            IndexOfBlock = indexOfBlock;
            BlockStartIndex = blockStartIndex;
        }
        /// <summary>
        /// Zero-based start common index(not subindex) of block.
        /// </summary>
        public int BlockStartIndex;
        /// <summary>
        /// Zero-based index of block in the BlockCollection.
        /// </summary>
        public int IndexOfBlock;
    }
}
