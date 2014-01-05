using System.Collections;
using System.Collections.Generic;

namespace BigDataCollections.DistributedArray.SupportClasses.BlockCollection
{
    public partial class BlockCollection<T>
    {
        /// <summary>
        /// Enumerates the elements of a BlockCollection(T).
        /// </summary>
        class BlockCollectionEnumerator : IEnumerator<Block<T>>
        {
            /// <summary>
            /// Supports a iteration over a BlockCollection(T).
            /// </summary>
            /// <param name="parent"></param>
            public BlockCollectionEnumerator(BlockCollection<T> parent)
            {
                _parent = parent;
                Reset();
            }
            public void Dispose()
            {

            }
            public bool MoveNext()
            {
                if (_currentIndex + 1 < _parent.Count)
                {
                    Current = _parent[++_currentIndex];
                    return true;
                }
                return false;
            }
            public void Reset()
            {
                _currentIndex = -1;
                Current = null;
            }

            //Data
            public Block<T> Current { get; private set; }
            object IEnumerator.Current
            {
                get { return Current; }
            }
            /// <summary>
            /// Parent BlockCollection to enumerate.
            /// </summary>
            private readonly BlockCollection<T> _parent;
            /// <summary>
            /// Index of current block of parent collection.
            /// </summary>
            private int _currentIndex;
        }
    }
}
