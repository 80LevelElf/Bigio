using System.Collections;
using System.Collections.Generic;

namespace Bigio.BigArray.Support_Classes.BlockCollection
{
    public partial class BlockCollection<T>
    {
        /// <summary>
        /// Enumerates the elements of a <see cref="BlockCollection{T}"/>.
        /// </summary>
        class BlockCollectionEnumerator : IEnumerator<Block<T>>
        {
            //Data

            /// <summary>
            /// Parent <see cref="BlockCollection{T}"/> to enumerate.
            /// </summary>
            private readonly BlockCollection<T> _parent;

            /// <summary>
            /// Index of current block of parent collection.
            /// </summary>
            private int _currentIndex;

            //API

            /// <summary>
            /// Supports a iteration over a <see cref="BlockCollection{T}"/>.
            /// </summary>
            /// <param name="parent"></param>
            public BlockCollectionEnumerator(BlockCollection<T> parent)
            {
                _parent = parent;
                Reset();
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {

            }

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns>
            /// True if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
            /// </returns>
            public bool MoveNext()
            {
                if (_currentIndex + 1 < _parent.Count)
                {
                    Current = _parent[++_currentIndex];
                    return true;
                }
                return false;
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            public void Reset()
            {
                _currentIndex = -1;
                Current = null;
            }

            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            /// <returns>
            /// The element in the collection at the current position of the enumerator.
            /// </returns>
            public Block<T> Current { get; private set; }

            /// <summary>
            /// Gets the current element in the collection.
            /// </summary>
            /// <returns>
            /// The current element in the collection.
            /// </returns>
            object IEnumerator.Current
            {
                get { return Current; }
            }
        }
    }
}
