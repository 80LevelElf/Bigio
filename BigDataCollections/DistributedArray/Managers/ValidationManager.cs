using System.Collections.Generic;

namespace BigDataCollections.DistributedArray.Managers
{
    static class ValidationManager
    {
        /// <summary>
        /// Check range of the specified collection to valid.
        /// </summary>
        /// <param name="collection">Collection which must to be check.</param>
        /// <param name="index">The zero-based starting index of range of the specified collection to check.</param>
        /// <param name="count">The number of elements of the range to check.</param>
        /// <returns>Return true of range is valid, otherwise return false.</returns>
        public static bool IsValidRange<T>(ICollection<T> collection, int index, int count)
        {
            return !(index < 0 || index >= collection.Count || count < 0 || index + count > collection.Count);
        }
        /// <summary>
        /// Check index to valid in current DistributedArray(T).
        /// </summary>
        /// <param name="collection">Collection which must to be check.</param>
        /// <param name="index">The zero-based starting index of the specified collection element.</param>
        /// <returns>True if index is valid, otherwise return false.</returns>
        public static bool IsValidIndex<T>(ICollection<T> collection, int index)
        {
            return !(index < 0 || index >= collection.Count);
        }
        /// <summary>
        /// Check count to valid in the specified collection.
        /// </summary>
        /// <param name="collection">Collection which must to be check.</param>
        /// <param name="count">Count to check.</param>
        /// <returns>True if count is valid, otherwise return false.</returns>
        public static bool IsValidCount<T>(ICollection<T> collection, int count)
        {
            return !(count < 0 || count > collection.Count);
        }
    }
}
