using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Bigio
{
    /// <summary>
    /// Represents a variable size last-in-first-out (LIFO) collection adapted for big amount of collections.
    /// </summary>
    /// <typeparam name="T">Specifies the type of elements in the stack.</typeparam>
    class BigStack<T> : IEnumerable<T>
    {
        //Data

        /// <summary>
        /// Internal array to stote data.
        /// </summary>
        private readonly BigArray<T> _array;

        /// <summary>
        /// Gets the number of elements contained in the <see cref="BigStack{T}"/>.
        /// </summary>
        public int Count
        {
            get
            {
                return _array.Count;
            }
        }

        //API
        /// <summary>
        /// Initializes a new instance of the <see cref="BigStack{T}"/> class that is empty.
        /// </summary>
        public BigStack() : this(new Collection<T>())
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BigStack{T}"/> class that contains elements copied from the specified collection.
        /// </summary>
        /// <param name="collection">The collection to copy elements from.</param>
        public BigStack(ICollection<T> collection)
        {
            _array = new BigArray<T>(collection);
        }

        /// <summary>
        /// Removes all objects from the <see cref="BigStack{T}"/>.
        /// </summary>
        public void Clear()
        {
            _array.Clear();
        }

        /// <summary>
        /// Determines whether an element is in the <see cref="BigStack{T}"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="BigStack{T}"/>.
        /// The value can be null for reference types.</param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            return _array.Contains(item);
        }

        /// <summary>
        /// Copies the <see cref="BigStack{T}"/> to an existing one-dimensional Array, starting at the specified array index.
        /// </summary>
        /// <param name="array">The one-dimensional array that is the destination of the elements copied from <see cref="BigStack{T}"/>.
        ///  The Array must have zero-based indexing. </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            _array.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns an enumerator for the <see cref="BigStack{T}"/>.
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
        /// Returns the object at the top of the <see cref="BigStack{T}"/> without removing it.
        /// </summary>
        /// <returns>The object at the top of the <see cref="BigStack{T}"/>.</returns>
        public T Peek()
        {
            T item = _array[Count - 1];

            return item;
        }

        /// <summary>
        /// Removes and returns the object at the top of the <see cref="BigStack{T}"/>.
        /// </summary>
        /// <returns>The object removed from the top of the <see cref="BigStack{T}"/>.</returns>
        public T Pop()
        {
            T item = _array[Count - 1];
            _array.RemoveLast();

            return item;
        }

        /// <summary>
        /// Inserts an object at the top of the <see cref="BigStack{T}"/>.
        /// </summary>
        /// <param name="item">The object to push onto the <see cref="BigStack{T}"/>.
        ///  The value can be null for reference types.</param>
        public void Push(T item)
        {
            _array.Add(item);
        }

        /// <summary>
        /// Rebalance internal data strucuture to make data parts less fragmented.
        /// </summary>
        public void Rebalance()
        {
            _array.Rebalance();
        }

        /// <summary>
        /// Copies the <see cref="BigStack{T}"/> to a new array.
        /// </summary>
        /// <returns>A new array containing copies of the elements of the <see cref="BigStack{T}"/>.</returns>
        public T[] ToArray()
        {
            return _array.ToArray();
        }
    }
}
