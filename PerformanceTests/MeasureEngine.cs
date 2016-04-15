using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace PerformanceTests
{
    public static class MeasureEngine
    {
        public static long MeasureMethod(object context, MethodInfo method, List<object> argument)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            method.Invoke(context, argument.ToArray());

            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }
    }
}
