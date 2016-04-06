using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace PerformanceTests
{
    public class TestEngine<TList> where TList : IList<int>
    {
        private readonly TList _list;
        private const int DEFAULT_LIST_SIZE = 1000000;

        public TestEngine()
        {
            _list = (TList)Activator.CreateInstance(typeof(TList));
        }

        public List<TestResult> GetResult(TestArguments arguments)
        {
            return GetResult(arguments.MethodName, arguments.TestCountArray).ToList();
        }

        public IEnumerable<TestResult> GetResult(string methodName, int[] testCountArray = null)
        {
            if (testCountArray == null)
                testCountArray = new[] {DEFAULT_LIST_SIZE};

            Type thisType = this.GetType();
            MethodInfo theMethod = thisType.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);

            var stopWatch = new Stopwatch();

            foreach (int currentCount in testCountArray)
            {
                object[] currentCountArgument = { currentCount };

                long timeOfAllInvoketionsMs = 0;
                int countOfInvokations = 3;

                for (int i = 0; i < countOfInvokations; i++)
                {
                    stopWatch.Start();
                    theMethod.Invoke(this, currentCountArgument);
                    stopWatch.Stop();

                    timeOfAllInvoketionsMs += stopWatch.ElapsedMilliseconds;

                    stopWatch.Reset();
                }

                yield return new TestResult(currentCount, timeOfAllInvoketionsMs / countOfInvokations);
            }
        }

        protected void ClearList()
        {
            _list.Clear();
        }

        protected void FillList(int size = DEFAULT_LIST_SIZE)
        {
            ClearList();

            for (int i = 0; i < size; i++)
            {
                _list.Add(i);
            }
        }

        protected void Add(int elementCount)
        {
            ClearList();

            for (int i = 0; i < elementCount; i++)
            {
                _list.Add(i);
            }
        }
    }
}
