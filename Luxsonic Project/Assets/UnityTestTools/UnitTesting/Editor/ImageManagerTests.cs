using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using NSubstitute;

[TestFixture]
[Category("Unit Tests")]
public class ImageManagerTests{

    [Test]
    public void TestAddImage()
    {
        GameObject mannyObj = new GameObject();
        mannyObj.AddComponent<ImageManager>();
        ImageManager manny = mannyObj.GetComponent<ImageManager>();
        Texture2D tex = new Texture2D(100, 100);

        GameObject thumbObject = new GameObject();
        Thumbnail thumb = Substitute.For<Thumbnail>();
        thumbObject.AddComponent<Thumbnail>();
        thumbObject.AddComponent<SpriteRenderer>();


        manny.thumbnail = thumbObject;

        manny.AddImage(tex);

        // The manager should have a non-empty list and should contain the texture we created.
        Assert.Greater(manny.GetImages().Count, 0, "The list of images in the Image Manager is empty.");
        Assert.True(manny.GetImages().Contains(tex), "The list of images in the Image Manager does not contain the requested texture.");
    }

    [Test]
    public void TestCreateTray()
    {
        GameObject mannyObj = new GameObject();
        mannyObj.AddComponent<ImageManager>();
        ImageManager manny = mannyObj.GetComponent<ImageManager>();
        

        GameObject thumbObject = new GameObject();
        Thumbnail thumb = Substitute.For<Thumbnail>();
        thumbObject.AddComponent<Thumbnail>();
        thumbObject.AddComponent<SpriteRenderer>();
        // 
        //GameObject thumbObject = Substitute.For<GameObject>();
        //thumbObject.AddComponent<Thumbnail>();

        Texture2D tex = new Texture2D(100, 100);

        manny.thumbnail = thumbObject;

        Assert.IsNotNull(manny.GetComponent<ImageManager>().thumbnail, "The thumbnail object for the image manager is NULL");
        manny.AddImage(tex);
        Assert.Greater(manny.GetImages().Count, 0, "The list of images in the Image Manager is empty.");

        manny.CreateTray();

        // The list of thumbnails should not be empty.
        Assert.Greater(manny.GetThumbnails().Count, 0, "The list of thumbnails in the Image Manager is empty.");
    }

    [Test]
    public void TestCreateDisplay()
    {
        GameObject mannyObj = new GameObject();
        mannyObj.AddComponent<ImageManager>();
        ImageManager manny = mannyObj.GetComponent<ImageManager>();


        GameObject thumbObject = new GameObject();
        Thumbnail thumb = Substitute.For<Thumbnail>();
        thumbObject.AddComponent<Thumbnail>();
        thumbObject.AddComponent<SpriteRenderer>();

        GameObject dispObject = new GameObject();
        Display disp = Substitute.For<Display>();
        dispObject.AddComponent<Display>();
        dispObject.AddComponent<SpriteRenderer>();
        dispObject.AddComponent<BoxCollider>();
        // 
        //GameObject thumbObject = Substitute.For<GameObject>();
        //thumbObject.AddComponent<Thumbnail>();

        Texture2D tex = new Texture2D(100, 100);

        manny.thumbnail = thumbObject;
        manny.displayObj = dispObject;

        manny.CreateDisplay(tex);

        // The list of displays should not be empty
        Assert.Greater(manny.GetDisplays().Count, 0, "The list of displays in the Image Manager is empty.");
    }
}
