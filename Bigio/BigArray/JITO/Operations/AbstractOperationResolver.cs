using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bigio.BigArray.JITO.Operations
{
	internal abstract class AbstractOperationResolver
	{
		protected int ChooseFastestMethod()
		{
			long minTime = long.MaxValue;
			int index = 0;

			var possibleRealizationData = GetPossibleRealizationTestData();

			if (possibleRealizationData.IdPerRealization.Count == 0)
				throw new ArgumentOutOfRangeException("possibleRealizationList",
					"There is no any possible realizations of method for " + GetType().Name);
			
			var argumentArray = possibleRealizationData.ArgumentArray;
			foreach (KeyValuePair<int, object> keyValuePair in possibleRealizationData.IdPerRealization)
			{
				var possibleRealization = keyValuePair.Value;
				var methodInfo = GetMethodInfo(possibleRealization, possibleRealizationData.MethodName);
				long currentTime = GetExecutionTime(possibleRealization, methodInfo, argumentArray,
					possibleRealizationData.CountOfTestInvokations);

				if (currentTime < minTime)
				{
					minTime = currentTime;
					index = keyValuePair.Key;
				}
			}

			return index;
		}

		protected abstract PossibleRealizationTestData GetPossibleRealizationTestData();

		private long GetExecutionTime(object context, MethodInfo method, object[] argumentArray,
			int countOfInvokations)
		{
			long timeOfAllInvoketionsMs = 0;

			for (int i = 0; i < countOfInvokations; i++)
			{
				timeOfAllInvoketionsMs += MeasureManager.MeasureMethod(context, method, argumentArray);
			}

			return timeOfAllInvoketionsMs;
		}

		private MethodInfo GetMethodInfo(object obj, string methodName)
		{
			Type thisType = obj.GetType();
			return thisType.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);
		}
	}
}
