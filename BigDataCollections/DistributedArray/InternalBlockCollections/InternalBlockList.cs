using System.Collections.Generic;
using BigDataCollections.DistributedArray.Interfaces;

namespace BigDataCollections.DistributedArray.InternalBlockCollections
{
    /// <summary>
    /// InternalBlockList is interlayer of List(T) for List(T) can be used
    /// as internal block collection of DistributedArray(T)
    /// </summary>
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
