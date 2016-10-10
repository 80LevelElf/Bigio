using System.Collections.Generic;
using Bigio.BigDictionary.Cell;
using Bigio.BigDictionary.KeyValuePair;

namespace Bigio.BigDictionary.Node
{
	public interface INode<TKey, TValue> : IEnumerable<ICell<TKey, TValue>>
	{
		void SetValue(int index, IKeyValuePair<TKey, TValue> pair);

		void Rebalance(int newCount);

		ICell<TKey, TValue> this[int index] { get; set; }

		int Count { get; }
	}
}
