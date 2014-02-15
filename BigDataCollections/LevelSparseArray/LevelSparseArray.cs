using System;
using System.Collections.Generic;
using BigDataCollections.LevelSparseArray.Managers;
using BigDataCollections.LevelSparseArray.SupportClasses;
using BigDataCollections.LevelSparseArray.SupportClasses.Level;

namespace BigDataCollections.LevelSparseArray
{
    class LevelSparseArray<T>
    {
        //API
        public LevelSparseArray()
        {
            _firstLevel = new Level(IndexManager.LevelSizes[0]);
        }
        public T this[int index]
        {
            set
            {
                DataLevel(index).Data = value;
            }
            get
            {
                return DataLevel(index).Data;
            }
        }

        //Support
        private DataLevel<T> DataLevel(int index)
        {
            return DataLevel(IndexManager.GetIndex(index));
        }
        private DataLevel<T> DataLevel(Index index)
        {
            var currentLevel = _firstLevel;
            var levelCount = index.LevelIndexes.Length;

            for (int i = 0; i < levelCount; i++)
            {
                var levelIndex = index.LevelIndexes[i];

                //If level don't exist, we will cteate it
                if (currentLevel[levelIndex] == null)
                {
                    bool isLastLevel = (i == levelCount - 1);

                    if (isLastLevel)
                    {
                        currentLevel[levelIndex] = new DataLevel<T>();
                    }
                    else
                    {
                        currentLevel[levelIndex] = new Level(IndexManager.LevelSizes[i]);
                    }
                }

                currentLevel = currentLevel[levelIndex];
            }

            return (DataLevel<T>)currentLevel;
        }

        //Data
        private readonly Level _firstLevel;
    }
}
