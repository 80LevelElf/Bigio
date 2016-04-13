using System;
using System.Collections.Generic;
using Bigio;
using Wintellect.PowerCollections;

namespace PerformanceTests
{
    public static class TestManager
    {
        private const int HUNDRED = 100;
        private const int THOUSAND = 1000;
        private const int MILLION = THOUSAND * THOUSAND;
        private const int BILLIARD = MILLION * THOUSAND;

        private static readonly TestEngine<BigArray<int>> _bigioTestEngine = new TestEngine<BigArray<int>>();
        private static readonly TestEngine<BigList<int>> _wintellectTestEngine = new TestEngine<BigList<int>>();
        private static readonly TestEngine<List<int>> _listTestEngine = new TestEngine<List<int>>();

        public static void TestAdd()
        {
            TestArguments arg = new TestArguments("Add", CallFlag.ClearTestList, new[] { MILLION / 10, MILLION, MILLION * 10, MILLION * 100});
            WriteResult("Add", _bigioTestEngine.GetResult(arg), _wintellectTestEngine.GetResult(arg), _listTestEngine.GetResult(arg));
        }

        public static void TestAddRange()
        {
            WriteResult("AddRange",
                _bigioTestEngine.GetResult(new TestArguments("AddRange", CallFlag.ClearTestList, new[] { MILLION / 100, MILLION / 10, MILLION })),
                _wintellectTestEngine.GetResult(new TestArguments("AddRange", CallFlag.ClearTestList, new[] { MILLION / 100, MILLION / 10, MILLION })),
                _listTestEngine.GetResult(new TestArguments("AddRange", CallFlag.ClearTestList, new[] { MILLION / 100, MILLION / 10, MILLION })));
        }

        public static void TestInsertInStartPosition()
        {
            WriteResult("InsertInStartPosition",
                _bigioTestEngine.GetResult(new TestArguments("InsertInStartPosition", CallFlag.ClearTestList, new[] { MILLION / 100, MILLION / 10, MILLION, MILLION * 10 })),
                _wintellectTestEngine.GetResult(new TestArguments("InsertInStartPosition", CallFlag.ClearTestList, new[] { MILLION / 100, MILLION / 10, MILLION, MILLION * 10 })),
                _listTestEngine.GetResult(new TestArguments("InsertInStartPosition", CallFlag.ClearTestList, new[] { MILLION / 100, MILLION / 10 })));
        }

        public static void TestInsertInMiddlePosition()
        {
            WriteResult("InsertInMiddlePosition",
                _bigioTestEngine.GetResult(new TestArguments("InsertInMiddlePosition", CallFlag.ClearTestList, new[] { THOUSAND, MILLION / 100, MILLION / 10 })),
                _wintellectTestEngine.GetResult(new TestArguments("InsertInMiddlePosition", CallFlag.ClearTestList, new[] { THOUSAND, MILLION / 100, MILLION / 10 })),
                _listTestEngine.GetResult(new TestArguments("InsertInMiddlePosition", CallFlag.ClearTestList, new[] { THOUSAND, MILLION / 100, MILLION / 10 })));
        }

        public static void TestInsertInRandomPosition()
        {
            WriteResult("InsertInRandomPosition",
                _bigioTestEngine.GetResult(new TestArguments("InsertInRandomPosition", CallFlag.ClearTestList, new[] { THOUSAND, MILLION / 100, MILLION / 10, MILLION })),
                _wintellectTestEngine.GetResult(new TestArguments("InsertInRandomPosition", CallFlag.ClearTestList, new[] { THOUSAND, MILLION / 100, MILLION / 10, MILLION })),
                _listTestEngine.GetResult(new TestArguments("InsertInRandomPosition", CallFlag.ClearTestList, new[] { THOUSAND, MILLION / 100, MILLION / 10 })));
        }

        public static void TestInsertRangeInRandom()
        {
            WriteResult("InsertRangeInRandom",
                _bigioTestEngine.GetResult(new TestArguments("InsertRangeInRandom", CallFlag.ClearTestList, new[] { THOUSAND, MILLION / 100, MILLION / 10 })),
                _wintellectTestEngine.GetResult(new TestArguments("InsertRangeInRandom", CallFlag.ClearTestList, new[] { THOUSAND, MILLION / 100, MILLION / 10 })),
                _listTestEngine.GetResult(new TestArguments("InsertRangeInRandom", CallFlag.ClearTestList, new[] { THOUSAND, MILLION / 100 })));
        }

        public static void TestFor()
        {
            TestArguments arg = new TestArguments("For", CallFlag.FillTestList, new[] { 1, 4 });
            WriteResult("For", _bigioTestEngine.GetResult(arg), _wintellectTestEngine.GetResult(arg), _listTestEngine.GetResult(arg));
        }

        public static void TestForeach()
        {
            TestArguments arg = new TestArguments("Foreach", CallFlag.FillTestList, new[] { 1, 10, 30 });
            WriteResult("Foreach", _bigioTestEngine.GetResult(arg), _wintellectTestEngine.GetResult(arg), _listTestEngine.GetResult(arg));
        }

        public static void TestIndexOf()
        {
            TestArguments arg = new TestArguments("IndexOf", CallFlag.FillTestList, new[] { HUNDRED, THOUSAND, THOUSAND * 10 });
            WriteResult("IndexOf", _bigioTestEngine.GetResult(arg), _wintellectTestEngine.GetResult(arg), _listTestEngine.GetResult(arg));
        }

        public static void TestBinarySearch()
        {
            TestArguments arg = new TestArguments("BinarySearch", CallFlag.FillTestList, new[] { THOUSAND, THOUSAND * 10, THOUSAND * 100 });
            WriteResult("BinarySearch", _bigioTestEngine.GetResult(arg), _wintellectTestEngine.GetResult(arg), _listTestEngine.GetResult(arg));
        }

        public static void TestFindAll()
        {
            TestArguments arg = new TestArguments("FindAll", CallFlag.FillTestList, new[] { 2, 5, 7 });
            WriteResult("FindAll", _bigioTestEngine.GetResult(arg), _wintellectTestEngine.GetResult(arg), _listTestEngine.GetResult(arg));
        }

        public static void TestReverse()
        {
            WriteResult("Reverse",
                _bigioTestEngine.GetResult(new TestArguments("Reverse", CallFlag.FillTestList, new[] { 1, 5, 10, 15 })),
                _wintellectTestEngine.GetResult(new TestArguments("Reverse", CallFlag.FillTestList, new[] { 1 })),
                _listTestEngine.GetResult(new TestArguments("Reverse", CallFlag.FillTestList, new[] { 1, 5, 10, 15 })));
        }

        private static void WriteResult(string methodName, List<TestResult> bigioResult, List<TestResult> wintellectResult,
            List<TestResult> listResult)
        {
            Console.WriteLine("_____________{0}_____________", methodName);
            Console.WriteLine("Count \t \t Bigio \t \t Wintellect \t \t Microsoft List");

            for (int i = 0; i < bigioResult.Count; i++)
            {
                var count = bigioResult[i].CountOfObjects;

                string bigioStr = "-",
                    wintellectStr = "-",
                    listStr = "-";

                bigioStr = bigioResult[i].ElapsedMilliseconds.ToString();

                if (i < wintellectResult.Count)
                    wintellectStr = wintellectResult[i].ElapsedMilliseconds.ToString();

                if (i < listResult.Count)
                    listStr = listResult[i].ElapsedMilliseconds.ToString();

                Console.WriteLine("{0,10} \t {1,10} \t {2,10} \t {3,10}", count, bigioStr, wintellectStr, listStr);
            }
        }
    }
}
