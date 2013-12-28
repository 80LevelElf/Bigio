using System.Collections;
using System.Collections.Generic;

namespace BigDataCollections
{
    public partial class DistributedArray<T>
    {
        /// <summary>
        /// Enumerates the elements of a DistributedArray(T).
        /// </summary>
        public class DistributedArrayEnumerator : IEnumerator<T>
        {
            /// <summary>
            /// Supports a iteration over a DistributedArray(T) collection.
            /// </summary>
            /// <param name="array">DistributedArray(T) object using for enumerate it.</param>
            public DistributedArrayEnumerator(DistributedArray<T> array)
            {
                Array = array;
                if (Array.Count != 0)
                {
                    Reset();
                }
            }
            public void Dispose()
            {

            }
            public bool MoveNext()
            {
                if (_subenumerator == null)
                {
                    return false;
                }

                bool canMove = _subenumerator.MoveNext();
                if (!canMove)
                {
                    //Try to move next block
                    if (_indexOfCurrentBlock < Array._blockCollection.Count - 1)
                    {
                        _indexOfCurrentBlock++;
                        _subenumerator = Array._blockCollection[_indexOfCurrentBlock].GetEnumerator();
                        return MoveNext();
                    }
                    //There is no block to move
                    return false;
                }
                //If everithing is ok
                return true;
            }
            /// <summary>
            /// Move enumerator to the specified index of the DistributedArray(T).
            /// </summary>
            /// <param name="index">he zero-based index of the element to point to.</param>
            public void MoveToIndex(int index)
            {
                var blockInfo = Array._structureManager.BlockInformation(index);

                _subenumerator = Array._blockCollection[blockInfo.IndexOfBlock].GetEnumerator();
                for (int i = blockInfo.BlockStartIndex; i <= index; i++)
                {
                    _subenumerator.MoveNext();
                }
            }
            public void Reset()
            {
                if (Array.Count != 0)
                {
                    _subenumerator = Array._blockCollection[0].GetEnumerator();
                    _indexOfCurrentBlock = 0;
                }
            }
            public T Current
            {
                get
                {
                    return _subenumerator.Current;
                }
            }
            object IEnumerator.Current
            {
                get { return Current; }
            }

            //Data
            /// <summary>
            /// Parent DistributedArray(T) of enumerator.
            /// </summary>
            public DistributedArray<T> Array;
            /// <summary>
            /// Index of parent block of current _subenumerator.
            /// </summary>
            private int _indexOfCurrentBlock;
            /// <summary>
            /// Enumerator of current block. When we cross current block
            /// _subenumerator will be enumerator of block after current,
            /// if there is next block.
            /// </summary>
            private IEnumerator<T> _subenumerator;
        }
    }
}