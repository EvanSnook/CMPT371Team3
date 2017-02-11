using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;

    [TestFixture]
    [Category("Sample Tests")]
    internal class SampleTests
    {
        [Test]
        public void PassingTest()
        {
            BrightnessSlider bs = new BrightnessSlider();
            Assert.Pass();
        }
    }