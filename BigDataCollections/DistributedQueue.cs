using System;
using System.Collections;
using System.Collections.Generic;

namespace BigDataCollections
{
    class DistributedQueue<T> : IEnumerable<T>
    {
        /// <summary>
        /// Crerate new empty instance of DistributedQueue(T) based on DistributedArray(T).
        /// </summary>
        public DistributedQueue()
        {
            _data = new DistributedArray<T>();
        }
        /// <summary>
        /// Crerate new instance of DistributedQueue(T) based on DistributedArray(T) using specified collection.
        /// </summary>
        /// <param name="collection">Collection whitch use as base for new DistributedArray(T).
        /// The collection it self cannot be null, but it can contain elements that are null, if type T is a reference type.</param>
        public DistributedQueue(ICollection<T> collection)
        {
            _data = new DistributedArray<T>(collection);
        }
        /// <summary>
        /// Create new epmty instance of DistributedQueue(T) based on DistributedArray(T) with specified capacity. 
        /// </summary>
        /// <param name="capacity">The number of elements that the new queue can initially store.</param>
        public DistributedQueue(int capacity)
        {
            _data = new DistributedArray<T>(capacity);
        }
        /// <summary>
        /// Removes all elements from the DistributedQueue(T). If there is too many elements
        /// (at this moment if count of elements is more or equal than 64*MaxBlockSize) -
        /// force call of garbadge collector to delete all generations of garbage.
        /// </summary>
        public void Clear()
        {
            _data.Clear();
        }
        /// <summary>
        /// Removes all elements from the DistributedQueue(T).
        /// </summary>
        /// <param name="isImmediately">If true - force call of garbadge collector
        /// to delete all generations of garbage, otherwise - just remove links to the data
        /// and wait whan garbadge collector remove it independently.</param>
        public void Clear(bool isImmediately)
        {
            _data.Clear(isImmediately);
        }
        /// <summary>
        /// Remove true if DistributedQueue(T) contains value, otherwise return false.
        /// </summary>
        /// <param name="item">Data to be checked.</param>
        public bool Contains(T item)
        {
            return _data.Contains(item);
        }
        /// <summary>
        /// Copies the entire DistributedQueue(T) to a compatible one-dimensional array, starting at the beginning of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional Array that is the destination of the elements copied from DistributedQueue(T).
        ///  The Array must have zero-based indexing.</param>
        public void CopyTo(T[] array)
        {
            _data.CopyTo(array);
        }
        /// <summary>
        /// Copies the entire DistributedQueue(T) to a compatible one-dimensional array, starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional Array that is the destination of the elements copied from DistributedQueue(T).
        ///  The Array must have zero-based indexing. </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins. </param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            _data.CopyTo(array, arrayIndex);
        }
        /// <summary>
        /// Removes and returns the object at the beginning of the DistributedQueue(T).
        /// </summary>
        /// <returns>The object that is removed from the beginning of the DistributedQueue(T).</returns>
        public T Dequeue()
        {
            if (_data.Count == 0)
            {
                throw new InvalidOperationException("There queue is empty!");
            }

            int lastIndex = _data.Count - 1;
            T item = _data[lastIndex];
            _data.RemoveAt(lastIndex);

            return item;
        }
        /// <summary>
        /// Adds an object to the end of the DistributedQueue(T).
        /// </summary>
        /// <param name="item">The object to add to the DistributedQueue(T). The value can benull for reference types.</param>
        public void Enqueue(T item)
        {
            _data.Add(item);
        }
        /// <summary>
        /// Returns an enumerator that iterates through the DistributedArray(T).
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            return _data.GetEnumerator();
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
            if (_data.Count == 0)
            {
                throw new InvalidOperationException("There queue is empty!");
            }

            return _data[0];
        }
        /// <summary>
        /// Copies the elements of the DistributedQueue(T) to a new array.
        /// </summary>
        /// <returns>An array containing copies of the elements of the DistributedQueue(T).</returns>
        public T[] ToArray()
        {
            return _data.ToArray();
        }
        /// <summary>
        /// Sets the capacity of every block of based DistributedArray(T) to the actual number of elements in it.
        /// </summary>
        public void TrimExcess()
        {
            _data.TrimExcess();
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
                return _data.DefaultBlockSize;
            }
            set
            {
                _data.DefaultBlockSize = value;
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
                return _data.MaxBlockSize;
            }
            set
            {
                _data.MaxBlockSize = value;
            }
        }
        /// <summary>
        /// Get the number of elements actually contained in the DistributedQueue(T).
        /// </summary>
        public int Count
        {
            get
            {
                return _data.Count;
            }
        }
        /// <summary>
        /// Gets a value indicating whether the DistributedQueue(T) is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return _data.IsReadOnly;
            }
        }

        private readonly DistributedArray<T> _data;
    }
}
