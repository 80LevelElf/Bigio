using System.Collections.Generic;

namespace BigDataCollections.DistributedArray.Interfaces
{
    /// <summary>
    /// IArrayList is a IList collection with additional methods for more fast work
    /// as internal block collection of DistributedArray(T).
    /// </summary>
    public interface IArrayList<T>: IList<T>
    {
        /// <summary>
        /// Adds the elements of the specified collection to the end of IArrayList(T).
        /// </summary>
        /// <param name="collection">The collection whose elements should be added to the end of the IArrayList(T).
        ///  The collection it self cannot benull, but it can contain elements that are null, if type T is a reference type.</param>
        void AddRange(ICollection<T> collection);
        /// <summary>
        /// Inserts the elements of a collection into the IArrayList(T) at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the new elements should be inserted.</param>
        /// <param name="collection">The collection whose elements should be inserted into the DistributedArray(T).
        ///  The collection it self cannot be null, but it can contain elements that are null, if type T is a reference type.</param>
        void InsertRange(int index, ICollection<T> collection);
        /// <summary>
        /// Reverses the order of the elements in the entire IArrayList(T).
        /// </summary>
        void Reverse();
    }
}
