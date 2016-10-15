using System;
using System.Collections;
using System.Collections.Generic;
using Bigio.BigDictionary.KeyValuePair;

namespace Bigio.BigDictionary.Node
{
	public class NodeValueEnumerator<TKey, TValue> : IEnumerator<IKeyValuePair<TKey, TValue>>
	{
		public void Dispose()
		{
			throw new NotImplementedException();
		}

		public bool MoveNext()
		{
			throw new NotImplementedException();
		}

		public void Reset()
		{
			throw new NotImplementedException();
		}

		public IKeyValuePair<TKey, TValue> Current { get; }

		object IEnumerator.Current
		{
			get { return Current; }
		}
	}
}
