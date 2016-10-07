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
        private static int[] StaticPrecalculatedSizeArray { set; get; }

        static SlowGrowBalancer()
        {
            var sizeList = new List<int>();

            int summ = 0;
            unchecked
            {
                int i = -1;

                while (true)
                {
                    int value = (int)(512 + 7 * i * Math.Log(1 + Math.Pow(i, 10)));
                    summ += value;

                    //Overflow
                    if (summ < 0)
                    {
	                    StaticPrecalculatedSizeArray = sizeList.ToArray();
                        return;
                    }

					sizeList.Add(value);
                    i++;
                }
            }
        }

		protected override int[] PrecalculatedSizeArray
        {
            get { return StaticPrecalculatedSizeArray; }
        }
    }
}