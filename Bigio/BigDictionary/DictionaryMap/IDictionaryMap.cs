using Bigio.BigDictionary.Node;

namespace Bigio.BigDictionary.DictionaryMap
{
	public interface IDictionaryMap<TKey, TValue>
	{
		FindNodeResult<TKey, TValue> FindNode(INode<TKey, TValue> rootNode, int hash);

		FindNodeResult<TKey, TValue> FindNode(INode<TKey, TValue> rootNode, TKey key);
	}
}
