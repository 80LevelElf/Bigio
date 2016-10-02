namespace Bigio.BigArray.Support_Classes.ArrayMap
{
    /// <summary>
    /// Helper entity to make storing information about cached element count in <see cref="ArrayMap{T}"/> more structured.
    /// </summary>
    internal struct CachedCountInfo
    {
        /// <summary>
        /// Create new <see cref="CachedCountInfo"/> with specified <see cref="CachedIndexOfFirstChangedBlock"/> and <see cref="CachedCount"/>
        /// </summary>
        public CachedCountInfo(int cachedIndexOfFirstChangedBlock, int cachedCount)
        {
            CachedIndexOfFirstChangedBlock = cachedIndexOfFirstChangedBlock;
            CachedCount = cachedCount;
        }

        /// <summary>
        /// There we store index of first changed block of <see cref="ArrayMap{T}"/>. We the help of this value we can
        /// understand - is cache out of date?
        /// </summary>
        public int CachedIndexOfFirstChangedBlock;

        /// <summary>
        /// Cached count of elements in <see cref="ArrayMap{T}"/>. If <see cref="CachedIndexOfFirstChangedBlock"/> value lead us to
        /// current <see cref="CachedCountInfo"/> is valid then we can use <see cref="CachedCount"/> to prevent calculcation it one more time.
        /// </summary>
        public int CachedCount;
    }
}
