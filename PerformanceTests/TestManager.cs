using System;
using System.Collections.Generic;
using Bigio;
using Wintellect.PowerCollections;

namespace PerformanceTests
{
    public static class TestManager
    {
        private const int THOUSAND = 1000;
        private const int MILLION = THOUSAND * THOUSAND;
        private const int BILLIARD = MILLION * THOUSAND;

        private static readonly TestEngine<BigArray<int>> _bigioTestEngine = new TestEngine<BigArray<int>>();
        private static readonly TestEngine<BigList<int>> _wintellectTestEngine = new TestEngine<BigList<int>>();
        private static readonly TestEngine<List<int>> _listTestEngine = new TestEngine<List<int>>();

        public static void TestAdd()
        {
            TestArguments arg = new TestArguments("Add", new[] { MILLION / 10, MILLION, MILLION * 10, MILLION * 100});
            WriteResult("Add", _bigioTestEngine.GetResult(arg), _wintellectTestEngine.GetResult(arg), _listTestEngine.GetResult(arg));
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
