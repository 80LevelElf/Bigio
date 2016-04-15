namespace PerformanceTests.СomparativeTests
{
    public class TestArguments
    {
        public TestArguments(string methodName, CallFlag flag, int[] testCountArray)
        {
            MethodName = methodName;
            TestCountArray = testCountArray;
            CallFlag = flag;
        }

        public string MethodName { get; private set; }
        public int[] TestCountArray { get; private set; }
        public CallFlag CallFlag { get; private set; }
    }
}
