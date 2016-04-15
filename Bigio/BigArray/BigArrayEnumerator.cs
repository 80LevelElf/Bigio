using System.Collections;
using System.Collections.Generic;
using Bigio.BigArray.Support_Classes.BlockCollection;

namespace Bigio
{
    public partial class BigArray<T>
    {
        /// <summary>
        /// Enumerates the elements of a BigArray(T).
        /// </summary>
        public class BigArrayEnumerator : IEnumerator<T>
        {
            //Data

            /// <summary>
            /// Index of parent block of current _subenumerator.
            /// </summary>
            private int _indexOfCurrentBlock;

            private readonly int _blockCount;

            private BlockCollection<T> _blockCollection; 

            /// <summary>
            /// Enumerator of current block. When we cross current block
            /// _subenumerator will be enumerator of block after current,
            /// if there is next block.
            /// </summary>
            private IEnumerator<T> _subenumerator;

            //API

            /// <summary>
            /// Supports a iteration over a BigArray(T) collection.
            /// </summary>
            /// <param name="array">BigArray(T) object using for enumerate it.</param>
            public BigArrayEnumerator(BigArray<T> array)
            {
                Array = array;
                _blockCollection = Array._blockCollection;
                _blockCount = _blockCollection.Count;

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
                    _indexOfCurrentBlock++;

                    if (_indexOfCurrentBlock < _blockCount)
                    {
                        _subenumerator = _blockCollection[_indexOfCurrentBlock].GetEnumerator();

                        return MoveNext();
                    }

                    //There is no block to move
                    return false;
                }

                //If everithing is ok
                return true;
            }
            /// <summary>
            /// Move enumerator to the specified index of the BigArray(T).
            /// </summary>
            /// <param name="index">he zero-based index of the element to point to.</param>
            public void MoveToIndex(int index)
            {
                var blockInfo = Array._blockStructure.BlockInfo(index);

                _subenumerator = Array._blockCollection[blockInfo.IndexOfBlock].GetEnumerator();

                //It is always executed at least once
                for (int i = blockInfo.StartIndex; i <= index; i++)
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

            /// <summary>
            /// Parent BigArray(T) of enumerator.
            /// </summary>
            public BigArray<T> Array { get; set; }
        }
    }
}