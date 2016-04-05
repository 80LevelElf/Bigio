using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using PerformanceTests.Configs;

namespace PerformanceTests.TemplateTests
{
    [Config(typeof(StandardConfig))]
    public class AddAndAddRangeTest<TList> : AbstractTest<TList> where TList : IList<int>
    {
        protected AddAndAddRangeTest(TList list) : base(list)
        {
        }

        [Benchmark]
        public void Add()
        {
            _list.Add(0);
        }

        protected override void Setup()
        {
            //No elements to add
        }
    }
}
