namespace BigDataCollections.DistributedArray.Support
{
    struct BlockRange
    {
        public BlockRange(int startSubindex, int count)
        {
            StartSubindex = startSubindex;
            Count = count;
        }

        public int StartSubindex;
        public int Count;
    }
}
