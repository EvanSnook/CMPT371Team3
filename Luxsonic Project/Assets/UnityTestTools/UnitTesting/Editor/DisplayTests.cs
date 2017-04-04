using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using NSubstitute;
using System.IO;

[TestFixture]
[Category("Unit Tests")]
public class DisplayTests
{

    [Test]
	[Ignore("Needs to be fixed")]
    public void TestAddImage()
    {
        GameObject mannyObj = new GameObject();
        mannyObj.AddComponent<Display>();
        Display disp = mannyObj.GetComponent<Display>();
        Texture2D tex = new Texture2D(100, 100);

        GameObject trayObject = new GameObject();
        trayObject.AddComponent<Tray>();



        GameObject thumbObject = new GameObject();
        thumbObject.AddComponent<Thumbnail>();
        thumbObject.AddComponent<SpriteRenderer>();

        GameObject dispImgObj = new GameObject();
        dispImgObj.AddComponent<SpriteRenderer>();

        disp.displayImagePrefab = dispImgObj;

        trayObject.GetComponent<Tray>().thumbnailPrefab = thumbObject;
        disp.trayPrefab = trayObject;

        //disp.AddImage(tex);

        // The manager should have a non-empty list and should contain the texture we created.
        Assert.Greater(disp.GetImages().Count, 0, "The list of images in the Image Manager is empty.");
        Assert.True(disp.GetImages().Contains(tex), "The list of images in the Image Manager does not contain the requested texture.");
    }

    [Test]
    public void TestCreateTray()
    {
        GameObject dispObj = new GameObject();
        dispObj.AddComponent<Display>();
        Display disp = dispObj.GetComponent<Display>();

        GameObject trayObject = new GameObject();
        trayObject.AddComponent<Tray>();



        GameObject thumbObject = new GameObject();
        thumbObject.AddComponent<Thumbnail>();
        thumbObject.AddComponent<SpriteRenderer>();

        trayObject.GetComponent<Tray>().thumbnailPrefab = thumbObject;
        disp.trayPrefab = trayObject;

        FileInfo file = new FileInfo(@"Assets/resources/Test.png");

        byte[] dicomImage = File.ReadAllBytes(file.ToString());

        Assert.AreNotEqual(0, dicomImage.Length, "The array of bytes from the File should not be empty");
        //From bytes, this is where we will call and write the code to decipher DICOMs
        Texture2D image = new Texture2D(1000, 1000);

        disp.CreateTray(image);
        Assert.IsTrue(disp.GetComponent<Display>().trayCreated, "The tray was not created.");

    }


    [Test]
    [Ignore("Test has some life issues.")]
    public void TestCreateDisplay()
    {
        //        GameObject mannyObj = new GameObject();
        //mannyObj.AddComponent<ImageManager>();
        //ImageManager manny = mannyObj.GetComponent<ImageManager>();


        GameObject thumbObject = new GameObject();
        thumbObject.AddComponent<Thumbnail>();
        thumbObject.AddComponent<SpriteRenderer>();

        GameObject dispObject = new GameObject();
        dispObject.AddComponent<Display>();
        dispObject.AddComponent<SpriteRenderer>();
        dispObject.AddComponent<BoxCollider>();

        //        Texture2D tex = new Texture2D(100, 100);

        //manny.thumbnail = thumbObject;
        //manny.displayObj = dispObject;
        Camera cam = new Camera();
        cam = Camera.main;
        // manny.CreateDisplay(tex);

        // The list of displays should not be empty
        //Assert.Greater(manny.GetDisplays().Count, 0, "The list of displays in the Image Manager is empty.");
    }

    [Test]
    public void TestCreateScrollButtons()
    {
        GameObject dispObj = new GameObject();
        dispObj.AddComponent<Display>();
        Display disp = dispObj.GetComponent<Display>();

        GameObject trayObject = new GameObject();
        trayObject.AddComponent<Tray>();

        GameObject buttonPrefab = new GameObject();
        buttonPrefab.AddComponent<VRButton>();

        GameObject textObject = new GameObject();
        textObject.AddComponent<TextMesh>();
        textObject.transform.SetParent(buttonPrefab.transform);

        disp.buttonPrefab = buttonPrefab.GetComponent<VRButton>();
        

        GameObject thumbObject = new GameObject();
        thumbObject.AddComponent<Thumbnail>();
        thumbObject.AddComponent<SpriteRenderer>();

        trayObject.GetComponent<Tray>().thumbnailPrefab = thumbObject;
        disp.trayPrefab = trayObject;

        VRButton[] scrollBtns = disp.TestCreateScrollButtons();

        Assert.NotNull(scrollBtns[0]);
        Assert.NotNull(scrollBtns[1]);
    }

    [Test]
	[Ignore("Test does not work with the camera.")]
    public void TestCreateCopy()
    {
        GameObject dispObj = new GameObject();
        dispObj.AddComponent<Display>();
        Display disp = dispObj.GetComponent<Display>();

        GameObject trayObject = new GameObject();
        trayObject.AddComponent<Tray>();

        GameObject dispImgObj = new GameObject();
        dispImgObj.AddComponent<SpriteRenderer>();

        disp.displayImagePrefab = dispImgObj;

        GameObject buttonPrefab = new GameObject();
        buttonPrefab.AddComponent<VRButton>();

        GameObject textObject = new GameObject();
        textObject.AddComponent<TextMesh>();
        textObject.transform.SetParent(buttonPrefab.transform);

        disp.buttonPrefab = buttonPrefab.GetComponent<VRButton>();


        GameObject thumbObject = new GameObject();
        thumbObject.AddComponent<Thumbnail>();
        thumbObject.AddComponent<SpriteRenderer>();

        GameObject copyPrefab = new GameObject();
        copyPrefab.AddComponent<SpriteRenderer>();
        copyPrefab.AddComponent<Copy>();
        copyPrefab.AddComponent<BoxCollider>();
        GameObject child = new GameObject();
        child.AddComponent<SpriteRenderer>();

        child.transform.SetParent(copyPrefab.transform);

        Shader shad = (Shader)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Resources/Shaders/ImageEffects.shader", typeof(Shader));

        copyPrefab.GetComponent<Copy>().curShader = shad;
        
        disp.copyPrefab = copyPrefab;

        trayObject.GetComponent<Tray>().thumbnailPrefab = thumbObject;
        disp.trayPrefab = trayObject;

        Texture2D tex = new Texture2D(100, 100);

        disp.CreateCopy(tex);

        Assert.AreEqual(1, disp.GetCopies().Count);

        disp.CreateCopy(tex);
        Assert.AreEqual(2, disp.GetCopies().Count);
    }

    [Test]
	[Ignore("Test has some life issues.")]
    public void TestScrollLeftAndRight()
    {
        GameObject dispObj = new GameObject();
        dispObj.AddComponent<Display>();
        Display disp = dispObj.GetComponent<Display>();

        GameObject trayObject = new GameObject();
        trayObject.AddComponent<Tray>();

        GameObject buttonPrefab = new GameObject();
        buttonPrefab.AddComponent<VRButton>();

        GameObject textObject = new GameObject();
        textObject.AddComponent<TextMesh>();
        textObject.transform.SetParent(buttonPrefab.transform);

        disp.buttonPrefab = buttonPrefab.GetComponent<VRButton>();


        GameObject thumbObject = new GameObject();
        thumbObject.AddComponent<Thumbnail>();
        thumbObject.AddComponent<SpriteRenderer>();

        GameObject copyPrefab = new GameObject();
        copyPrefab.AddComponent<SpriteRenderer>();
        copyPrefab.AddComponent<Copy>();
        copyPrefab.AddComponent<BoxCollider>();

        GameObject dispImgObj = new GameObject();
        dispImgObj.AddComponent<SpriteRenderer>();

        disp.displayImagePrefab = dispImgObj;

        GameObject child = new GameObject();
        child.AddComponent<SpriteRenderer>();

        child.transform.SetParent(copyPrefab.transform);

        Shader shad = (Shader)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Resources/Shaders/ImageEffects.shader", typeof(Shader));

        copyPrefab.GetComponent<Copy>().curShader = shad;

        disp.copyPrefab = copyPrefab;

        trayObject.GetComponent<Tray>().thumbnailPrefab = thumbObject;
        disp.trayPrefab = trayObject;

        Texture2D tex = new Texture2D(100, 100);

		// This function takes in a dictionary as well
        /*disp.AddImage(tex);
        disp.AddImage(tex);
        disp.AddImage(tex);
        disp.AddImage(tex);*/
        
        disp.TestScrollLeftAndRight(4);
        
    }

}
