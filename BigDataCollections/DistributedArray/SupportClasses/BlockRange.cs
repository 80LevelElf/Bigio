namespace BigDataCollections.DistributedArray.SupportClasses
{
    struct BlockRange
    {
        //API
        public BlockRange(int subindex, int count, int commonBlockStartIndex = -1)
        {
            Subindex = subindex;
            CommonBlockStartIndex = commonBlockStartIndex;
            Count = count;
        }

        //Data
        public int Subindex;
        public int CommonBlockStartIndex;
        public int Count;
    }
}
