using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Bigio.BigArray.Support_Classes.ArrayMap;
using Bigio.BigArray.Support_Classes.Balancer;
using Bigio.BigArray.Support_Classes.BlockCollection;

namespace PerformanceTests.EngineMeasuringTest
{
    public static class ArrayMapTests
    {
        private const int HUNDRED = 100;
        private const int THOUSAND = 1000;
        private const int MILLION = THOUSAND * THOUSAND;
        private const int BILLIARD = MILLION * THOUSAND;

        private const int BlockCount = 1000;
        private static readonly int ElementsInBlockCount = 1024;
        private static readonly Random _random = new Random();
        private static object _writeResultLocker = new object();

        private static readonly string _logFilePath;

        static ArrayMapTests()
        {
            if (!Directory.Exists("logs"))
                Directory.CreateDirectory("logs");

            _logFilePath = "logs/СBlockStructureTest by " + DateTime.Now.ToString("yy-MM-dd-mm-ss") + ".txt";
        }

        public static void TestBinarySearch()
        {
            WriteResult("BinarySearch", GetResult(new TestArguments("BinarySearch", new[] { THOUSAND, THOUSAND * 100, MILLION, MILLION * 10})));
        }

        public static void TestLinearSearch()
        {
            WriteResult("LinearSearch", GetResult(new TestArguments("LinearSearch", new[] { THOUSAND, THOUSAND * 100 })));
        }

        private static Block<int> GetFilledBlock(int size)
        {
            var block = new Block<int>(ElementsInBlockCount);
            for (int i = 0; i < size; i++)
            {
                block.Add(i);
            }

            return block;
        }

        private static IEnumerable<TestResult> GetResult(TestArguments arguments)
        {
            //Prepare data
            var blockCollection = new BlockCollection<int>();
            for (int i = 0; i < BlockCount; i++)
            {
                blockCollection.Add(GetFilledBlock(ElementsInBlockCount));
            }

            var blockStructure = new ArrayMap<int>(new FixedBalancer(),  blockCollection);

            //Prepare measure engine
            var method = GetMethodInfo(arguments.MethodName);

            foreach (var currentCount in arguments.TestCountArray)
            {
                List<object> argumentList = new List<object> { blockStructure, currentCount};
                
                //Get middle estimation of several times calling
                long timeOfAllInvoketionsMs = 0;
                int countOfInvokations = 3;

                for (int i = 0; i < countOfInvokations; i++)
                {
                    timeOfAllInvoketionsMs += MeasureEngine.MeasureStaticMethod(method, argumentList);
                }

                yield return new TestResult(currentCount, timeOfAllInvoketionsMs / countOfInvokations);
            }
        }

        private static void BinarySearch(ArrayMap<int> arrayMap, int count)
        {
            for (int i = 0; i < count; i++)
            {
                arrayMap.BlockInfo(_random.Next(BlockCount*ElementsInBlockCount));
            }
        }

        private static void LinearSearch(ArrayMap<int> arrayMap, int count)
        {
            //To prevent using BinarySearch
            arrayMap.DataChanged(0);

            for (int i = 0; i < count; i++)
            {
                arrayMap.BlockInfo(_random.Next(BlockCount * ElementsInBlockCount));
            }
        }

        private static MethodInfo GetMethodInfo(string methodName)
        {
            Type thisType = typeof (ArrayMapTests);
            return thisType.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);
        }

        private static void WriteResult(string methodName, IEnumerable<TestResult> resultList)
        {
            lock (_writeResultLocker)
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine(string.Format("_____________{0}_____________", methodName));
                stringBuilder.AppendLine("Count \t \t Estimation");

                foreach (var resultItem in resultList)
                {
                    stringBuilder.AppendLine(string.Format("{0} \t \t {1}", resultItem.CountOfObjects, resultItem.ElapsedMilliseconds));
                }

                Console.WriteLine(stringBuilder);

                //Log in file
                using (var writer = new StreamWriter(_logFilePath, true))
                {
                    writer.Write(stringBuilder);
                }
            }
        }
    }
}
