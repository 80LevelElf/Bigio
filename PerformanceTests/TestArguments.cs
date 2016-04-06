namespace PerformanceTests
{
    public class TestArguments
    {
        public TestArguments(string methodName, int[] testCountArray)
        {
            MethodName = methodName;
            TestCountArray = testCountArray;
        }

        public string MethodName { get; private set; }
        public int[] TestCountArray { get; private set; }
    }
}
