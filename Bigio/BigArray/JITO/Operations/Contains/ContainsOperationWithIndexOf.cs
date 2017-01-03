namespace Bigio.BigArray.JITO.Operations.Contains
{
	internal class ContainsOperationWithIndexOf<T> : ContainsOperation<T>
	{
		public ContainsOperationWithIndexOf(BigArray<T> source) : base(source)
		{
		}

		public override bool Contains(T item)
		{
			return Source.IndexOf(item) != -1;
		}
	}
}
