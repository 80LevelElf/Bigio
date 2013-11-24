using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BigDataCollections.DistributedArray.SupportClasses.BlockCollection
{
    partial class BlockCollection<T>
    {
        class BlockCollectionEnumerator : IEnumerator<List<T>>
        {
            public BlockCollectionEnumerator(BlockCollection<T> parent)
            {
                Parent = parent;
                Reset();
            }

            public void Dispose()
            {

            }

            public bool MoveNext()
            {
                if (_currentIndex + 1 < Parent.Count)
                {
                    Current = Parent[++_currentIndex];
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                _currentIndex = -1;
                Current = null;
            }

            public List<T> Current { get; private set; }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            //Data
            private BlockCollection<T> Parent { get; set; }
            private int _currentIndex;
        }
    }
}
