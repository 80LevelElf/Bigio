using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Wintellect.PowerCollections;

namespace PerformanceTests
{
    public class TestEngine<TList> where TList : IList<int>
    {
        private readonly TList _list;
        private const int DEFAULT_LIST_SIZE = 1000000;
        private readonly int[] _sampleArray = {1, 2, 3, 4, 5, 6, 7, 8, 9};
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private readonly Random _random = new Random();

        /// <summary>
        /// Some methods of testing lists can't be called directly via IList, because IList doesn't contains definitions of many methods
        /// such as InsertRange, AddRange and so far. So we need to call this methods with reflection
        /// </summary>
        private readonly Dictionary<string, MethodInfo> _usingReflectionMethods = new Dictionary<string, MethodInfo>();

        public TestEngine()
        {
            _list = (TList)Activator.CreateInstance(typeof(TList));

            _usingReflectionMethods.Add("InsertRangeInRandom", GetMethodsInfoOfList("InsertRange").First(i => i.GetParameters().Length == 2));
            _usingReflectionMethods.Add("AddRange", GetMethodsInfoOfList("AddRange").First(i => i.GetParameters().Length == 1));
            _usingReflectionMethods.Add("BinarySearch", GetMethodsInfoOfList("BinarySearch").First(i => i.GetParameters().Length == 1));
            _usingReflectionMethods.Add("FindAll", GetMethodsInfoOfList("FindAll").First(i => i.GetParameters().Length == 1 || i.GetParameters().Length == 2));
            _usingReflectionMethods.Add("Reverse", GetMethodsInfoOfList("Reverse").First(i => i.GetParameters().Length == 0));
        }

        public List<TestResult> GetResult(TestArguments arguments)
        {
            return GetResult(arguments.MethodName, arguments.CallFlag, arguments.TestCountArray).ToList();
        }

        public IEnumerable<TestResult> GetResult(string methodName, CallFlag flag, int[] testCountArray = null)
        {
            if (testCountArray == null)
                testCountArray = new[] {DEFAULT_LIST_SIZE};

            MethodInfo method = GetMethodInfoOfTestEngine(methodName);

            foreach (int currentCount in testCountArray)
            {
                List<object> argumentList = new List<object>{ currentCount };
                if (_usingReflectionMethods.ContainsKey(methodName))
                    argumentList.Add(_usingReflectionMethods[methodName]);

                long timeOfAllInvoketionsMs = 0;
                int countOfInvokations = 3;

                for (int i = 0; i < countOfInvokations; i++)
                {
                    timeOfAllInvoketionsMs += MeasureMethod(method, argumentList, flag);
                }

                yield return new TestResult(currentCount, timeOfAllInvoketionsMs / countOfInvokations);
            }
        }

        private long MeasureMethod(MethodInfo method, List<object> argument, CallFlag flag)
        {
            if (flag == CallFlag.ClearTestList)
                ClearList();
            if (flag == CallFlag.FillTestList)
                FillList();

            _stopwatch.Reset();
            _stopwatch.Start();

            method.Invoke(this, argument.ToArray());

            _stopwatch.Stop();
            return _stopwatch.ElapsedMilliseconds;
        }

        protected void ClearList()
        {
            _list.Clear();
        }

        protected void FillList(int size = DEFAULT_LIST_SIZE)
        {
            for (int i = 0; i < size; i++)
            {
                _list.Add(i);
            }
        }

        protected void Add(int elementCount)
        {
            for (int i = 0; i < elementCount; i++)
            {
                _list.Add(i);
            }
        }

        protected void InsertInStartPosition(int elementCount)
        {
            for (int i = 0; i < elementCount; i++)
            {
                _list.Insert(0, i);
            }
        }

        protected void InsertInMiddlePosition(int elementCount)
        {
            for (int i = 0; i < elementCount; i++)
            {
                _list.Insert(_list.Count / 2, i);
            }
        }

        protected void InsertInRandomPosition(int elementCount)
        {
            for (int i = 0; i < elementCount; i++)
            {
                _list.Insert(_random.Next(_list.Count), i);
            }
        }

        protected void For(int count)
        { 
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
            int temp = 0;
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
            for (int i = 0; i < elementCount; i++)
            {
                _list.IndexOf(i);
            }
        }

        protected void InsertRangeInRandom(int count, MethodInfo method)
        {
            for (int i = 0; i < count; i++)
            {
                method.Invoke(_list, new object[]{_random.Next(_list.Count), _sampleArray.ToList()});
            }
        }

        protected void AddRange(int count, MethodInfo method)
        {
            for (int i = 0; i < count; i++)
            {
                method.Invoke(_list, new object[] { _sampleArray.ToList() });
            }
        }

        protected void BinarySearch(int count, MethodInfo method)
        {
            for (int i = 0; i < count; i++)
            {
                method.Invoke(_list, new object[] { i });
            }
        }

        protected void FindAll(int count, MethodInfo method)
        {
            bool is2Args = method.GetParameters().Length == 2;

            for (int i = 0; i < count; i++)
            {
                var copyOfI = i;
                Predicate<int> predicate = item => item == copyOfI;

                var result = (IEnumerable<int>)method.Invoke(_list, is2Args ? new object[] { predicate, false } : new object[] { predicate });
                result.ToList(); //Because Wintellect use lazy evaluation in FindAll
            }
        }

        protected void Reverse(int count, MethodInfo method)
        {
            for (int i = 0; i < count; i++)
            {
                method.Invoke(_list, new object[] { });
            }
        }

        private IEnumerable<MethodInfo> GetMethodsInfoOfList(string methodName)
        {
            Type thisType = typeof(TList);
            return thisType.GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(i => i.Name == methodName);
        }

        private MethodInfo GetMethodInfoOfTestEngine(string methodName)
        {
            Type thisType = GetType();            
            return thisType.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
        }
    }
}
