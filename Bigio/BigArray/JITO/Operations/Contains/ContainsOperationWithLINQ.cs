using System.Collections.Generic;
using System.Linq;

namespace Bigio.BigArray.JITO.Operations.Contains
{
	internal class ContainsOperationWithLINQ<T> : ContainsOperation<T>
	{
		public ContainsOperationWithLINQ(BigArray<T> source) : base(source)
		{
		}

		public override bool Contains(T item)
		{
			// We have to implement Contains method as in LINQ instead of using LINQ implementation
			// because standard implementation can try to use BigArray.Contains:
			// https://github.com/Microsoft/referencesource/blob/master/System.Core/System/Linq/Enumerable.cs
			var comparer = EqualityComparer<T>.Default;

			return Source.Any(currentItem => comparer.Equals(currentItem, item));
		}
	}
}
