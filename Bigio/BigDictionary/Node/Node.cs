using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Bigio.BigDictionary.Balancer;
using Bigio.BigDictionary.Cell;
using Bigio.BigDictionary.KeyValuePair;

namespace Bigio.BigDictionary.Node
{
	public class Node<TKey, TValue> : INode<TKey, TValue>
	{
		private bool _isSubNodesOn;
		private readonly ICell<TKey, TValue>[] _innerArray;
		private readonly IBalancer _balancer;
		private readonly int _level;

		public Node(int cellCount, int level, IBalancer balancer)
		{
			_innerArray = new ICell<TKey, TValue>[cellCount];
			_isSubNodesOn = false;
			ElementCount = cellCount;
			_balancer = balancer;
			_level = level;
		}

		public ICell<TKey, TValue> this[int index]
		{
			get
			{
				return _innerArray[index];
			}
			set
			{
				Debug.Assert(index >= 0);
				Debug.Assert(index <= _innerArray.Length);

				_innerArray[index] = value;
			}
		}

		public void SetValue(int hash, IKeyValuePair<TKey, TValue> pair)
		{
			int index = hash%Count;

			//Normally just add one more collision
			if (!_isSubNodesOn)
			{
				if (_innerArray[index] == null)
					_innerArray[index] = new ValueCell<TKey, TValue>();
			}
			//But if node it too large - rebuild asked cell to NodeCell (and add one more node)
			else
			{
				if (_innerArray[index] == null)
				{
					_innerArray[index] = new NodeCell<TKey, TValue>(new Node<TKey, TValue>(
						_balancer.GetNewNodeSize(_level + 1),
						_level + 1,
						_balancer));
				}
			}

			var cell = _innerArray[index];
			cell.Add(hash, pair);

			ElementCount++;
		}

		public void Expand(int newCount)
		{
			
		}

		public IEnumerator<ICell<TKey, TValue>> GetEnumerator()
		{
			return new NodeEnumerator<TKey, TValue>(this);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public int Count
		{
			get { return _innerArray.Length; }
		}

		public int ElementCount { get; private set; }

		private void TryToRebalance()
		{
			if (Count >= ElementCount * _balancer.GetOversizeK(_level))
			{
				var newCount = Count * _balancer.GetGrowK(_level);
				if (newCount >= _balancer.GetMaxNodeSize(_level))
				{
					_isSubNodesOn = true;
				}
				else
				{
					Expand(newCount);
				}
			}
		}
	}
}
