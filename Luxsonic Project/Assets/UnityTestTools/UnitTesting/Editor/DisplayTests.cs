using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

    [TestFixture]
    [Category("Unit Tests")]
    public class DisplayTests
    {
        [Test]
        public void TestConstructor()
        {
            Texture2D tex = new Texture2D(100, 100);    // Create a texture to give to the display

            //Display disp = new Display(tex);    // Create a new display

            //Assert.IsNotNull(disp.imageRenderer);   // Make sure the image renderer was set properly
            //Assert.IsNotNull(disp.myTransform);     // As well as the transform object
        }
    }
   

