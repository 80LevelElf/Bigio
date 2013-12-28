namespace BigDataCollections.DistributedArray.Managers.StructureManager
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
        public BlockInformation(int indexOfBlock, int blockStartIndex, int count)
        {
            IndexOfBlock = indexOfBlock;
            BlockStartIndex = blockStartIndex;
            Count = count;
        }
        public int Compare(int index)
        {
            if (index < BlockStartIndex)
            {
                return -1;
            }
            if (index >= BlockStartIndex + Count)
            {
                return 1;
            }
            return 0;
        }
        /// <summary>
        /// Zero-based start common index(not subindex) of block.
        /// </summary>
        public int BlockStartIndex;
        /// <summary>
        /// Zero-based index of block in the BlockCollection.
        /// </summary>
        public int IndexOfBlock;
        public int Count;
    }
}
