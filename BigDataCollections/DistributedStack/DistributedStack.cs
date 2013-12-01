using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BigDataCollections
{
    /// <summary>
    /// Represents a variable size last-in-first-out (LIFO) collection
    ///  of instances of the same arbitrary type based on DistributedArray(T). 
    /// </summary>
    /// <typeparam name="T">Specifies the type of elements in the stack.</typeparam>
    class DistributedStack<T> : IEnumerable<T>
    {
        //API
        /// <summary>
        /// Initializes a new instance of the DistributedStack(T)
        ///  class that is empty.
        /// </summary>
        public DistributedStack() : this(new Collection<T>())
        {
            
        }
        /// <summary>
        /// Initializes a new instance of the DistributedStack(T) class that contains
        ///  elements copied from the specified collection.
        /// </summary>
        /// <param name="collection">The collection to copy elements from.</param>
        public DistributedStack(ICollection<T> collection)
        {
            _array = new DistributedArray<T>(collection);
        }
        /// <summary>
        /// Removes all objects from the DistributedStack(T).
        /// </summary>
        public void Clear()
        {
            _array.Clear();
        }
        /// <summary>
        /// Determines whether an element is in the DistributedStack(T).
        /// </summary>
        /// <param name="item">The object to locate in the DistributedStack(T).
        ///  The value can be null for reference types.</param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            return _array.Contains(item);
        }
        /// <summary>
        /// Copies the DistributedStack(T) to an existing one-dimensional Array,
        ///  starting at the specified array index.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            _array.CopyTo(array, arrayIndex);
        }
        /// <summary>
        /// Returns an enumerator for the DistributedStack(T).
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
        /// Returns the object at the top of the DistributedStack(T) without removing it.
        /// </summary>
        /// <returns>The object at the top of the DistributedStack(T).</returns>
        public T Peek()
        {
            T item = _array[_array.Count - 1];

            return item;
        }
        /// <summary>
        /// Removes and returns the object at the top of the DistributedStack(T).
        /// </summary>
        /// <returns>The object removed from the top of the DistributedStack(T).</returns>
        public T Pop()
        {
            T item = _array[_array.Count - 1];
            _array.RemoveAt(_array.Count - 1);

            return item;
        }
        /// <summary>
        /// Inserts an object at the top of the DistributedStack(T).
        /// </summary>
        /// <param name="item">The object to push onto the DistributedStack(T).
        ///  The value can be null for reference types.</param>
        public void Push(T item)
        {
            _array.Add(item);
        }
        /// <summary>
        /// Rebalance DistributedStack(T) to every block have DefaultBlockSize elements.
        /// </summary>
        public void Rebalance()
        {
            _array.Rebalance();
        }
        /// <summary>
        /// Copies the DistributedStack(T) to a new array.
        /// </summary>
        /// <returns>A new array containing copies of the elements of the DistributedStack(T).</returns>
        public T[] ToArray()
        {
            return _array.ToArray();
        }

        //Data
        /// <summary>
        /// Gets the number of elements contained in the DistributedStack(T).
        /// </summary>
        public int Count
        {
            get
            {
                return _array.Count;
            }
        }
        private readonly DistributedArray<T> _array;
    }
}
