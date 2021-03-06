﻿using System.Collections.Generic;

namespace Bigio.BigArray.Interfaces
{
    /// <summary>
    /// IBigList is a <see cref="IList{T}"/> collection with additional methods for more fast work
    /// as internal block collection of <see cref="BigArray{T}"/>.
    /// </summary>
    public interface IBigList<T>: IList<T>
    {
        /// <summary>
        /// Adds the elements of the specified collection to the end of <see cref="IBigList{T}"/>.
        /// </summary>
        /// <param name="collection">The collection whose elements should be added to the end of the <see cref="IBigList{T}"/>.
        ///  The collection it self cannot benull, but it can contain elements that are null, if type T is a reference type.</param>
        void AddRange(ICollection<T> collection);

        /// <summary>
        /// Inserts the elements of a collection into the <see cref="IBigList{T}"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the new elements should be inserted.</param>
        /// <param name="collection">The collection whose elements should be inserted into the <see cref="BigArray{T}"/>.
        ///  The collection it self cannot be null, but it can contain elements that are null, if type T is a reference type.</param>
        void InsertRange(int index, ICollection<T> collection);

        /// <summary>
        /// Reverses the order of the elements in the entire <see cref="IBigList{T}"/>.
        /// </summary>
        void Reverse();
    }
}
