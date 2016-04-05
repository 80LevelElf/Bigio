using System;
using System.Collections.Generic;

namespace PerformanceTests.TemplateTests
{
    public abstract class AbstractTest<TList> where TList : IList<int>
    {
        protected readonly TList _list;
        protected readonly Random _random;
        protected const int DEFAULT_MAX_COUNT = 1000000;

        protected AbstractTest(TList list)
        {
            _random = new Random();
            _list = list;
        }

        protected virtual void Setup()
        {
            for (int i = 0; i < DEFAULT_MAX_COUNT; i++)
            {
                _list.Add(_random.Next());
            }
        }

        protected int GetRandomIndex()
        {
            return _random.Next(DEFAULT_MAX_COUNT);
        }
    }
}
