using System;

namespace BigDataCollections.DistributedArray.Managers
{
    /// <summary>
    /// DefaultValuesManager allowes you set and get global value of different values.
    /// </summary>
    public static class DefaultValuesManager
    {
        //API
        /// <summary>
        /// Set initial data for global values.
        /// </summary>
        static DefaultValuesManager()
        {
            DefaultBlockSize = 1024;
            MaxBlockSize = 4*DefaultBlockSize;
        }
        /// <summary>
        /// Through this field you can set or get global value of DefaultBlockSize.
        /// All new objects using it will have this value of DefaultBlockSize field.
        /// </summary>
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
        /// <summary>
        /// Through this field you can set or get global value of MaxBlockSize.
        /// All new objects using it will have this value of MaxBlockSize field.
        /// </summary>
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

        //Data
        /// <summary>
        /// Internal value of DefaultBlockSize. Dont use it out of DefaultBlockSize field.
        /// </summary>
        private static int _defaultBlockSize;
        /// <summary>
        /// Internal value of MaxBlockSize. Dont use it out of DefaultBlockSize field.
        /// </summary>
        private static int _maxBlockSize;
    }
}
