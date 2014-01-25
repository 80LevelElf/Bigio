using System;

namespace BigDataCollections.LevelSparseArray.SupportClasses.Level
{
    class DataLevel<T> : Level
    {
        //API
        public DataLevel() : base(0)
        {
            
        }
        public DataLevel(T data):base(0)
        {
            Data = data;
        }

        public T Data { get; set; }
    }
}
