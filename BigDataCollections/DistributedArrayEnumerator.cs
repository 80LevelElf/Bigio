using System.Collections;
using System.Collections.Generic;

namespace BigDataCollections
{
    public partial class DistributedArray<T>
    {
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
                    if (_indexOfCurrentBlock < Array._blocks.Count - 1)
                    {
                        _indexOfCurrentBlock++;
                        _subenumerator = Array._blocks[_indexOfCurrentBlock].GetEnumerator();
                        return MoveNext();
                    }
                    //There is no block to move
                    return false;
                }
                //If everithing is ok
                return true;
            }
            public void Reset()
            {
                if (Array.Count != 0)
                {
                    _subenumerator = Array._blocks[0].GetEnumerator();
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
            /// <summary>
            /// Move enumerator to the specified index of the DistributedArray(T).
            /// </summary>
            /// <param name="index">he zero-based index of the element to point to.</param>
            public void MoveToIndex(int index)
            {
                int indexOfBlock, blockStartIndex;
                Array.IndexOfBlockAndBlockStartIndex(index, out indexOfBlock, out blockStartIndex);

                _subenumerator = Array._blocks[indexOfBlock].GetEnumerator();
                for (int i = blockStartIndex; i <= index; i++)
                {
                    _subenumerator.MoveNext();
                }
            }

            //Data
            /// <summary>
            /// Parent DistributedArray(T) of enumerator.
            /// </summary>
            public DistributedArray<T> Array;
            private IEnumerator<T> _subenumerator;
            private int _indexOfCurrentBlock;
        }
    }
}