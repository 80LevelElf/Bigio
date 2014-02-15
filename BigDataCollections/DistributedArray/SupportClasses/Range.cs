using System.Diagnostics;

namespace BigDataCollections.DistributedArray.SupportClasses
{
    /// <summary>
    /// Range is object containes information about some
    /// area inside any array. It consists of index of start element
    /// and count of element.
    /// </summary>
    struct Range
    {
        //API
        /// <summary>
        /// Create new instance of Range with specified data.
        /// </summary>
        /// <param name="index">Index of start element of range.</param>
        /// <param name="count">Count of elements in range.</param>
        public Range(int index, int count)
        {
            Index = index;
            Count = count;
        }
        public int IndexOfLastElement
        {
            get
            {
                if (Count == 0)
                {
                    return Index;
                }
                return Index + Count - 1;
            }
        }
        public static Range[] operator /(Range range, int count)
        {
            Debug.Assert(count >= 0, "Count can't be less than 0.");

            Range[] multyrange;
            int sizeOfOneRange = range.Count/count;

            //Calculate
            if (sizeOfOneRange != 0)
            {
                multyrange = new Range[count];

                int index = range.Index;
                for (int i = 0; i < count - 1; i++)
                {
                    multyrange[i] = new Range(index, sizeOfOneRange);
                    index += sizeOfOneRange;
                }

                // We need to add last subrange in separate code because count of it
                // can be not equal than 0. For example Range{index = 0, count = 10} / 3
                // = { Range{ 0, 3}, Range{3, 3}, Range{6, 4} }.
                multyrange[count - 1] = new Range(index, range.Count - index);
            }
            else
            {
                multyrange = new[] {range};
            }

            return multyrange;
        }
        /// <summary>
        /// Index of start element of range.
        /// </summary>
        public int Index;
        /// <summary>
        /// Count of elements in range.
        /// </summary>
        public int Count;
    }
}
