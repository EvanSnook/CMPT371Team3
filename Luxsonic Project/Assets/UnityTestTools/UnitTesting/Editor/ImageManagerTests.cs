using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

[TestFixture]
[Category("Unit Tests")]
public class ImageManagerTests{

    [Test]
    public void TestAddImage()
    {
        Texture2D tex = new Texture2D(100, 100);
        ImageManager manny = new ImageManager();
        manny.AddImage(tex);

    }

    [Test]
    public void TestCreateTray()
    {

    }

    [Test]
    public void TestCreateDisplay()
    {

    }
}
