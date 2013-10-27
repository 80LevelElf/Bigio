using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BigDataCollections
{
    /// <summary>
    /// Represents a first-in, first-out collection of objects based on DistributedArray(T).
    /// </summary>
    /// <typeparam name="T">Specifies the type of elements in the queue.</typeparam>
    class DistributedQueue<T> : IEnumerable<T>
    {
        //API
        /// <summary>
        /// Crerate new empty instance of DistributedQueue(T) based on DistributedArray(T).
        /// </summary>
        public DistributedQueue() : this(new Collection<T>())
        {
            
        }
        /// <summary>
        /// Crerate new instance of DistributedQueue(T) based on DistributedArray(T) using specified collection.
        /// </summary>
        /// <param name="collection">Collection whitch use as base for new DistributedArray(T).
        /// The collection it self cannot be null, but it can contain elements that are null, if type T is a reference type.</param>
        public DistributedQueue(ICollection<T> collection)
        {
            _array = new DistributedArray<T>(collection);
        }
        /// <summary>
        /// Removes all elements from the DistributedQueue(T). 
        /// </summary>
        public void Clear()
        {
            _array.Clear();
        }
        /// <summary>
        /// Remove true if DistributedQueue(T) contains value, otherwise return false.
        /// </summary>
        /// <param name="item">Data to be checked.</param>
        public bool Contains(T item)
        {
            return _array.Contains(item);
        }
        /// <summary>
        /// Copies the entire DistributedQueue(T) to a compatible one-dimensional array, starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional Array that is the destination of the elements copied from DistributedQueue(T).
        ///  The Array must have zero-based indexing. </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins. </param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            _array.CopyTo(array, arrayIndex);
        }
        /// <summary>
        /// Removes and returns the object at the beginning of the DistributedQueue(T).
        /// </summary>
        /// <returns>The object that is removed from the beginning of the DistributedQueue(T).</returns>
        public T Dequeue()
        {
            if (_array.Count == 0)
            {
                throw new InvalidOperationException("There queue is empty!");
            }

            int lastIndex = _array.Count - 1;
            T item = _array[lastIndex];
            _array.RemoveAt(lastIndex);

            return item;
        }
        /// <summary>
        /// Adds an object to the end of the DistributedQueue(T).
        /// </summary>
        /// <param name="item">The object to add to the DistributedQueue(T). The value can benull for reference types.</param>
        public void Enqueue(T item)
        {
            _array.Add(item);
        }
        /// <summary>
        /// Returns an enumerator that iterates through the DistributedArray(T).
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
        /// Returns the object at the beginning of the DistributedArray(T) without removing it.
        /// </summary>
        /// <returns>The object at the beginning of the DistributedArray(T).</returns>
        public T Peek()
        {
            if (_array.Count == 0)
            {
                throw new InvalidOperationException("There queue is empty!");
            }

            return _array[0];
        }
        /// <summary>
        /// Copies the elements of the DistributedQueue(T) to a new array.
        /// </summary>
        /// <returns>An array containing copies of the elements of the DistributedQueue(T).</returns>
        public T[] ToArray()
        {
            return _array.ToArray();
        }
        /// <summary>
        /// Rebalance DistributedQueue(T) to every block have DefaultBlockSize elements.
        /// </summary>
        public void Rebalance()
        {
            _array.Rebalance();
        }

        //Data
        /// <summary>
        /// Default size of one DistributedQueue(T) block. 
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
        /// <summary>
        /// Get the number of elements actually contained in the DistributedQueue(T).
        /// </summary>
        public int Count
        {
            get
            {
                return _array.Count;
            }
        }
        /// <summary>
        /// Gets a value indicating whether the DistributedQueue(T) is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return _array.IsReadOnly;
            }
        }

        private readonly DistributedArray<T> _array;
    }
}
