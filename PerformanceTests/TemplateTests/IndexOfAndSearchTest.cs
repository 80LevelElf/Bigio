using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using PerformanceTests.Configs;

namespace PerformanceTests.TemplateTests
{
    [Config(typeof(StandardConfig))]
    public class IndexOfAndSearchTest<TList> : AbstractTest<TList> where TList : IList<int>
    {
        public IndexOfAndSearchTest(TList list) : base(list)
        {
        }

        [Benchmark]
        public void IndexOf()
        {
            _list.IndexOf(GetRandomIndex());
        }

        public void Contains()
        {
            _list.Contains(GetRandomIndex());
        }

        /**/

        private int GetRandomIndex()
        {
            return _random.Next(DEFAULT_MAX_COUNT/10);
        }
    }
}
