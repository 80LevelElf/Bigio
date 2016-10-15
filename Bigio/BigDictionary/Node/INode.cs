using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Bigio.BigDictionary.Cell;
using Bigio.BigDictionary.KeyValuePair;

namespace Bigio.BigDictionary.Node
{
	public interface INode<TKey, TValue> : IEnumerable<ICell<TKey, TValue>>
	{
		void SetValue(int hash, IKeyValuePair<TKey, TValue> pair);

		void Expand(int newCount);

		ICell<TKey, TValue> this[int index] { get; set; }

		int Count { get; }
	}
}
