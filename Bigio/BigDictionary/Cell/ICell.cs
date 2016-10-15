using Bigio.BigDictionary.KeyValuePair;
using Bigio.BigDictionary.Node;

namespace Bigio.BigDictionary.Cell
{
	public interface ICell<TKey, TValue>
	{
		INode<TKey, TValue> NextNode { get; }

		void Add(int hash, IKeyValuePair<TKey, TValue> pair);
	}
}
