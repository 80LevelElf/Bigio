using System.Collections.Generic;
using Bigio.BigArray.Interfaces;

namespace Bigio.BigArray.InternalBlockCollections
{
    /// <summary>
    /// InternalBlockList is interlayer of List(T) for List(T) can be used
    /// as internal block collection of BigArray(T)
    /// </summary>
    public class InternalBlockList<T> : List<T>, IBigList<T>
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
