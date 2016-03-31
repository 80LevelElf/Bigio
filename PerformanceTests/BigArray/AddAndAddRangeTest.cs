using BenchmarkDotNet.Attributes;
using Bigio;
using PerformanceTests.Configs;

namespace PerformanceTests.BigArray
{
    [Config(typeof(StandardConfig))]
    public class AddAndAddRangeTest
    {
        private readonly BigArray<int> _bigArrayToAdd = new BigArray<int>();
        private readonly BigArray<int> _bigArrayToAddRange = new BigArray<int>();
        private readonly int[] _rangeToAdd = {1, 2, 3};

        [Benchmark]
        public void Bigio_Add()
        {
            _bigArrayToAdd.Add(0);
        }

        [Benchmark]
        public void Bigio_AddRange()
        {
            _bigArrayToAddRange.AddRange(_rangeToAdd);
        }
    }
}
