using System.Collections;
using System.Collections.Generic;
using Bigio.BigDictionary.Balancer;
using Bigio.BigDictionary.DictionaryMap;
using Bigio.BigDictionary.Node;

namespace Bigio.BigDictionary
{
	//TODO: still in development
	class BigDictionary<TKey, TValue> : IDictionary<TKey, TValue>
	{
		private INode<TKey, TValue> _rootNode;
		private readonly IBalancer _balancer;
		private readonly IDictionaryMap<TKey, TValue> _dictionaryMap; 

		public BigDictionary()
		{
			_balancer = new FastReducedBalancer();
			_rootNode = new Node<TKey, TValue>(_balancer.GetNewNodeSize(0));
			_dictionaryMap = new StandardDictionaryMap<TKey, TValue>();
		}

		public void Add(TKey key, TValue value)
		{
			var pair = new KeyValuePair.KeyValuePair<TKey,TValue>(key, value);
			int hash = key.GetHashCode();

			var result = _dictionaryMap.FindNode(_rootNode, hash);
			var node = result.FindedNode;

			node.SetValue(hash % node.Count, pair);

			if (node.Count >= _balancer.GetMaxNodeSize(node.Count))
			{
				
			}
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			throw new System.NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Add(KeyValuePair<TKey, TValue> item)
		{
			throw new System.NotImplementedException();
		}

		public void Clear()
		{
			throw new System.NotImplementedException();
		}

		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			throw new System.NotImplementedException();
		}

		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			throw new System.NotImplementedException();
		}

		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			throw new System.NotImplementedException();
		}

		public int Count { get; private set; }
		public bool IsReadOnly { get; private set; }

		public bool ContainsKey(TKey key)
		{
			throw new System.NotImplementedException();
		}

		public bool Remove(TKey key)
		{
			throw new System.NotImplementedException();
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			throw new System.NotImplementedException();
		}

		public TValue this[TKey key]
		{
			get { throw new System.NotImplementedException(); }
			set { throw new System.NotImplementedException(); }
		}

		public ICollection<TKey> Keys { get; private set; }
		public ICollection<TValue> Values { get; private set; }
	}
}
