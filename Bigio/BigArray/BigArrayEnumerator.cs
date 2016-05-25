using System.Collections;
using System.Collections.Generic;
using Bigio.BigArray.Support_Classes.BlockCollection;

namespace Bigio
{
    public partial class BigArray<T>
    {
        /// <summary>
        /// Enumerates the elements of a <see cref="BigArray"/>.
        /// </summary>
        public class BigArrayEnumerator : IEnumerator<T>
        {
            //Data

            /// <summary>
            /// Cached block count of <see cref="_blockCollection"/>. We use it to make code faster.
            /// </summary>
            private readonly int _blockCount;

            /// <summary>
            /// Parent <see cref="BlockCollection{T}"/>
            /// </summary>
            private readonly BlockCollection<T> _blockCollection;

            /// <summary>
            /// Cached current <see cref="Block{T}"/>. We use it to make code faster.
            /// </summary>
            private Block<T> _currentBlock;

            /// <summary>
            /// Current common index in <see cref="_currentBlock"/>. If we <see cref="MoveNext"/> we increase this index and maybe 
            /// move to the next block(if we need to).
            /// </summary>
            private int _indexInCurrentBlock;

            /// <summary>
            /// Index of current block we work with. We can get this information throught <see cref="_currentBlock"/>, but it will be faster
            /// if we cache it.
            /// </summary>
            private int _currentBlockIndex;

            /// <summary>
            /// Count of elements in block we work with. We can get this information throught <see cref="_currentBlock"/>, but it will be faster
            /// if we cache it.
            /// </summary>
            private int _currentBlockCount;

            //API

            /// <summary>
            /// Supports a iteration over a <see cref="BigArray"/> collection.
            /// </summary>
            /// <param name="array"><see cref="BigArray"/> object using for enumerate it.</param>
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
            /// true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
            /// </returns>
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
            /// Move enumerator to the specified index of the <see cref="BigArray"/>.
            /// </summary>
            /// <param name="index">The zero-based index of the element to point to.</param>
            public void MoveToIndex(int index)
            {
                var blockInfo = Array._blockStructure.BlockInfo(index);

                _indexInCurrentBlock = index - blockInfo.CommonStartIndex;
                _currentBlockIndex = blockInfo.IndexOfBlock;
                _currentBlock = Array._blockCollection[_currentBlockIndex];
                _currentBlockCount = blockInfo.Count;
            }


            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            public void Reset()
            {
                if (Array.Count != 0)
                {
                    _indexInCurrentBlock = -1;
                    _currentBlockIndex = -1;
                    _currentBlockCount = -1;
                }
            }

            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            /// <returns>
            /// The element in the collection at the current position of the enumerator.
            /// </returns>
            public T Current
            {
                get
                {
                    return _currentBlock[_indexInCurrentBlock];
                }
            }

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

            /// <summary>
            /// Parent <see cref="BigArray"/> of enumerator.
            /// </summary>
            public BigArray<T> Array { get; set; }
        }
    }
}