using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Bigio
{
    /// <summary>
    /// Represents a first-in, first-out collection of objects adapted for big amount of collections.
    /// </summary>
    /// <typeparam name="T">Specifies the type of elements in the queue.</typeparam>
    class BigQueue<T> : IEnumerable<T>
    {
        //Data

        /// <summary>
        /// Internal array to stote data.
        /// </summary>
        private readonly BigArray<T> _array;

        /// <summary>
        /// Copies the elements of the <see cref="BigQueue{T}"/> to a new array.
        /// </summary>
        /// <returns>An array containing copies of the elements of the <see cref="BigQueue{T}"/>.</returns>
        public T[] ToArray()
        {
            return _array.ToArray();
        }

        /// <summary>
        /// Get the number of elements actually contained in the <see cref="BigQueue{T}"/>.
        /// </summary>
        public int Count
        {
            get
            {
                return _array.Count;
            }
        }

        /// <summary>
        /// Default size of one <see cref="BigQueue{T}"/> block. 
        /// Because of the way memory allocation is most effective that it is a power of 2.
        /// </summary>
        public int DefaultBlockSize
        {
            get
            {
                return _array.DefaultBlockSize;
            }
            set
            {
                _array.DefaultBlockSize = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="BigQueue{T}"/> is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return _array.IsReadOnly;
            }
        }

        /// <summary>
        /// The size of any block never will be more than this number.
        /// Because of the way memory allocation is most effective that it is a power of 2.
        /// </summary>
        public int MaxBlockSize
        {
            get
            {
                return _array.MaxBlockSize;
            }
            set
            {
                _array.MaxBlockSize = value;
            }
        }

        //API

        /// <summary>
        /// Crerate new empty instance of <see cref="BigQueue{T}"/>.
        /// </summary>
        public BigQueue() : this(new Collection<T>())
        {
            
        }

        /// <summary>
        /// Crerate new instance of <see cref="BigQueue{T}"/> using specified collection.
        /// </summary>
        /// <param name="collection">Collection whitch use as base for the new <see cref="BigQueue{T}"/>.
        /// The collection it self can't be null, but it can contains elements that are null, if type T is a reference type.</param>
        public BigQueue(ICollection<T> collection)
        {
            _array = new BigArray<T>(collection);
        }

        /// <summary>
        /// Removes all elements from the <see cref="BigQueue{T}"/>. 
        /// </summary>
        public void Clear()
        {
            _array.Clear();
        }

        /// <summary>
        /// Remove true if <see cref="BigQueue{T}"/> contains value, otherwise return false.
        /// </summary>
        /// <param name="item">Data to be checked.</param>
        public bool Contains(T item)
        {
            return _array.Contains(item);
        }

        /// <summary>
        /// Copies the entire <see cref="BigQueue{T}"/> to a compatible one-dimensional array, starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional Array that is the destination of the elements copied from <see cref="BigQueue{T}"/>.
        ///  The Array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            _array.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes and returns the object at the beginning of the <see cref="BigQueue{T}"/>.
        /// </summary>
        /// <returns>The object that is removed from the beginning of the <see cref="BigQueue{T}"/>.</returns>
        public T Dequeue()
        {
            if (_array.Count == 0)
            {
                throw new InvalidOperationException("There queue is empty!");
            }

            T item = _array[_array.Count - 1];
            _array.RemoveLast();

            return item;
        }

        /// <summary>
        /// Adds an object to the end of the <see cref="BigQueue{T}"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="BigQueue{T}"/>. The value can be null for reference types.</param>
        public void Enqueue(T item)
        {
            _array.Add(item);
        }

        /// <summary>
        /// Returns an enumerator that iterates through <see cref="BigQueue{T}"/>.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            return _array.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns the object at the beginning of the <see cref="BigQueue{T}"/> without removing it.
        /// </summary>
        /// <returns>The object at the beginning of the <see cref="BigQueue{T}"/>.</returns>
        public T Peek()
        {
            if (_array.Count == 0)
            {
                throw new InvalidOperationException("There queue is empty!");
            }

            return _array[0];
        }

        /// <summary>
        /// Rebalance internal data strucuture to make data parts less fragmented.
        /// </summary>
        public void Rebalance()
        {
            _array.Rebalance();
        }
    }
}
