using System;

namespace Bigio.BigDictionary.Balancer
{
	public abstract class AbstractPrecalculatedBalancer : IBalancer
	{
		protected abstract int[] PrecalculatedSizeArray { get; }

		protected abstract int[] PrecalculatedMaxSizeArray { get; }

		protected abstract int[] PrecalculateOversizeKArray { get; }

		protected abstract int[] PrecalculateGrowKArray { get; }

		public virtual int GetNewNodeSize(int level)
		{
			return PrecalculatedSizeArray[Math.Min(level, PrecalculatedSizeArray.Length)];
		}

		public int GetMaxNodeSize(int level)
		{
			return PrecalculatedMaxSizeArray[Math.Min(level, PrecalculatedMaxSizeArray.Length)];
		}

		public int GetOversizeK(int level)
		{
			return PrecalculateOversizeKArray[Math.Min(level, PrecalculateOversizeKArray.Length)];
		}

		public int GetGrowK(int level)
		{
			return PrecalculateGrowKArray[Math.Min(level, PrecalculateGrowKArray.Length)];
		}
	}
}
