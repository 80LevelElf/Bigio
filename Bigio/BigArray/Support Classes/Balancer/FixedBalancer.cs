using System;
using Bigio.BigArray.Interfaces;

namespace Bigio.BigArray.Support_Classes.Balancer
{
    /// <summary>
    /// Balancer with fixed size of <see cref="DefaultBlockSize"/> and <see cref="MaxBlockSize"/>
    /// </summary>
    public class FixedBalancer : IBalancer
    {
        /// <summary>
        /// Internal value of <see cref="DefaultBlockSize"/>.
        /// </summary>
        private int _defaultBlockSize;

        /// <summary>
        /// Internal value of <see cref="MaxBlockSize"/>.
        /// </summary>
        private int _maxBlockSize;

        /// <summary>
        /// Default size of one <see cref="BigArray{T}"/> block. 
        /// Because of the way memory allocation is most effective that it is a power of 2.
        /// </summary>
        public int DefaultBlockSize
        {
            get
            {
                return _defaultBlockSize;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value");

                if (value > MaxBlockSize)
                    throw new ArgumentOutOfRangeException("value", "DefaultBlockSize can't be greater than MaxBlockSize");

                _defaultBlockSize = value;
            }
        }

        /// <summary>
        /// The size of any block never will be more than this number.
        /// Because of the way memory allocation is most effective that it is a power of 2.
        /// </summary>
        public int MaxBlockSize
        {
            get
            {
                return _maxBlockSize;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value");

                _maxBlockSize = value;
            }
        }

        public FixedBalancer(int defaultBlockSize = 1024, int maxBlockSize = 4098)
        {
            MaxBlockSize = maxBlockSize;
            DefaultBlockSize = defaultBlockSize;
        }

        public int GetNewBlockSize(int indexOfBlock)
        {
            return DefaultBlockSize;
        }

        public int GetDefaultBlockSize(int indexOfBlock)
        {
            return DefaultBlockSize;
        }

        public int GetMaxBlockSize(int indexOfBlock)
        {
            return MaxBlockSize;
        }
    }
}