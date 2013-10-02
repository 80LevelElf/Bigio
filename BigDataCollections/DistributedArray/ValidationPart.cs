using System.Collections.Generic;

namespace BigDataCollections
{
    public partial class DistributedArray<T>
    {
        /// <summary>
        /// Check range of the the current DistributedArray(T) to valid.
        /// </summary>
        /// <param name="index">The zero-based starting index of range of the DistributedArray(T) to check.</param>
        /// <param name="count">The number of elements of the range to check.</param>
        /// <returns>Return true of range is valid, otherwise return false.</returns>
        private bool IsValidRange(int index, int count)
        {
            return IsValidRange(this, index, count);
        }
        /// <summary>
        /// Check range of the specified collection to valid.
        /// </summary>
        /// <param name="collection">Collection which must to be check.</param>
        /// <param name="index">The zero-based starting index of range of the specified collection to check.</param>
        /// <param name="count">The number of elements of the range to check.</param>
        /// <returns>Return true of range is valid, otherwise return false.</returns>
        private static bool IsValidRange(ICollection<T> collection, int index, int count)
        {
            return !(index < 0 || index >= collection.Count || count < 0 || index + count > collection.Count);
        }
        /// <summary>
        /// Check index to valid in the current DistributedArray(T).
        /// </summary>
        /// <param name="index">The zero-based starting index of the DistributedArray(T) element.</param>
        /// <returns>True if index is valid, otherwise return false.</returns>
        private bool IsValidIndex(int index)
        {
            return IsValidIndex(this, index);
        }
        /// <summary>
        /// Check index to valid in current DistributedArray(T).
        /// </summary>
        /// <param name="collection">Collection which must to be check.</param>
        /// <param name="index">The zero-based starting index of the specified collection element.</param>
        /// <returns>True if index is valid, otherwise return false.</returns>
        private static bool IsValidIndex(ICollection<T> collection, int index)
        {
            return !(index < 0 || index >= collection.Count);
        }
        /// <summary>
        /// Check count to valid in the current DistributedArray(T).
        /// </summary>
        /// <param name="count">Count to check.</param>
        /// <returns>True if count is valid, otherwise return false.</returns>
        private bool IsValidCount(int count)
        {
            return IsValidCount(this, count);
        }
        /// <summary>
        /// Check count to valid in the specified collection.
        /// </summary>
        /// <param name="collection">Collection which must to be check.</param>
        /// <param name="count">Count to check.</param>
        /// <returns>True if count is valid, otherwise return false.</returns>
        private static bool IsValidCount(ICollection<T> collection, int count)
        {
            return !(count < 0 || count > collection.Count);
        }
    }
}
