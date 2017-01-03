using System.Collections.Generic;
using Bigio.BigArray.Support_Classes.Balancer;

namespace Bigio.BigArray.JITO
{
	internal static class TestSampleManager
	{
		public enum BigArraySize
		{
			Small,
			Middle,
			Large
		}

		private static readonly Dictionary<BigArraySize, BigArray<string>> ArrayPerSize;

		static TestSampleManager()
		{
			var balancer = new FixedBalancer();
			int blockSize = balancer.DefaultBlockSize;

			ArrayPerSize = new Dictionary<BigArraySize, BigArray<string>>
			{
				{BigArraySize.Small, GetArray(4*blockSize)},
				{BigArraySize.Middle, GetArray(16*blockSize)},
				{BigArraySize.Large, GetArray(64*blockSize)}
			};
		}

		public static BigArray<string> GetSample(BigArraySize arraySize)
		{
			return ArrayPerSize[arraySize];
		}

		private static BigArray<string> GetArray(int size)
		{
			// We have to turn off JITO (just in time optimization) for testing because:
			// 1) We can accidentally move to infinite recursive loop (and this is bad)
			// 2) We shouldn't calculate the fastest implementations of all methods
			//    we need to test current one

			var array = new BigArray<string>(new BigArrayConfiguration<string>
			{
				UseJustInTimeOptimization = false
			});

			for (int i = 0; i < size; i++)
			{
				array.Add(i.ToString());
			}

			return array;
		}
	}
}
