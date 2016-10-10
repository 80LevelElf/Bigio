namespace Bigio.BigDictionary.Balancer
{
	public interface IBalancer
	{
		int GetNewNodeSize(int level);

		int GetMaxNodeSize(int level);
	}
}
