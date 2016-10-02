using System.Collections.Generic;

namespace PerformanceTests.СomparativeTests
{
	public class StringTestEngine<TList> : TestEngine<TList, string> where TList : IList<string>
	{
		private readonly List<string> _sampleList = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9" };

		protected override string GetValue(int i)
		{
			return i.ToString();
		}

		protected override List<string> GetSampleList()
		{
			return _sampleList;
		}
	}
}
