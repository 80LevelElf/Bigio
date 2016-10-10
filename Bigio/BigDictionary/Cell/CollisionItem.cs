using System.Diagnostics;
using Bigio.BigDictionary.KeyValuePair;

namespace Bigio.BigDictionary.Cell
{
	public class CollisionItem<TKey, TValue>
	{
		public CollisionItem(IKeyValuePair<TKey, TValue> pair)
		{
			Debug.Assert(pair != null);

			Pair = pair;
		}

		public CollisionItem(IKeyValuePair<TKey, TValue> pair, CollisionItem<TKey, TValue> previous)
			: this(pair)
		{
			previous.Next = this;
		}

		public IKeyValuePair<TKey, TValue> Pair { get; set; }

		public CollisionItem<TKey, TValue> Next { get; set; }  
	}
}
