using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace BigDataCollections
{
    class DistributedStack<T> : IEnumerable<T>
    {
        //API
        public DistributedStack() : this(new Collection<T>())
        {
            
        }
        public DistributedStack(ICollection<T> collection)
        {
            _array = new DistributedArray<T>(collection);
        }
        public void Clear()
        {
            _array.Clear();
        }
        public bool Contains(T item)
        {
            return _array.Contains(item);
        }
        public void CopyTo(T[] array, int arrayIndex)
        {
            _array.CopyTo(array, arrayIndex);
        }
        public IEnumerator<T> GetEnumerator()
        {
            return _array.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public T Peek()
        {
            T item = _array[_array.Count - 1];

            return item;
        }
        public T Pop()
        {
            T item = _array[_array.Count - 1];
            _array.RemoveAt(_array.Count - 1);

            return item;
        }
        public void Push(T item)
        {
            _array.Add(item);
        }
        public T[] ToArray()
        {
            return _array.ToArray();
        }
        public void TrimExcess()
        {
            _array.TrimExcess();
        }

        //Data
        public int Count
        {
            get
            {
                return _array.Count;
            }
        }
        private readonly DistributedArray<T> _array;
    }
}
