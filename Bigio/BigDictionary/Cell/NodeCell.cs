using Bigio.BigDictionary.KeyValuePair;
using Bigio.BigDictionary.Node;

namespace Bigio.BigDictionary.Cell
{
	public class NodeCell<TKey, TValue> : ICell<TKey, TValue>
	{
		public NodeCell(INode<TKey, TValue> nextNode)
		{
			NextNode = nextNode;
		} 

		public INode<TKey, TValue> NextNode { get; private set; }

		public void Add(int hash, IKeyValuePair<TKey, TValue> pair)
		{
			NextNode.SetValue(hash, pair);
		}
	}
}
