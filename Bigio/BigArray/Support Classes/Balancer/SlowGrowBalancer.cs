using System;
using System.Collections.Generic;

namespace Bigio.BigArray.Support_Classes.Balancer
{
    /// <summary>
    /// Balancer which provide you slowly growing sizes of block calculated by function:
    /// 512 + 7 * maxExistentIndex * Math.Log(1 + Math.Pow(maxExistentIndex, 10))
    /// </summary>
    public class SlowGrowBalancer : AbstractFunctionBalancer
    {
        private static List<int> StaticPrecalculatedSizeList { set; get; }
        private static int StaticMaxExistentIndex { get; set; }

        static SlowGrowBalancer()
        {
            StaticPrecalculatedSizeList = new List<int>();

            int summ = 0;
            unchecked
            {
                int maxExistentIndex = -1;

                while (true)
                {
                    int value = (int)(512 + 7 * maxExistentIndex * Math.Log(1 + Math.Pow(maxExistentIndex, 10)));
                    summ += value;

                    //Overflow
                    if (summ < 0)
                    {
                        StaticMaxExistentIndex = maxExistentIndex;
                        return;
                    }

                    StaticPrecalculatedSizeList.Add(value);
                    maxExistentIndex++;
                }
            }
        }

        protected override List<int> PrecalculatedSizeList
        {
            get { return StaticPrecalculatedSizeList; }
        }

        protected override int MaxExistentIndex
        {
            get { return StaticMaxExistentIndex; }
        }
    }
}