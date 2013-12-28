using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BigDataCollections.LevelSparseArray.SupportClasses;

namespace BigDataCollections.LevelSparseArray.Managers
{
    public static class IndexManager
    {
        static IndexManager()
        {
            MaxDegree = GetDegree(int.MaxValue);
        }

       /* public static Index GetIndex(int primaryIndex)
        {
            bool isPlus = primaryIndex >= 0;
            primaryIndex = Math.Abs(primaryIndex);

            while (primaryIndex != 0)
            {
                
            }
        }*/

        public static int MaxDegree { private set; get; }

        //Support
        private static int GetDegree(int number)
        {
            int count = 0;
            while (number != 0)
            {
                number /= 10;
                count++;
            }

            return count;
        }

        private static int Pow(int number, int degree)
        {
            if (degree < 0)
            {
                throw new ArgumentOutOfRangeException("degree", "Degree cant be less than 0!");
            }

            int result = 1;
            for (int i = 0; i < degree; i++)
            {
                result *= number;
            }

            return result;
        }

        //Data
        private static int[] _fibonacciNumbers = {1, 1, 2, 3, 5, 8, 13, 21};
    }
}
