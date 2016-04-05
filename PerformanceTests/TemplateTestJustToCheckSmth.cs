using System;
using BenchmarkDotNet.Attributes;
using Bigio;
using PerformanceTests.Configs;

namespace PerformanceTests
{
    [Config(typeof(StandardConfig))]
    public class TemplateTestJustToCheckSmth
    {
        private readonly BigArray<int> _bigArrayToAdd = new BigArray<int>();
        private readonly Random _random = new Random();
        private const int MAX = 10000000;

        public TemplateTestJustToCheckSmth()
        {
            for (int i = 0; i < MAX; i++)
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
            //_bigArrayToAdd.Exists_Parallel(i => i == _random.Next());
        }
    }
}
