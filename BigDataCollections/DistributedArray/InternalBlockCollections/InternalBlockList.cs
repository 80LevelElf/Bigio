using System.Collections.Generic;
using BigDataCollections.DistributedArray.Interfaces;

namespace BigDataCollections.DistributedArray.InternalBlockCollections
{
    public class InternalBlockList<T> : List<T>, IArrayList<T>
    {
        public void AddRange(ICollection<T> collection)
        {
            AddRange((IEnumerable<T>) collection);
        }

        public void InsertRange(int index, ICollection<T> collection)
        {
            InsertRange(index, (IEnumerable<T>)collection);
        }
    }
}
