namespace PerformanceTests
{
    public class TestResult
    {
        public TestResult(int countOfObjects, long elapsedMilliseconds)
        {
            CountOfObjects = countOfObjects;
            ElapsedMilliseconds = elapsedMilliseconds;
        }

        public int CountOfObjects { get; private set; }
        public long ElapsedMilliseconds { get; private set; }
    }
}
