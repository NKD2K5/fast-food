using NUnit.Framework;
using System;
using System.Linq;

namespace UnitTestProject
{
    [TestFixture]
    public class ArrayUtilsTests
    {
        [Test]
        public void Test_EmptyArray()
        {
            int[] arr = { };
            Assert.AreEqual(0, ArrayUtils.SumArray(arr));
        }

        [Test]
        public void Test_SinglePositiveElement()
        {
            int[] arr = { 5 };
            Assert.AreEqual(5, ArrayUtils.SumArray(arr));
        }

        [Test]
        public void Test_SingleNegativeElement()
        {
            int[] arr = { -3 };
            Assert.AreEqual(-3, ArrayUtils.SumArray(arr));
        }

        [Test]
        public void Test_MultiplePositiveElements()
        {
            int[] arr = { 1, 2, 3, 4, 5 };
            Assert.AreEqual(15, ArrayUtils.SumArray(arr));
        }

        [Test]
        public void Test_MultipleNegativeElements()
        {
            int[] arr = { -1, -2, -3, -4, -5 };
            Assert.AreEqual(-15, ArrayUtils.SumArray(arr));
        }

        [Test]
        public void Test_MixedElements()
        {
            int[] arr = { -3, 7, -2, 8, -1 };
            Assert.AreEqual(9, ArrayUtils.SumArray(arr));
        }

        [Test]
        public void Test_ArrayWithZero()
        {
            int[] arr = { 0, 1, 2, 3, 4, 5 };
            Assert.AreEqual(15, ArrayUtils.SumArray(arr));
        }

        [Test]
        public void Test_LargeArray()
        {
            int[] arr = Enumerable.Repeat(1,1000000).ToArray();
            Assert.AreEqual(1000000, ArrayUtils.SumArray(arr));
        }

        [Test]
        public void Test_NullArray()
        {
            Assert.Throws<ArgumentException>(() => ArrayUtils.SumArray(null));
        }
    }
}
