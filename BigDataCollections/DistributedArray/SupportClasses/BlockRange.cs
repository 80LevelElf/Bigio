namespace BigDataCollections.DistributedArray.SupportClasses
{
    struct BlockRange
    {
        public BlockRange(int startSubindex, int count, int commonBlockStartIndex = -1)
        {
            StartSubindex = startSubindex;
            CommonBlockStartIndex = commonBlockStartIndex;
            Count = count;
        }

        public int StartSubindex;
        public int CommonBlockStartIndex;
        public int Count;
    }
}
