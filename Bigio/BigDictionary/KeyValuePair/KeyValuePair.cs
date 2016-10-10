using System.Text;

namespace Bigio.BigDictionary.KeyValuePair
{
	//TODO: Maybe struct?
	public class KeyValuePair<TKey, TValue> : IKeyValuePair<TKey, TValue>
	{
		public KeyValuePair(TKey key, TValue value)
		{
			Key = key;
			Value = value;
		}

		public TKey Key { get; private set; }
		public TValue Value { get; private set; }

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();

			builder.Append('[');
			if (Key != null)
			{
				builder.Append(Key);
			}

			builder.Append(", ");
			if (Value != null)
			{
				builder.Append(Value);
			}
			builder.Append(']');

			return builder.ToString();
		}
	}
}
