using System.Collections;
using System.Collections.Generic;

namespace BigDataCollections
{
    public partial class DistributedArray<T>
    {
        public class DistributedArrayEnumerator : IEnumerator<T>
        {
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
                if (_subEnumerator == null)
                {
                    return false;
                }

                bool canMove = _subEnumerator.MoveNext();
                if (!canMove)
                {
                    //Try to move next block
                    if (_currentBlockIndex < Array._blocks.Count - 1)
                    {
                        _currentBlockIndex++;
                        _subEnumerator = Array._blocks[_currentBlockIndex].GetEnumerator();
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
                    _subEnumerator = Array._blocks[0].GetEnumerator();
                    _currentBlockIndex = 0;
                }
            }

            public T Current
            {
                get
                {
                    return _subEnumerator.Current;
                }
            }

            public void MoveToIndex(int index)
            {
                int indexOfBlock, blockStartCommonIndex;
                Array.IndexOfBlockAndBlockStartCommonIndex(index, out indexOfBlock, out blockStartCommonIndex);

                _subEnumerator = Array._blocks[indexOfBlock].GetEnumerator();
                for (int i = 0; i <= index; i++)
                {
                    _subEnumerator.MoveNext();
                }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            //Data
            public DistributedArray<T> Array;
            private IEnumerator<T> _subEnumerator;
            private int _currentBlockIndex;
        }
    }
}