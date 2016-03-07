namespace Bigio.Common.Classes
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
