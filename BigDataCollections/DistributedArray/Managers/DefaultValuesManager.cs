using System;

namespace BigDataCollections.DistributedArray.Managers
{
    public static class DefaultValuesManager
    {
        //API
        static DefaultValuesManager()
        {
            DefaultBlockSize = 1024;
            MaxBlockSize = 4*DefaultBlockSize;
            MaxCountOfRemovedElementsToCallGC = 16*MaxBlockSize;
        }
        public static int DefaultBlockSize
        {
            get
            {
                return _defaultBlockSize;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", "DefaultBlockSize cant be less than 0.");
                }
                _defaultBlockSize = value;
            }
        }
        public static int MaxBlockSize
        {
            get
            {
                return _maxBlockSize;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", "MaxBlockSize cant be less than 0.");
                }
                if (value < DefaultBlockSize)
                {
                    throw new ArgumentOutOfRangeException("value", "MaxBlockSize cant be less than DefaultBlockSize.");
                }
                _maxBlockSize = value;
            }
        }
        public static int MaxCountOfRemovedElementsToCallGC
        {
            get
            {
                return _maxCountOfRemovedElementsToCallGC;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", "MaxCountOfRemovedElementsToCallGC cant be less than 0.");
                }
                _maxCountOfRemovedElementsToCallGC = value;
            }
        }
        public static GarabeCollectorManager GarabeCollectorManager
        {
            get
            {
                return new GarabeCollectorManager(MaxCountOfRemovedElementsToCallGC);
            }
        }

        //Data
        private static int _defaultBlockSize;
        private static int _maxBlockSize;
        private static int _maxCountOfRemovedElementsToCallGC;
    }
}
