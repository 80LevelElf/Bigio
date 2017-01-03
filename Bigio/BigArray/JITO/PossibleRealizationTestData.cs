using System.Collections.Generic;

namespace Bigio.BigArray.JITO
{
	internal class PossibleRealizationTestData
	{
		public Dictionary<int, object> IdPerRealization { get; private set; }

		public object[] ArgumentArray { get; private set; }

		public string MethodName { get; private set; }

		public int CountOfTestInvokations { get; private set; }

		public PossibleRealizationTestData(Dictionary<int, object> idPerRealization, string methodName,
			object[] argumentArray, int countOfTestInvokations)
		{
			IdPerRealization = idPerRealization;
			MethodName = methodName;
			ArgumentArray = argumentArray;
			CountOfTestInvokations = countOfTestInvokations;
		}
	}
}
