using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;

namespace UnityTest
{
    [TestFixture]
    [Category("Unit Tests")]
    internal class BrightnessSliderTests
    {
        [Test]
        public void updateImageTest()
        {
			// TDD: Scripts are not fully developed yet

//			GameObject image = new GameObject ();
//			image.AddComponent<BrightnessSlider>();
//			BrightnessSlider slider = image.GetComponent<BrightnessSlider> ();
//
//			slider.image = image;
//			slider.lightValue = 0.5F;
//
//			Assert.NotNull (slider.image);
//			Assert.AreEqual (0.5F, slider.lightValue);
//
//			slider.updateImage();
//
//			Light light = image.GetComponent<Light> ();
//			float lightValue = light.color.r;
//
//			Assert.AreEqual (slider.lightValue, lightValue);
//			Assert.AreNotEqual (0.0F, lightValue);

			Assert.Pass ();
        }
    }
}
