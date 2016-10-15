namespace Bigio.BigDictionary.Balancer
{
	public class FastReducedBalancer : AbstractPrecalculatedBalancer
	{
		private static readonly int[] PrecalculatedSizeArrayStatic = {1024, 8, 2, 1};

		private static readonly int[] PrecalculatedMaxSizeArrayStatic = { 1024*8, 8*8, 8 * 4 };

		private static readonly int[] PrecalculateOversizeKArrayStatic = { 2 };

		private static readonly int[] PrecalculateGrowKArrayStatic = { 2 };

		protected override int[] PrecalculatedSizeArray
		{
			get { return PrecalculatedSizeArrayStatic; }
		}

		protected override int[] PrecalculatedMaxSizeArray
		{
			get { return PrecalculatedMaxSizeArrayStatic; }
		}

		protected override int[] PrecalculateOversizeKArray
		{
			get { return PrecalculateOversizeKArrayStatic; }
		}

		protected override int[] PrecalculateGrowKArray
		{
			get { return PrecalculateGrowKArrayStatic; }
		}
	}
}
