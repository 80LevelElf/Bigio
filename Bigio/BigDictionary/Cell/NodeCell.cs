using Bigio.BigDictionary.Node;

namespace Bigio.BigDictionary.Cell
{
	public class NodeCell<TKey, TValue> : ICell<TKey, TValue>
	{
		public INode<TKey, TValue> NextNode { get; private set; }
	}
}
