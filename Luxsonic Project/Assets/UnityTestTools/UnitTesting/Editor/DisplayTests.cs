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
    [Ignore("Send message does not work in editor mode")]
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

        GameObject thumbOutline = new GameObject();
        thumbOutline.AddComponent<SpriteRenderer>();
        thumbOutline.transform.SetParent(thumbObject.transform);

        GameObject dispImgObj = new GameObject();
        dispImgObj.AddComponent<SpriteRenderer>();
        dispImgObj.AddComponent<DisplayImage>();

        disp.displayImagePrefab = dispImgObj;

        trayObject.GetComponent<Tray>().thumbnailPrefab = thumbObject;
        disp.trayPrefab = trayObject;

        GameObject infoObj = new GameObject();
        infoObj.AddComponent<TextMesh>();
        infoObj.transform.SetParent(dispImgObj.transform);

        disp.displayImagePrefab = dispImgObj;

        Dictionary<string, string> patientInfo = new Dictionary<string, string>();
        patientInfo.Add("PatientName", "Steve");

        disp.AddImage(tex, patientInfo);

        // The manager should have a non-empty list and should contain the texture we created.
        Assert.Greater(disp.GetImages().Count, 0, "The list of images in the Image Manager is empty.");
        Assert.True(disp.GetImages().Contains(tex), "The list of images in the Image Manager does not contain the requested texture.");

    }

    [Test]
    public void TestCreateButton()
    {
        GameObject dispObj = new GameObject();
        dispObj.AddComponent<Display>();
        Display disp = dispObj.GetComponent<Display>();

        GameObject buttonObj = new GameObject();
        buttonObj.AddComponent<VRButton>();

        GameObject textObj = new GameObject();
        textObj.AddComponent<TextMesh>();
        textObj.transform.SetParent(buttonObj.transform);

        disp.buttonPrefab = buttonObj;

        ButtonAttributes attributes = new ButtonAttributes();
        GameObject button = disp.CreateButton(attributes, disp.buttonPrefab);

        Assert.IsNotNull(button);

    }

    [Test]
    [Ignore("Send message does not work in editor mode")]
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

        GameObject thumbOutline = new GameObject();
        thumbOutline.AddComponent<SpriteRenderer>();
        thumbOutline.transform.SetParent(thumbObject.transform);

        trayObject.GetComponent<Tray>().thumbnailPrefab = thumbObject;
        disp.trayPrefab = trayObject;

        FileInfo file = new FileInfo(@"Assets/resources/Disgruntled_Walrus_Logo.png");

        byte[] dicomImage = File.ReadAllBytes(file.ToString());

        Assert.AreNotEqual(0, dicomImage.Length, "The array of bytes from the File should not be empty");
        //From bytes, this is where we will call and write the code to decipher DICOMs
        Texture2D image = new Texture2D(1000, 1000);

        disp.CreateTray(image);
        Assert.IsTrue(disp.GetComponent<Display>().trayCreated, "The tray was not created.");

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

        disp.buttonPrefab = buttonPrefab;


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
    [Ignore("Send message does not work in editor mode")]
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

        disp.buttonPrefab = buttonPrefab;

        trayObject.GetComponent<Tray>().buttonPrefab = buttonPrefab;

        GameObject thumbObject = new GameObject();
        thumbObject.AddComponent<Thumbnail>();
        thumbObject.AddComponent<SpriteRenderer>();

        GameObject thumbOutline = new GameObject();
        thumbOutline.AddComponent<SpriteRenderer>();
        thumbOutline.transform.SetParent(thumbObject.transform);

        GameObject copyPrefab = new GameObject();
        copyPrefab.AddComponent<SpriteRenderer>();
        copyPrefab.AddComponent<Copy>();
        copyPrefab.AddComponent<BoxCollider>();

        GameObject dispImgObj = new GameObject();
        dispImgObj.AddComponent<SpriteRenderer>();
        dispImgObj.AddComponent<DisplayImage>();
        GameObject dispImgChild = new GameObject();
        dispImgChild.transform.SetParent(dispImgObj.transform);

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
        Dictionary<string, string> patientInfo = new Dictionary<string, string>();
        patientInfo.Add("PatientName", "Steve");
        // This function takes in a dictionary as well
        disp.AddImage(tex, patientInfo);
        disp.AddImage(tex, patientInfo);
        disp.AddImage(tex, patientInfo);
        disp.AddImage(tex, patientInfo);

        disp.TestScrollLeftAndRight(4);

    }

}
