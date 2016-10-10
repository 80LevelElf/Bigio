namespace Bigio.BigDictionary.KeyValuePair
{
	public interface IKeyValuePair<TKey, TValue>
	{
		TKey Key { get; }

		TValue Value { get; }
	}
}
