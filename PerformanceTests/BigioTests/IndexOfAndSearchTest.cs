using BenchmarkDotNet.Attributes;
using Bigio;
using PerformanceTests.Configs;

namespace PerformanceTests.BigioTests
{
    [Config(typeof(StandardConfig))]
    public class IndexOfAndSearchTest : TemplateTests.InsertAndInsertRangeTest<BigArray<int>>
    {
        public IndexOfAndSearchTest() : base(new BigArray<int>())
        {
        }

        [Benchmark]
        public void LastIndexOf()
        {
            _list.LastIndexOf(GetRandomIndex());
        }

        [Benchmark]
        public void Exists()
        {
            _list.Exists(i => i == GetRandomIndex());
        }

        [Benchmark]
        public void FindIndex()
        {
            _list.FindIndex(i => i == GetRandomIndex());
        }

        [Benchmark]
        public void FindLastIndex()
        {
            _list.FindLastIndex(i => i == GetRandomIndex());
        }

        [Benchmark]
        public void FindAll()
        {
            var divider = GetRandomIndex();
            _list.FindAll(i => i % divider == 0);
        }
    }
}
