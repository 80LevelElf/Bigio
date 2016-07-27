using System.Collections.Generic;
using Bigio.BigArray.Interfaces;

namespace Bigio.BigArray.Support_Classes.BlockCollection
{
    /// <summary>
    /// InternalBlockList is interlayer of <see cref="List{T}"/> for <see cref="List{T}"/> can be used
    /// as internal block collection of <see cref="BigArray{T}"/>.
    /// </summary>
    public class InternalBlockList<T> : List<T>, IBigList<T>
    {
        /// <summary>
        /// Adds the elements of the specified collection to the end of <see cref="IBigList{T}"/>.
        /// </summary>
        /// <param name="collection">The collection whose elements should be added to the end of the <see cref="IBigList{T}"/>.
        ///  The collection it self cannot benull, but it can contain elements that are null, if type T is a reference type.</param>
        public void AddRange(ICollection<T> collection)
        {
            AddRange((IEnumerable<T>) collection);
        }

        /// <summary>
        /// Inserts the elements of a collection into the <see cref="IBigList{T}"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the new elements should be inserted.</param>
        /// <param name="collection">The collection whose elements should be inserted into the <see cref="BigArray{T}"/>.
        ///  The collection it self cannot be null, but it can contain elements that are null, if type T is a reference type.</param>
        public void InsertRange(int index, ICollection<T> collection)
        {
            InsertRange(index, (IEnumerable<T>)collection);
        }
    }
}
