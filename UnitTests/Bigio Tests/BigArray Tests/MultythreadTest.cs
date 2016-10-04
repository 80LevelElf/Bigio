using System;
using System.Threading.Tasks;
using Bigio;
using NUnit.Framework;

namespace UnitTests.Bigio_Tests.BigArray_Tests
{
	[TestFixture]
	public class MultythreadTest
	{
		private const int TEST_SIZE = 16384;

		[Test]
		public void IndexOf()
		{
			ForAllItemsMultythread((array, item) =>
			{
				Assert.AreEqual(item, array.IndexOf(item));	
			});
		}

		[Test]
		public void LastIndexOf()
		{
			ForAllItemsMultythread((array, item) =>
			{
				Assert.AreEqual(item, array.LastIndexOf(item));
			});
		}

		[Test]
		public void Find()
		{
			ForAllItemsMultythread((array, item) =>
			{
				Assert.AreEqual(item, array.Find(i => i == item));
			});
		}

		[Test]
		public void FindIndex()
		{
			ForAllItemsMultythread((array, item) =>
			{
				Assert.AreEqual(item, array.FindIndex(i => i == item));
			});
		}

		[Test]
		public void FindLastIndex()
		{
			ForAllItemsMultythread((array, item) =>
			{
				Assert.AreEqual(item, array.FindLastIndex(i => i == item));
			});
		}

		private void ForAllItemsMultythread(Action<BigArray<int>, int> checkAction)
		{
			var array = GetTestInstance();
			Parallel.ForEach(array, item =>
			{
				checkAction(array, item);
			});
		}

		private BigArray<int> GetTestInstance()
		{
			BigArray<int> array = new BigArray<int>();

			for (int i = 0; i < TEST_SIZE; i++)
			{
				array.Add(i);
			}

			return array;
		}
	}
}
