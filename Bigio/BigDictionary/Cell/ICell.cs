using Bigio.BigDictionary.Node;

namespace Bigio.BigDictionary.Cell
{
	public interface ICell<TKey, TValue>
	{
		INode<TKey, TValue> NextNode { get; }
	}
}
