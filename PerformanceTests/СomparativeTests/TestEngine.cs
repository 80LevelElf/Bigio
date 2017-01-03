using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PerformanceTests.СomparativeTests
{
    public abstract class TestEngine<TList, T> where TList : IList<T>
    {
        private readonly TList _list;
        private const int DEFAULT_LIST_SIZE = 1000000;
        private readonly Random _random = new Random();

        /// <summary>
        /// Some methods of testing lists can't be called directly via IList, because IList doesn't contains definitions of many methods
        /// such as InsertRange, AddRange and so far. So we need to call this methods with reflection
        /// </summary>
        private readonly Dictionary<string, MethodInfo> _usingReflectionMethods = new Dictionary<string, MethodInfo>();

	    protected abstract T GetValue(int i);

	    protected abstract List<T> GetSampleList();

        public TestEngine()
        {
            _list = (TList)Activator.CreateInstance(typeof(TList));
            
            _usingReflectionMethods.Add("InsertRangeInRandom", GetMethodsInfoOfList("InsertRange").First(i => i.GetParameters().Length == 2));
            _usingReflectionMethods.Add("AddRange", GetMethodsInfoOfList("AddRange").First(i => i.GetParameters().Length == 1));
            _usingReflectionMethods.Add("BinarySearch", GetMethodsInfoOfList("BinarySearch").First(i => i.GetParameters().Length == 1));
            _usingReflectionMethods.Add("FindAll", GetMethodsInfoOfList("FindAll").First(i => i.GetParameters().Length == 1 || i.GetParameters().Length == 2));
            _usingReflectionMethods.Add("Reverse", GetMethodsInfoOfList("Reverse").First(i => i.GetParameters().Length == 0));
            _usingReflectionMethods.Add("LastIndexOf", GetMethodsInfoOfList("LastIndexOf").First(i => i.GetParameters().Length == 1));
            _usingReflectionMethods.Add("Find", GetMethodsInfoOfList("Find").First(i => i.GetParameters().Length == 1));
            _usingReflectionMethods.Add("FindLast", GetMethodsInfoOfList("FindLast").First(i => i.GetParameters().Length == 1));
        }

        public List<TestResult> GetResult(TestArguments arguments)
        {
            return GetResult(arguments.MethodName, arguments.CallFlag, arguments.TestCountArray).ToList();
        }

        private IEnumerable<TestResult> GetResult(string methodName, CallFlag flag, int[] testCountArray = null)
        {
            if (testCountArray == null)
                testCountArray = new[] {DEFAULT_LIST_SIZE};

            MethodInfo method = GetMethodInfoOfTestEngine(methodName);

            foreach (int currentCount in testCountArray)
            {
                List<object> argumentList = new List<object>{ currentCount };
                if (_usingReflectionMethods.ContainsKey(methodName))
                    argumentList.Add(_usingReflectionMethods[methodName]);

                //Get middle estimation of several times calling
                long timeOfAllInvoketionsMs = 0;
                int countOfInvokations = 3;

                for (int i = 0; i < countOfInvokations; i++)
                {
                    if (flag == CallFlag.ClearTestCollection)
                        ClearList();
                    if (flag == CallFlag.FillTestCollection)
                        FillList();

                    timeOfAllInvoketionsMs += MeasureEngine.MeasureMethod(this, method, argumentList);
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
            for (int i = 0; i < size; i++)
            {
                _list.Add(GetValue(i));
            }
        }

        protected void Add(int elementCount)
        {
            for (int i = 0; i < elementCount; i++)
            {
                _list.Add(GetValue(i));
            }
        }

        protected void InsertInStartPosition(int elementCount)
        {
            for (int i = 0; i < elementCount; i++)
            {
                _list.Insert(0, GetValue(i));
            }
        }

        protected void InsertInMiddlePosition(int elementCount)
        {
            for (int i = 0; i < elementCount; i++)
            {
                _list.Insert(_list.Count / 2, GetValue(i));
            }
        }

        protected void InsertInRandomPosition(int elementCount)
        {
            for (int i = 0; i < elementCount; i++)
            {
                _list.Insert(_random.Next(_list.Count), GetValue(i));
            }
        }

        protected void For(int count)
        {
	        T temp;
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < _list.Count; j++)
                {
	                temp = _list[j];
                }
            }
        }

        protected void Foreach(int count)
        {
            T temp;
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
                _list.IndexOf(GetValue(i));
            }
        }

        protected void InsertRangeInRandom(int count, MethodInfo method)
        {
            for (int i = 0; i < count; i++)
            {
                method.Invoke(_list, new object[]{ _random.Next(_list.Count), GetSampleList()});
            }
        }

        protected void AddRange(int count, MethodInfo method)
        {
            for (int i = 0; i < count; i++)
            {
                method.Invoke(_list, new object[] { GetSampleList() });
            }
        }

        protected void BinarySearch(int count, MethodInfo method)
        {
            for (int i = 0; i < count; i++)
            {
                method.Invoke(_list, new object[] { GetValue(i) });
            }
        }

        protected void FindAll(int count, MethodInfo method)
        {
            bool is2Args = method.GetParameters().Length == 2;

            for (int i = 0; i < count; i++)
            {
                var copyOfI = i;
                Predicate<T> predicate = item => item.Equals(GetValue(copyOfI));

                var result = (IEnumerable<T>)method.Invoke(_list, is2Args ? new object[] { predicate, false } : new object[] { predicate });
                result.ToList(); //Because Wintellect use lazy evaluation in FindAll
            }
        }

        protected void Find(int count, MethodInfo method)
        {
            for (int i = 0; i < count; i++)
            {
                var copyOfI = i;
				Predicate<T> predicate = item => item.Equals(GetValue(copyOfI));

				method.Invoke(_list, new object[] { predicate });
            }
        }

        protected void FindLast(int count, MethodInfo method)
        {
            for (int i = 0; i < count; i++)
            {
                var copyOfI = i;
				Predicate<T> predicate = item => item.Equals(GetValue(copyOfI));

				method.Invoke(_list, new object[] { predicate });
            }
        }

        protected void Reverse(int count, MethodInfo method)
        {
            for (int i = 0; i < count; i++)
            {
                method.Invoke(_list, new object[] { });
            }
        }

        protected void LastIndexOf(int count, MethodInfo method)
        {
            for (int i = 0; i < count; i++)
            {
                method.Invoke(_list, new object[] { GetValue(i) });
            }
        }

		protected void Contains(int count)
		{
			for (int i = 0; i < count; i++)
			{
				var temp = _list.Contains(GetValue(i));
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
