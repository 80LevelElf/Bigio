using System.Collections.Generic;

namespace PerformanceTests.СomparativeTests
{
	public class IntTestEngine<TList> : TestEngine<TList, int> where TList : IList<int>
	{
		private readonly List<int> _sampleList =  new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };

		protected override int GetValue(int i)
		{
			return i;
		}

		protected override List<int> GetSampleList()
		{
			return _sampleList;
		}
	}
}
