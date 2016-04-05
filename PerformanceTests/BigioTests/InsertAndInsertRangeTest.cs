using Bigio;

namespace PerformanceTests.BigioTests
{
    public class InsertAndInsertRangeTest : TemplateTests.InsertAndInsertRangeTest<BigArray<int>>
    {
        public InsertAndInsertRangeTest() : base(new BigArray<int>())
        {
        }
    }
}
