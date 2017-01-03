using System;
using System.Collections.Generic;

namespace Bigio.BigArray.JITO.Operations.Contains
{
	internal class ContainsOperationResolver : AbstractOperationResolver
	{
		private enum ContainsOperationType
		{
			None,
			WithIndexOf,
			WithLINQ
		}

		private ContainsOperationType FastestType;

		public ContainsOperation<T> GetOperation<T>(BigArray<T> source)
		{
			switch (FastestType)
			{
				case ContainsOperationType.WithIndexOf:
					return new ContainsOperationWithIndexOf<T>(source);
				case ContainsOperationType.WithLINQ:
					return new ContainsOperationWithLINQ<T>(source);
				case ContainsOperationType.None:
					FastestType = (ContainsOperationType)ChooseFastestMethod();
					return GetOperation(source);
				default:
					throw new IndexOutOfRangeException("There is no any handling for such contains method type!");
			}
		}

		protected override PossibleRealizationTestData GetPossibleRealizationTestData()
		{
			var source = TestSampleManager.GetSample(TestSampleManager.BigArraySize.Large);

			return new PossibleRealizationTestData(
				new Dictionary<int, object>
				{
					{ (int)ContainsOperationType.WithIndexOf, new ContainsOperationWithIndexOf<string>(source) },
					{ (int)ContainsOperationType.WithLINQ, new ContainsOperationWithLINQ<string>(source) }
				}, "Contains", new object[] { (source.Count - 1).ToString() }, 100);
		}
	}
}
