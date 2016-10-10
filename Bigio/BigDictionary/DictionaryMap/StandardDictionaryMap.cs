using Bigio.BigDictionary.Cell;
using Bigio.BigDictionary.Node;

namespace Bigio.BigDictionary.DictionaryMap
{
	public class StandardDictionaryMap<TKey, TValue> : AbstractDictionaryMap<TKey, TValue>
	{
		public override FindNodeResult<TKey, TValue> FindNode(INode<TKey, TValue> rootNode, int hash)
		{
			return FindNodeRecursively(rootNode, hash, 0);
		}

		private static FindNodeResult<TKey, TValue> FindNodeRecursively(INode<TKey, TValue> currentNode, int hash, int level)
		{
			ICell<TKey, TValue> neededCell = currentNode[hash % currentNode.Count];
			INode<TKey, TValue> nextNode = neededCell.NextNode;

			if (nextNode != null)
			{
				level++;
				return FindNodeRecursively(nextNode, hash, level);
			}

			return new FindNodeResult<TKey, TValue>
			{
				FindedNode = currentNode,
				Level = 0
			};
		}
	}
}
