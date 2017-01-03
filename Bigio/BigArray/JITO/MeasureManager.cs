using System.Diagnostics;
using System.Reflection;

namespace Bigio.BigArray.JITO
{
	internal static class MeasureManager
	{
		public static long MeasureMethod(object context, MethodInfo method, object[] argumentArray)
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();

			method.Invoke(context, argumentArray);

			stopwatch.Stop();
			return stopwatch.ElapsedMilliseconds;
		}

		public static long MeasureMethod(MethodInfo method, object[] argumentArray)
		{
			return MeasureMethod(null, method, argumentArray);
		}
	}
}
