using Bigio.BigDictionary.KeyValuePair;
using Bigio.BigDictionary.Node;

namespace Bigio.BigDictionary.Cell
{
	public class ValueCell<TKey, TValue> : ICell<TKey, TValue>
	{
		public CollisionItem<TKey, TValue> FirstCollision { get; set; }

		public INode<TKey, TValue> NextNode
		{
			get { return null; }
		}

		public void Add(IKeyValuePair<TKey, TValue> pair)
		{
			FirstCollision = new CollisionItem<TKey, TValue>(pair, FirstCollision);
		}
	}
}
