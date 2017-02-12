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

        // The manager should have a non-empty list and should contain the texture we created.
        Assert.Greater(manny.GetImages().Count, 0, "The list of images in the Image Manager is empty.");
        Assert.True(manny.GetImages().Contains(tex), "The list of images in the Image Manager does not contain the requested texture.");
    }

    [Test]
    public void TestCreateTray()
    {
        ImageManager manny = new ImageManager();
        manny.CreateTray();

        // The list of thumbnails should not be empty.
        Assert.Greater(manny.GetThumbnails().Count, 0, "The list of thumbnails in the Image Manager is empty.");
    }

    [Test]
    public void TestCreateDisplay()
    {
        ImageManager manny = new ImageManager();
        Texture2D tex = new Texture2D(100, 100);
        manny.CreateDisplay(tex);

        // The list of displays should not be empty
        Assert.Greater(manny.GetDisplays().Count, 0, "The list of displays in the Image Manager is empty.");
    }
}
