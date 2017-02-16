using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;

namespace UnityTest
{
	[TestFixture]
	[Category("Unit Tests")]
	internal class ContrastSliderTests
	{
		[Test]
		public void PassingTest()
		{

			// TDD: Test not full finished / Scripts not fully developed yet

//			GameObject image = new GameObject();
//			Material mat = Resources.Load ("x-ray-skull-from-right-side") as Material;
//			image.AddComponent<MeshRenderer>();
//			MeshRenderer ren = image.GetComponent<MeshRenderer>();
//			ren.material = mat;
//
//			Material material = image.GetComponent<MeshRenderer>().material;
//			float contrast = material.GetFloat ("Contrast");
//
//			image.AddComponent<ContrastSlider>();
//			ContrastSlider slider = image.GetComponent<ContrastSlider> ();
//
//			slider.Contrast = 1.0F;
//
//			slider.updateImage();
//
//			Assert.AreEqual (slider.Contrast, material.GetFloat("Contrast"));
//			Assert.AreNotEqual (contrast, image.transform.localScale.x);

			Assert.Pass();
		}
	}
}
