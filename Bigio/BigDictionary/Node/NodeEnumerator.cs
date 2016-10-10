using System.Collections;
using System.Collections.Generic;
using Bigio.BigDictionary.Cell;

namespace Bigio.BigDictionary.Node
{
	public class NodeEnumerator<TKey, TValue> : IEnumerator<ICell<TKey, TValue>>
	{
		private const int STILL_NOT_ENUMERATED = 0;

		private int _currentPosition = STILL_NOT_ENUMERATED;

		private ICell<TKey, TValue> _currentCell;

		private readonly INode<TKey, TValue> _node;

		public NodeEnumerator(INode<TKey, TValue> node)
		{
			_node = node;
		}

		public void Dispose()
		{
			
		}

		public bool MoveNext()
		{
			if (_currentPosition < _node.Count)
			{
				_currentPosition++;
				_currentCell = _node[_currentPosition];

				return true;
			}

			return false;
		}

		public void Reset()
		{
			_currentPosition = STILL_NOT_ENUMERATED;
			_currentCell = null;
		}

		public ICell<TKey, TValue> Current
		{
			get { return _currentCell; }
		}

		object IEnumerator.Current
		{
			get { return Current; }
		}
	}
}
