using Bigio.Common.Managers;
using NUnit.Framework;

namespace UnitTests.Bigio_Tests.Common_Tests.Managers_Tests
{
    [TestFixture]
    public static class ValidationManagerTest
    {
        [Test]
        public static void Test()
        {
            var testArray = new int[32];

            //Tets count validation
            Assert.IsTrue(testArray.IsValidCount(0));
            Assert.IsTrue(testArray.IsValidCount(32));
            Assert.IsTrue(ValidationManager.IsValidCount(0, 0));

            Assert.IsFalse(testArray.IsValidCount(-1));
            Assert.IsFalse(testArray.IsValidCount(33));

            //Test index validation
            Assert.IsTrue(testArray.IsValidIndex(0));
            Assert.IsTrue(testArray.IsValidIndex(31));

            Assert.IsFalse(testArray.IsValidIndex(-1));
            Assert.IsFalse(testArray.IsValidIndex(32));
            Assert.IsFalse(ValidationManager.IsValidIndex(0, 0));

            //Range validation
            Assert.IsTrue(testArray.IsValidRange(1, 0));
            Assert.IsTrue(testArray.IsValidRange(1, 10));
            Assert.IsTrue(testArray.IsValidRange(0, 32));
            Assert.IsTrue(testArray.IsValidRange(31, 0));
            Assert.IsTrue(testArray.IsValidRange(0, 0));

            Assert.IsFalse(testArray.IsValidRange(-1, 0));
            Assert.IsFalse(testArray.IsValidRange(-1, 1));
            Assert.IsFalse(testArray.IsValidRange(30, 3));

            Assert.IsTrue(testArray.IsValidRange(32, 0)); // Read documentation
            Assert.IsTrue(ValidationManager.IsValidRange(0, 0, 0)); // Read documentation
        }
    }
}
