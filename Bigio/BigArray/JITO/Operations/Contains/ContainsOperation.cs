namespace Bigio.BigArray.JITO.Operations.Contains
{
	internal abstract class ContainsOperation<T> : AbstractOperation<T>
	{
		protected ContainsOperation(BigArray<T> source) : base(source)
		{
		}

		public abstract bool Contains(T item);
	}
}
