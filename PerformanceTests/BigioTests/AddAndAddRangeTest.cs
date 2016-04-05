using BenchmarkDotNet.Attributes;
using Bigio;
using PerformanceTests.Configs;

namespace PerformanceTests.BigioTests
{
    [Config(typeof(StandardConfig))]
    public class AddAndAddRangeTest : TemplateTests.AddAndAddRangeTest<BigArray<int>>
    {
        private readonly int[] _rangeToAdd = {1, 2, 3};

        public AddAndAddRangeTest() : base(new BigArray<int>())
        {
        }

        [Benchmark]
        public void AddRange()
        {
            _list.AddRange(_rangeToAdd);
        }
    }
}
