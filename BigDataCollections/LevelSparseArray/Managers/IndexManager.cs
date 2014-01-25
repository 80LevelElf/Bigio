using System;
using System.Collections.Generic;
using BigDataCollections.LevelSparseArray.SupportClasses;

namespace BigDataCollections.LevelSparseArray.Managers
{
    public static class IndexManager
    {
        static IndexManager()
        {
            LevelSizes = CalculateLevelSizes();
        }

        public static Index GetIndex(int primaryIndex)
        {
            if (primaryIndex < 0)
            {
                throw new ArgumentOutOfRangeException("primaryIndex");
            }

            var countOfLevels = LevelSizes.Length;
            var levelIndexes = new int[countOfLevels];

            primaryIndex = Math.Abs(primaryIndex);

            int indexer = countOfLevels - 1;
            while (primaryIndex != 0)
            {
                levelIndexes[indexer] = primaryIndex % LevelSizes[indexer];
                primaryIndex /= LevelSizes[indexer];

                indexer--;
            }

            return new Index {LevelIndexes = levelIndexes};
        }

        public static readonly int[] LevelSizes;

        //Support
        private static int[] CalculateLevelSizes()
        {
            int maxNumber = int.MaxValue;
            var sizes = new List<int>(FibonachiNumbers.Length);

            int indexOfLevel = 0;
            while (maxNumber != 0)
            {
                // Find min size of current level: for example we need to storage
                // elements with 1,2 and 3 digits, so we need to get level with 1000 elements,
                // because it is min size of level which can storage number with 3 digits.
                int currentSize = 10;
                for (int i = 1; i < FibonachiNumbers[indexOfLevel]; i++)
                {
                    currentSize *= 10;
                    if (maxNumber/currentSize == 0)
                    {
                        break;
                    }
                }

                sizes.Add(currentSize);

                maxNumber /= currentSize;
                indexOfLevel++;
            }

            sizes.Reverse();
            return sizes.ToArray();
        }
        private static readonly int[] FibonachiNumbers = { 1, 1, 2, 3, 5, 8 };
    }
}
