using Bigio.BigDictionary.Node;

namespace Bigio.BigDictionary.DictionaryMap
{
	public struct FindNodeResult<TKey, TValue>
	{
		public INode<TKey, TValue> FindedNode { get; set; }

		public int Level { get; set; }
	}
}
