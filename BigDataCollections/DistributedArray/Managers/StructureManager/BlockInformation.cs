using BigDataCollections.DistributedArray.SupportClasses.BlockCollection;

namespace BigDataCollections.DistributedArray.Managers.StructureManager
{
    /// <summary>
    /// Object of this class contain information of position of block in BlockCollection.
    /// </summary>
    struct BlockInformation<T>
    {
        //API
        public BlockInformation(BlockInfo info)
        {
            IndexOfBlock = info.IndexOfBlock;
            StartIndex = info.StartIndex;
            Count = info.Count;
        }
        public BlockInformation(int indexOfBlock, int startIndex, int count)
        {
            StartIndex = startIndex;
            IndexOfBlock = indexOfBlock;
            Count = count;
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
        /// <summary>
        /// Zero-based start common index(not subindex) of block.
        /// </summary>
        public int StartIndex;
        /// <summary>
        /// Zero-based index of block in the BlockCollection.
        /// </summary>
        public int IndexOfBlock;
        public int Count;
    }
}
