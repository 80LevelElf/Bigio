using Bigio.BigDictionary.Node;

namespace Bigio.BigDictionary.DictionaryMap
{
	public abstract class AbstractDictionaryMap<TKey, TValue> : IDictionaryMap<TKey, TValue>
	{
		public abstract FindNodeResult<TKey, TValue> FindNode(INode<TKey, TValue> rootNode, int hash);

		public FindNodeResult<TKey, TValue> FindNode(INode<TKey, TValue> rootNode, TKey key)
		{
			return FindNode(rootNode, key.GetHashCode());
		}
	}
}
