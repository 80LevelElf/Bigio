namespace Bigio.BigDictionary.Balancer
{
	public interface IBalancer
	{
		int GetNewNodeSize(int level);

		int GetMaxNodeSize(int level);

		int GetOversizeK(int level);

		int GetGrowK(int level);
	}
}
