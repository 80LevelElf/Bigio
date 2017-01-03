namespace Bigio.BigArray.JITO.Operations
{
	internal abstract class AbstractOperation<T>
	{
		protected BigArray<T> Source { get; private set; }

		protected AbstractOperation(BigArray<T> source)
		{
			Source = source;
		}
	}
}
