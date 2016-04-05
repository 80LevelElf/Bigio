using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using PerformanceTests.Configs;

namespace PerformanceTests.TemplateTests
{
    [Config(typeof(StandardConfig))]
    public class LoopsTest<TList> : AbstractTest<TList> where TList : IList<int>
    {
        public LoopsTest(TList list) : base(list)
        {
        }

        [Benchmark]
        public void For()
        {
            for (int i = 0; i < DEFAULT_MAX_COUNT; i++)
            {
                int temp = _list[i];
            }
        }

        [Benchmark]
        public void Foreach()
        {
            foreach (var item in _list)
            {
                int temp = item;
            }
        }

    }
}
