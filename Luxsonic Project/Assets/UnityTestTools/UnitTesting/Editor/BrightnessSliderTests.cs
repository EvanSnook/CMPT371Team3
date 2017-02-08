using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;

namespace UnityTest
{
    [TestFixture]
    [Category("Unit Tests")]
    internal class SampleTests
    {
        [Test]
        public void PassingTest()
        {
            Assert.Pass();
        }

		[Test]
		public void FailingTest()
		{
			Assert.Fail();
		}
    }
}
