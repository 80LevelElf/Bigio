using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Bigio;
using Wintellect.PowerCollections;

namespace PerformanceTests.СomparativeTests
{
    public static class TestManager<T>
    {
        private const int HUNDRED = 100;
        private const int THOUSAND = 1000;
        private const int MILLION = THOUSAND * THOUSAND;
        private const int BILLIARD = MILLION * THOUSAND;

        private static readonly string _logFilePath;

        static TestManager()
        {
            if (!Directory.Exists("logs"))
                Directory.CreateDirectory("logs");

            _logFilePath = "logs/СomparativeTests" + DateTime.Now.ToString("yy-MM-dd-mm-ss") + ".txt";
        }

        public static void TestAdd()
        {
            TestArguments arg = new TestArguments("Add", CallFlag.ClearTestCollection, new[] { MILLION / 100, MILLION / 10, MILLION, MILLION * 10, MILLION * 100 });
            WriteResult("Add", GetBigioEngine().GetResult(arg), GetWintellectEngine().GetResult(arg), GetListEngine().GetResult(arg));
        }

        public static void TestAddRange()
        {
            TestArguments arg = new TestArguments("AddRange", CallFlag.ClearTestCollection, new[] {THOUSAND, MILLION/100, MILLION/10, MILLION});
            WriteResult("AddRange", GetBigioEngine().GetResult(arg), GetWintellectEngine().GetResult(arg), GetListEngine().GetResult(arg));
        }

        public static void TestInsertInStartPosition()
        {
            WriteResult("InsertInStartPosition",
                GetBigioEngine().GetResult(new TestArguments("InsertInStartPosition", CallFlag.ClearTestCollection, new[] { HUNDRED, THOUSAND, MILLION / 100, MILLION / 10, MILLION, MILLION * 10 })),
                GetWintellectEngine().GetResult(new TestArguments("InsertInStartPosition", CallFlag.ClearTestCollection, new[] { HUNDRED, THOUSAND, MILLION / 100, MILLION / 10, MILLION, MILLION * 10 })),
                GetListEngine().GetResult(new TestArguments("InsertInStartPosition", CallFlag.ClearTestCollection, new[] { HUNDRED, THOUSAND, MILLION / 100, MILLION / 10 })));
        }

        public static void TestInsertInMiddlePosition()
        {
            WriteResult("InsertInMiddlePosition",
                GetBigioEngine().GetResult(new TestArguments("InsertInMiddlePosition", CallFlag.ClearTestCollection, new[] { HUNDRED, THOUSAND, MILLION / 100, MILLION / 10 })),
                GetWintellectEngine().GetResult(new TestArguments("InsertInMiddlePosition", CallFlag.ClearTestCollection, new[] { HUNDRED, THOUSAND, MILLION / 100, MILLION / 10 })),
                GetListEngine().GetResult(new TestArguments("InsertInMiddlePosition", CallFlag.ClearTestCollection, new[] { HUNDRED, THOUSAND, MILLION / 100, MILLION / 10 })));
        }

        public static void TestInsertInRandomPosition()
        {
            WriteResult("InsertInRandomPosition",
                GetBigioEngine().GetResult(new TestArguments("InsertInRandomPosition", CallFlag.ClearTestCollection, new[] { HUNDRED, THOUSAND, MILLION / 100, MILLION / 10, MILLION })),
                GetWintellectEngine().GetResult(new TestArguments("InsertInRandomPosition", CallFlag.ClearTestCollection, new[] { HUNDRED, THOUSAND, MILLION / 100, MILLION / 10, MILLION })),
                GetListEngine().GetResult(new TestArguments("InsertInRandomPosition", CallFlag.ClearTestCollection, new[] { HUNDRED, THOUSAND, MILLION / 100, MILLION / 10 })));
        }

        public static void TestInsertRangeInRandom()
        {
            WriteResult("InsertRangeInRandom",
                GetBigioEngine().GetResult(new TestArguments("InsertRangeInRandom", CallFlag.ClearTestCollection, new[] { HUNDRED, THOUSAND, MILLION / 100, MILLION / 10 })),
                GetWintellectEngine().GetResult(new TestArguments("InsertRangeInRandom", CallFlag.ClearTestCollection, new[] { HUNDRED, THOUSAND, MILLION / 100, MILLION / 10 })),
                GetListEngine().GetResult(new TestArguments("InsertRangeInRandom", CallFlag.ClearTestCollection, new[] { HUNDRED, THOUSAND, MILLION / 100 })));
        }

        public static void TestFor()
        {
            TestArguments arg = new TestArguments("For", CallFlag.FillTestCollection, new[] { 1, 4 });
            WriteResult("For", GetBigioEngine().GetResult(arg), GetWintellectEngine().GetResult(arg), GetListEngine().GetResult(arg));
        }

        public static void TestForeach()
        {
            TestArguments arg = new TestArguments("Foreach", CallFlag.FillTestCollection, new[] { 1, 5, 10 });
            WriteResult("Foreach", GetBigioEngine().GetResult(arg), GetWintellectEngine().GetResult(arg), GetListEngine().GetResult(arg));
        }

        public static void TestIndexOf()
        {
            TestArguments arg = new TestArguments("IndexOf", CallFlag.FillTestCollection, new[] { 10, HUNDRED, THOUSAND, THOUSAND * 10 });
            WriteResult("IndexOf", GetBigioEngine().GetResult(arg), GetWintellectEngine().GetResult(arg), GetListEngine().GetResult(arg));
        }

        public static void TestLastIndexOf()
        {
            TestArguments arg = new TestArguments("LastIndexOf", CallFlag.FillTestCollection, new[] { 1, 2, 5, 10 });
            WriteResult("LastIndexOf", GetBigioEngine().GetResult(arg), GetWintellectEngine().GetResult(arg), GetListEngine().GetResult(arg));
        }

        public static void TestBinarySearch()
        {
            TestArguments arg = new TestArguments("BinarySearch", CallFlag.FillTestCollection, new[] { HUNDRED, THOUSAND, THOUSAND * 10, THOUSAND * 100 });
            WriteResult("BinarySearch", GetBigioEngine().GetResult(arg), GetWintellectEngine().GetResult(arg), GetListEngine().GetResult(arg));
        }

        public static void TestFind()
        {
            TestArguments arg = new TestArguments("Find", CallFlag.FillTestCollection, new[] { 10, HUNDRED, THOUSAND, THOUSAND * 10 });
            WriteResult("Find", GetBigioEngine().GetResult(arg), GetWintellectEngine().GetResult(arg), GetListEngine().GetResult(arg));
        }

        public static void TestFindLast()
        {
            TestArguments arg = new TestArguments("FindLast", CallFlag.FillTestCollection, new[] { 1, 2, 5, 10 });
            WriteResult("FindLast", GetBigioEngine().GetResult(arg), GetWintellectEngine().GetResult(arg), GetListEngine().GetResult(arg));
        }

        public static void TestFindAll()
        {
            TestArguments arg = new TestArguments("FindAll", CallFlag.FillTestCollection, new[] { 2, 5, 7 });
            WriteResult("FindAll", GetBigioEngine().GetResult(arg), GetWintellectEngine().GetResult(arg), GetListEngine().GetResult(arg));
        }

        public static void TestReverse()
        {
            WriteResult("Reverse",
                GetBigioEngine().GetResult(new TestArguments("Reverse", CallFlag.FillTestCollection, new[] { 1, 5, 10, 15 })),
                GetWintellectEngine().GetResult(new TestArguments("Reverse", CallFlag.FillTestCollection, new[] { 1 })),
                GetListEngine().GetResult(new TestArguments("Reverse", CallFlag.FillTestCollection, new[] { 1, 5, 10, 15 })));
        }

        private static readonly object _writeResultLocker = new object();

        private static void WriteResult(string methodName, List<TestResult> bigioResult, List<TestResult> wintellectResult,
            List<TestResult> listResult)
        {
            lock (_writeResultLocker)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine(string.Format("_____________{0}_____________", methodName));
                stringBuilder.AppendLine("Count \t \t Bigio \t \t Wintellect \t \t Microsoft List");

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

                    stringBuilder.AppendLine(string.Format("{0,10} \t {1,10} \t {2,10} \t {3,10}", count, bigioStr, wintellectStr, listStr));
                }

                Console.WriteLine(stringBuilder);

                //Log in file
                using (var writer = new StreamWriter(_logFilePath, true))
                {
                    writer.Write(stringBuilder);
                }
            }
        }

        private static TestEngine<BigArray<T>, T> GetBigioEngine()
        {
	        if (typeof (T) == typeof (int))
				return new IntTestEngine<BigArray<int>>() as TestEngine<BigArray<T>, T>;

			if (typeof (T) == typeof (string))
				return new StringTestEngine<BigArray<string>>() as TestEngine<BigArray<T>, T>;

			throw new InvalidOperationException("Unknown TestEngine type!");
        }

        private static TestEngine<BigList<T>, T> GetWintellectEngine()
        {
			if (typeof(T) == typeof(int))
				return new IntTestEngine<BigList<int>>() as TestEngine<BigList<T>, T>;

			if (typeof(T) == typeof(string))
				return new StringTestEngine<BigList<string>>() as TestEngine<BigList<T>, T>;

			throw new InvalidOperationException("Unknown TestEngine type!");
		}

        private static TestEngine<List<T>, T> GetListEngine()
        {
			if (typeof(T) == typeof(int))
				return new IntTestEngine<List<int>>() as TestEngine<List<T>, T>;

			if (typeof(T) == typeof(string))
				return new StringTestEngine<List<string>>() as TestEngine<List<T>, T>;

			throw new InvalidOperationException("Unknown TestEngine type!");
		}
    }
}
