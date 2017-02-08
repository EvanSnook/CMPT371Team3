using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;

namespace UnityTest
{
    [TestFixture]
    [Category("Sample Tests")]
    internal class SampleTests
    {
        /*
        [Test]
        [Category("Failing Tests")]
        public void FailingTest()
        {
            Assert.Fail();
        }
        */

        [Test]
        public void PassingTest()
        {
            Assert.Pass();
        }
    }
}