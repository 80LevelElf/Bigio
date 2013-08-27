using System.Collections;
using System.Collections.Generic;

namespace BigDataCollections
{
    public partial class DistributedArray<T>
    {
        public class DistributedArrayEnumerator : IEnumerator<T>
        {
            /// <summary>
            /// Create new iterator for DistributedArray(T) object.
            /// </summary>
            /// <param name="parentArray">Iterated array.</param>
            public DistributedArrayEnumerator(DistributedArray<T> parentArray)
            {
                ParentArray = parentArray;
                _blockCount = parentArray._blocks.Count;
                Reset();
            }
            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns>True if the enumerator was successfully advanced to the next element;
            ///  false if the enumerator has passed the end of the collection. </returns>
            public bool MoveNext()
            {
                _currentIndexInBlock++;
                //If there is end of current block go to next block
                if (_currentIndexInBlock == _currentBlock.Count)
                {
                    _currentIndexOfBlock++;
                    //If there is last block stay at last element
                    if (_currentIndexOfBlock == _blockCount)
                    {
                        _currentIndexOfBlock--;
                        _currentIndexInBlock--;
                        return false;
                    }
                    //Go to next block
                    _currentBlock = ParentArray._blocks[_currentIndexOfBlock];
                    _currentIndexInBlock = 0;
                }
                return true;
            }
            public bool MovePrev()
            {
                _currentIndexInBlock--;
                //If _currentIndexInBlock is out of current block - move to 
                if (_currentIndexInBlock == -1)
                {
                    _currentIndexOfBlock--;
                    //If _currentIndexOfBlock is out of blocks - stay at first element
                    if (_currentIndexOfBlock == -1)
                    {
                        _currentIndexOfBlock++;
                        _currentIndexInBlock++;
                        return false;
                    }
                    //Go to prev block
                    _currentBlock = ParentArray._blocks[_currentIndexOfBlock];
                    _currentIndexInBlock = _currentBlock.Count - 1;
                }
                return true;
            }
            /// <summary>
            /// Move enumerator to needed position in the DistributedArray(T)
            /// </summary>
            /// <param name="index">The zero-based index of the needed element.</param>
            public void MoveToIndexBefore(int index)
            {
                //This need beacuse IndexOfBlockAndBlockStartCommonIndex cant find result if index == -1 because there is no such index.
                if (index == 0)
                {
                    _currentIndexInBlock = -1; // Move at element before first
                    _currentIndexOfBlock = 0;
                    _currentBlock = ParentArray._blocks[0];
                }
                else
                {
                    //Move to element before specified
                    index -= 1;

                    int startCommonIndex;
                    ParentArray.IndexOfBlockAndBlockStartCommonIndex(index, out _currentIndexOfBlock, out startCommonIndex);
                    _currentIndexInBlock = index - startCommonIndex;
                    _currentBlock = ParentArray._blocks[_currentIndexOfBlock];   
                }
            }
            public void MoveToIndexAfter(int index)
            {
                //This need beacuse IndexOfBlockAndBlockStartCommonIndex cant find result
                //if index is index of last element because there is no such index.
                if (index == ParentArray.Count - 1)
                {
                    _currentIndexOfBlock = ParentArray._blocks.Count - 1;
                    _currentBlock = ParentArray._blocks[_currentIndexOfBlock];
                    _currentIndexInBlock = _currentBlock.Count; //Move at element after last
                }
                else
                {
                    //Move to element after specified
                    index += 1;

                    int startCommonIndex;
                    ParentArray.IndexOfBlockAndBlockStartCommonIndex(index, out _currentIndexOfBlock, out startCommonIndex);
                    _currentIndexInBlock = index - startCommonIndex;
                    _currentBlock = ParentArray._blocks[_currentIndexOfBlock];
                }
            }
            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            public void Reset()
            {
                _currentBlock = ParentArray._blocks[0];
                _currentIndexInBlock = -1;
                _currentIndexOfBlock = 0;
            }
            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                
            }

            //Data
            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            public T Current
            {
                get { return ParentArray._blocks[_currentIndexOfBlock][_currentIndexInBlock]; }
            }
            /// <summary>
            /// Gets the current element in the collection.
            /// </summary>
            object IEnumerator.Current
            {
                get { return Current; }
            }
            /// <summary>
            /// Iterated array.
            /// </summary>
            public DistributedArray<T> ParentArray { get; private set; }

            private List<T> _currentBlock;
            private int _currentIndexOfBlock;
            private int _currentIndexInBlock;
            //Cache
            private readonly int _blockCount;
        }
    }
}