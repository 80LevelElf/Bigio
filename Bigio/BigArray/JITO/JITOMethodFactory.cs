using Bigio.BigArray.JITO.Operations.Contains;

namespace Bigio.BigArray.JITO
{
	internal static class JITOMethodFactory
	{
		private static readonly ContainsOperationResolver ContainsResolver = new ContainsOperationResolver();

		public static ContainsOperation<T> GetContainsOperation<T>(BigArray<T> source, bool isJITOTurnOn)
		{
			if (isJITOTurnOn)
				return ContainsResolver.GetOperation(source);

			return new ContainsOperationWithIndexOf<T>(source);
		} 
	}
}
