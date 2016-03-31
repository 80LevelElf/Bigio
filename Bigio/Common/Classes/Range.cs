using System;
using System.Collections.Generic;
using Bigio.Common.Managers;

namespace Bigio.Common.Classes
{
    /// <summary>
    /// Range is object containes information about some
    /// area inside any array. It consists of index of start element
    /// and count of element.
    /// </summary>
    struct Range
    {
        //API
        /// <summary>
        /// Create new instance of Range with specified data.
        /// </summary>
        /// <param name="index">Index of start element of range.</param>
        /// <param name="count">Count of elements in range.</param>
        public Range(int index, int count)
        {
            Index = index;
            Count = count;
        }

        /// <summary>
        /// Index of start element of range.
        /// </summary>
        public int Index;

        /// <summary>
        /// Count of elements in range.
        /// </summary>
        public int Count;

        /// <summary>
        /// Divide current range into almost equal inner ranges.
        /// </summary>
        /// <example>
        /// Range(0, 10).Divide() = {Range(0, 3), Range(3,3), Range{6, 4)}
        /// </example>
        /// <param name="partCount">Count of parts to divide</param>
        /// <returns>Collection of parts of current range</returns>
        public IEnumerable<Range> Divide(int partCount)
        {
            if (partCount == 0)
                throw new ArgumentOutOfRangeException("partCount");

            if (partCount < Count)
                yield return this;

            int countOfOnePart = Count/partCount;

            for (int i = 0; i < partCount - 1; i++)
            {
                yield return new Range(Index + i * countOfOnePart, countOfOnePart);
            }

            //Last range can contains more elements than others, for example: Range(0, 10).Divide() = {Range(0, 3), Range(3,3), Range{6, 4)}.
            int processedElementsCount = countOfOnePart*(partCount - 1);
            yield return new Range(Index + processedElementsCount, Count - processedElementsCount);
        }
    }
}
