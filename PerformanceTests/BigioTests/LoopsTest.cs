using Bigio;

namespace PerformanceTests.BigioTests
{
    public class LoopsTest : TemplateTests.LoopsTest<BigArray<int>>
    {
        public LoopsTest(BigArray<int> list) : base(list)
        {
        }
    }
}
