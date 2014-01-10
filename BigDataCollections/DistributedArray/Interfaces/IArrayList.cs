using System.Collections.Generic;

namespace BigDataCollections.DistributedArray.Interfaces
{
    public interface IArrayList<T>: IList<T>
    {
        void AddRange(ICollection<T> collection);
        void InsertRange(int index, ICollection<T> collection);
        void Reverse();
    }
}
