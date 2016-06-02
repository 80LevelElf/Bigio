using System;
using System.Collections;
using System.Collections.Generic;

namespace Bigio.BigArray.Support_Classes.BlockStructure
{
    /// <summary>
    /// Object of <see cref="MultyblockRange"/> contain information of range that can overlap many blocks. 
    /// </summary>
    class MultyblockRange : IEnumerable<BlockRange>
    {
        //Data

        /// <summary>
        /// Internal value of <see cref="IndexOfStartBlock"/> field. Dont use it out of <see cref="IndexOfStartBlock"/>
        /// set and get functions.
        /// </summary>
        private int _indexOfStartBlock;

        /// <summary>
        /// Internal value of Count field. Don't use it from your code. Use Count instead.
        /// </summary>
        private int _count;

        /// <summary>
        /// Internal value of <see cref="Ranges"/> field. Dont use it out of <see cref="Ranges"/>
        /// set and get functions.
        /// </summary>
        private IEnumerable<BlockRange> _ranges;

        //API

        /// <summary>
        /// Create new instance of <see cref="MultyblockRange"/>.
        /// </summary>
        /// <param name="indexOfStartBlock">Zero-based index of the start block in <see cref="BlockCollection{T}"/>.</param>
        /// <param name="count"></param>
        /// <param name="ranges"><see cref="BlockRange"/>s of blocks from the start block.</param>
        public MultyblockRange(int indexOfStartBlock, int count, IEnumerable<BlockRange> ranges)
        {
            IndexOfStartBlock = indexOfStartBlock;
            Ranges = ranges;
            Count = count;
        }

        /// <summary>
        /// Check equal of current MultyblockRange and other <see cref="MultyblockRange"/>.
        /// </summary>
        /// <param name="other">Other <see cref="MultyblockRange"/> to check.</param>
        /// <returns>If <see cref="BlockRange"/>s are the same in all data members
        /// (<see cref="IndexOfStartBlock"/> and <see cref="Count"/>) and containes same ranges return true,
        ///  otherwise return false.</returns>
        public bool Equals(MultyblockRange other)
        {
            var enumerator = GetEnumerator();
            var otherEnumerator = GetEnumerator();

            if (IndexOfStartBlock != other.IndexOfStartBlock || Count != other.Count)
                return false;

            //Compare all elements
            while (enumerator.MoveNext() && otherEnumerator.MoveNext())
            {
                if (!enumerator.Current.Equals(otherEnumerator.Current))
                    return false;
            }

            //If we crossed entire 2 collection, it will return true. If we crossed just 1 collection,
            // it will return false.
            return (enumerator.MoveNext() == otherEnumerator.MoveNext());
        }

        public IEnumerator<BlockRange> GetEnumerator()
        {
            return _ranges.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Count of <see cref="BlockRange"/>s containing in <see cref="MultyblockRange"/>.
        /// </summary>
        public int Count
        {
            get
            {
                return _count;
            }
            private set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value");

                _count = value;
            }
        }

        /// <summary>
        /// Zero-based index of the start block in <see cref="Bigio.BigArray.Support_Classes.BlockCollection"/>.
        /// </summary>
        public int IndexOfStartBlock
        {
            get
            {
                return _indexOfStartBlock;
            }
            private set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value");

                _indexOfStartBlock = value;
            }
        }

        /// <summary>
        /// Block ranges of blocks from the start block.
        /// </summary>
        public IEnumerable<BlockRange> Ranges
        {
            get
            {
                return _ranges;
            }
            private set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _ranges = value;
            }
        }
    }
}
