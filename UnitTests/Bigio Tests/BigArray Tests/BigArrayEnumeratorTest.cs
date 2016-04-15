using System;
using Bigio;
using NUnit.Framework;

namespace UnitTests.Bigio_Tests.BigArray_Tests
{
    [TestFixture]
    public class BigArrayEnumeratorTest
    {
        [Test]
        public void Test()
        {
            var array = new BigArray<int>();
            int count = (int) Math.Pow(10, 7);

            for (int i = 0; i < count; i++)
            {
                array.Add(i);
            }

            //Checking
            int startIndex = (int)Math.Pow(10, 4); 

            BigArray<int>.BigArrayEnumerator enumerator = (BigArray<int>.BigArrayEnumerator) array.GetEnumerator();
            enumerator.MoveToIndex(startIndex);

            for (int i = startIndex; i < count; i++)
            {
                Assert.AreEqual(enumerator.Current, array[i]);
                enumerator.MoveNext();
            }
        }
    }
}
