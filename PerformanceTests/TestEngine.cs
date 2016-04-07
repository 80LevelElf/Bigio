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
        private readonly int[] _sampleArray = {1, 2, 3, 4, 5, 6, 7, 8, 9};

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

        protected void InsertInStartPosition(int elementCount)
        {
            ClearList();

            for (int i = 0; i < elementCount; i++)
            {
                _list.Insert(0, i);
            }
        }

        protected void InsertInMiddlePosition(int elementCount)
        {
            ClearList();

            for (int i = 0; i < elementCount; i++)
            {
                _list.Insert(_list.Count / 2, i);
            }
        }

        protected void For(int count)
        { 
            if (_list.Count != DEFAULT_LIST_SIZE)
                FillList();

            //We need to do it 'count' time otherwise test of For will be test of Add
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < _list.Count; j++)
                {
                    _list[j] ++;
                }
            }
        }

        protected void Foreach(int count)
        {
            if (_list.Count != DEFAULT_LIST_SIZE)
                FillList();

            int temp = 0;

            //We need to do it 'count' time otherwise test of For will be test of Add
            for (int i = 0; i < count; i++)
            {
                foreach (var item in _list)
                {
                    temp = item;
                }
            }
        }

        protected void IndexOf(int elementCount)
        {
            if (_list.Count != elementCount)
                FillList(elementCount);

            for (int i = 0; i < elementCount; i++)
            {
                _list.IndexOf(i);
            }
        }
    }
}
