using System.Collections;
using System.Collections.Generic;
using System.Web;
using Bigio.BigArray.Support_Classes.BlockCollection;
using Bigio.BigArray.Support_Classes.BlockStructure;

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

            private readonly int _blockCount;

            private readonly BlockCollection<T> _blockCollection;

            private Block<T> _currentBlock;

            private int _indexInCurrentBlock;

            private int _currentBlockIndex;

            private int _currentBlockCount;

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
                if (_indexInCurrentBlock >= _currentBlockCount - 1)
                {
                    //Try to move next block
                    _currentBlockIndex++;

                    if (_currentBlockIndex < _blockCount)
                    {
                        _currentBlock = _blockCollection[_currentBlockIndex];
                        _indexInCurrentBlock = 0;
                        _currentBlockCount = _currentBlock.Count;

                        return true;
                    }

                    //There is no block to move
                    return false;
                }

                _indexInCurrentBlock++;
                return true;
            }

            /// <summary>
            /// Move enumerator to the specified index of the BigArray(T).
            /// </summary>
            /// <param name="index">The zero-based index of the element to point to.</param>
            public void MoveToIndex(int index)
            {
                var blockInfo = Array._blockStructure.BlockInfo(index, SearchMod.LinearSearch);

                _indexInCurrentBlock = index - blockInfo.StartIndexOfBlock;
                _currentBlockIndex = blockInfo.IndexOfBlock;
                _currentBlock = Array._blockCollection[_currentBlockIndex];
                _currentBlockCount = blockInfo.Count;
            }

            public void Reset()
            {
                if (Array.Count != 0)
                {
                    _indexInCurrentBlock = -1;
                    _currentBlockIndex = -1;
                    _currentBlockCount = -1;
                }
            }

            public T Current
            {
                get
                {
                    return _currentBlock[_indexInCurrentBlock];
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