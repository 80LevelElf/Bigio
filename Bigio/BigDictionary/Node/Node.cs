using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Bigio.BigDictionary.Cell;
using Bigio.BigDictionary.KeyValuePair;

namespace Bigio.BigDictionary.Node
{
	public class Node<TKey, TValue> : INode<TKey, TValue>
	{
		private readonly ICell<TKey, TValue>[] _innerArray;

		public Node(int cellCount)
		{
			_innerArray = new ICell<TKey, TValue>[cellCount];
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

		public void SetValue(int index, IKeyValuePair<TKey, TValue> pair)
		{
			if (_innerArray[index] == null)
				_innerArray[index] = new ValueCell<TKey, TValue>();

			var cell = _innerArray[index];
			
			Debug.Assert(cell is ValueCell<TKey, TValue>);

			var valueCell = (ValueCell<TKey, TValue>)cell;
			valueCell.Add(pair);
		}

		public void Rebalance(int newCount)
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

		public int Count { get; set; }
	}
}
