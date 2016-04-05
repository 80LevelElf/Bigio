using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using PerformanceTests.Configs;

namespace PerformanceTests.TemplateTests
{
    [Config(typeof(StandardConfig))]
    public class InsertAndInsertRangeTest<TList> : AbstractTest<TList> where TList : IList<int>
    {
        public InsertAndInsertRangeTest(TList list) : base(list)
        {
        }

        [Benchmark]
        public void Insert()
        {
            _list.Insert(_list.Count / 2, 0);
        }

        protected override void Setup()
        {
            //No elements to add
        }
    }
}
