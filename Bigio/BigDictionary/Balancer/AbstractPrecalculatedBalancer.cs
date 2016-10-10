using System;

namespace Bigio.BigDictionary.Balancer
{
	public abstract class AbstractPrecalculatedBalancer : IBalancer
	{
		protected abstract int[] PrecalculatedSizeArray { get; }

		protected abstract int[] PrecalculatedMaxSizeArray { get; }

		public virtual int GetNewNodeSize(int level)
		{
			return PrecalculatedSizeArray[Math.Min(level, PrecalculatedSizeArray.Length)];
		}

		public int GetMaxNodeSize(int level)
		{
			return PrecalculatedMaxSizeArray[Math.Min(level, PrecalculatedSizeArray.Length)];
		}
	}
}
