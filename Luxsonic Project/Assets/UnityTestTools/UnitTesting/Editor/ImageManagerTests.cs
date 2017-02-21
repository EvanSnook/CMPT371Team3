﻿using System.Collections;
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

        GameObject trayObject = new GameObject();
        trayObject.AddComponent<Tray>();
        


        GameObject thumbObject = new GameObject();
        thumbObject.AddComponent<Thumbnail>();
        thumbObject.AddComponent<SpriteRenderer>();

        trayObject.GetComponent<Tray>().thumbnail = thumbObject;
        manny.trayObj = trayObject;

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
        Texture2D tex = new Texture2D(100, 100);

        GameObject trayObject = new GameObject();
        trayObject.AddComponent<Tray>();



        GameObject thumbObject = new GameObject();
        thumbObject.AddComponent<Thumbnail>();
        thumbObject.AddComponent<SpriteRenderer>();

        trayObject.GetComponent<Tray>().thumbnail = thumbObject;
        manny.trayObj = trayObject;

        manny.CreateTray();
        Assert.IsTrue(manny.GetComponent<ImageManager>().trayCreated, "The tray was not created.");
        
    }

    /*
    [Test]
    public void TestCreateDisplay()
    {
        GameObject mannyObj = new GameObject();
        mannyObj.AddComponent<ImageManager>();
        ImageManager manny = mannyObj.GetComponent<ImageManager>();


        GameObject thumbObject = new GameObject();
        thumbObject.AddComponent<Thumbnail>();
        thumbObject.AddComponent<SpriteRenderer>();

        GameObject dispObject = new GameObject();
        dispObject.AddComponent<Display>();
        dispObject.AddComponent<SpriteRenderer>();
        dispObject.AddComponent<BoxCollider>();

        Texture2D tex = new Texture2D(100, 100);

        manny.thumbnail = thumbObject;
        manny.displayObj = dispObject;
        Camera cam = new Camera();
        cam = Camera.main;
        manny.CreateDisplay(tex);

        // The list of displays should not be empty
        Assert.Greater(manny.GetDisplays().Count, 0, "The list of displays in the Image Manager is empty.");
    }
    */
}
