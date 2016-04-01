using System;
using BenchmarkDotNet.Attributes;
using Bigio;
using PerformanceTests.Configs;

namespace PerformanceTests.BigArray
{
    [Config(typeof(StandardConfig))]
    public class TemplateTest_JustToCheckSmth
    {
        private readonly BigArray<int> _bigArrayToAdd = new BigArray<int>();
        private readonly Random _random = new Random();
        private const int MAX = 100000000;

        public TemplateTest_JustToCheckSmth()
        {
            for (int i = 0; i < 10000000; i++)
            {
                _bigArrayToAdd.Add(_random.Next());
            }
        }

        [Benchmark]
        public void Bigio_Exists()
        {
            _bigArrayToAdd.Exists(i => i == _random.Next());
        }

        [Benchmark]
        public void Bigio_ExistsMltThr()
        {
            _bigArrayToAdd.Exists_Parallel(i => i == _random.Next());
        }
    }
}
