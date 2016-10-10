namespace Bigio.BigDictionary.Balancer
{
	public class FastReducedBalancer : AbstractPrecalculatedBalancer
	{
		private static readonly int[] PrecalculatedSizeArrayStatic = {1024, 8, 2, 1};

		private static readonly int[] PrecalculatedMaxSizeArrayStatic = { 1024*8, 8*8, 8 * 4 };

		protected override int[] PrecalculatedSizeArray
		{
			get { return PrecalculatedSizeArrayStatic; }
		}

		protected override int[] PrecalculatedMaxSizeArray
		{
			get { return PrecalculatedMaxSizeArrayStatic; }
		}
	}
}
