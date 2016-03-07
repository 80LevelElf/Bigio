using System.Collections.Generic;
using Bigio.Common.Classes;

namespace Bigio.Common.Managers
{
    /// <summary>
    /// ValidationManager contain different validation functions for check validate
    /// data for collections. 
    /// </summary>
    static class ValidationManager
    {
        /// <summary>
        /// Check count to valid in the specified collection.
        /// </summary>
        /// <param name="collection">Collection which must to be check.</param>
        /// <param name="count">Count to check.</param>
        /// <returns>True if count is valid, otherwise return false.</returns>
        public static bool IsValidCount<T>(this ICollection<T> collection, int count)
        {
            return IsValidCount(collection.Count, count);
        }

        /// <summary>
        /// Check count to valid with the specified collection count.
        /// </summary>
        /// <param name="collectionCount">Count of collection which must to be check.</param>
        /// <param name="count">Count to check.</param>
        /// <returns>True if count is valid, otherwise return false.</returns>
        public static bool IsValidCount(int collectionCount, int count)
        {
            return count >= 0 && count <= collectionCount;
        }

        /// <summary>
        /// Check index to valid in current BigArray(T).
        /// </summary>
        /// <param name="collection">Collection which must to be check.</param>
        /// <param name="index">The zero-based starting index of the specified collection element.</param>
        /// <returns>True if index is valid, otherwise return false.</returns>
        public static bool IsValidIndex<T>(this ICollection<T> collection, int index)
        {
            return IsValidIndex(collection.Count, index);
        }

        /// <summary>
        /// Check index to valid in current BigArray(T).
        /// </summary>
        /// <param name="collectionCount">Count of collection which must to be check.</param>
        /// <param name="index">The zero-based starting index of the specified collection element.</param>
        /// <returns>True if index is valid, otherwise return false.</returns>
        public static bool IsValidIndex(int collectionCount, int index)
        {
            return index >= 0 && index < collectionCount;
        }

        /// <summary>
        /// Check range of the specified collection to valid.
        /// Index can be euqal to collection.Count
        /// </summary>
        /// <param name="collection">Collection to check.</param>
        /// <param name="index">The zero-based starting index of range of the specified collection to check.</param>
        /// <param name="count">The number of elements of the range to check.</param>
        /// <returns>Return true of range is valid, otherwise return false.</returns>
        public static bool IsValidRange<T>(this ICollection<T> collection, int index, int count)
        {
            return IsValidRange(collection.Count, index, count);
        }

        /// <summary>
        /// Check range of the specified collection to valid.
        /// Range index can be euqal to collection.Count
        /// </summary>
        /// <typeparam name="T">Type of collection elements.</typeparam>
        /// <param name="collection">Collection to check.</param>
        /// <param name="range">Range consist of specified index and count to check.</param>
        /// <returns>Return true of range is valid, otherwise return false.</returns>
        public static bool IsValidRange<T>(this ICollection<T> collection, Range range)
        {
            return IsValidRange(collection.Count, range.Index, range.Count);
        }

        /// <summary>
        /// Check range of the collection with specified count to valid.
        /// Index can be euqal to collectionCount
        /// </summary>
        /// <param name="collectionCount">Count of collection to check.</param>
        /// <param name="index">The zero-based starting index of range of the specified collection to check.</param>
        /// <param name="count">The number of elements of the range to check.</param>
        /// <returns>Return true of range is valid, otherwise return false.</returns>
        public static bool IsValidRange(int collectionCount, int index, int count)
        {
            return index >= 0 && count >= 0 && index + count <= collectionCount;
        }

        /// <summary>
        /// Check range of the collection with specified count to valid.
        /// Range index can be euqal to collectionCount
        /// </summary>
        /// <param name="collectionCount">Count of collection to check.</param>
        /// <param name="range">Range consist of specified index and count to check.</param>
        /// <returns>Return true of range is valid, otherwise return false.</returns>
        public static bool IsValidRange(int collectionCount, Range range)
        {
            return IsValidRange(collectionCount, range.Index, range.Count);
        }
    }
}
