﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using NSubstitute;

[TestFixture]
[Category("Unit Tests")]
// Testing for the Display Class
public class CopyTests
{
    [Test]
    // Test the new display function by creating a new display with a simple texture.
    public void TestNewCopy()
    {
        GameObject copyObj = new GameObject();
        copyObj.AddComponent<Copy>();
        copyObj.AddComponent<SpriteRenderer>();
        copyObj.AddComponent<BoxCollider>();

        Copy newCopy = copyObj.GetComponent<Copy>();
        GameObject child = new GameObject();
        child.AddComponent<SpriteRenderer>();

        child.transform.SetParent(newCopy.transform);
//        Material mat = (Material)AssetDatabase.LoadAssetAtPath("Assets/Resources/Materials/Red.mat", typeof(Material));

        Shader shad = (Shader)AssetDatabase.LoadAssetAtPath("Assets/Resources/Shaders/ImageEffects.shader", typeof(Shader));

        newCopy.curShader = shad;
        Assert.IsNotNull(newCopy.curShader);

        Texture2D tex = new Texture2D(100, 100);

        newCopy.NewCopy(tex);

        // The properties of the display should not be null
        Assert.IsNotNull(newCopy.transform, "The transform on the new display is Null");
        Assert.AreEqual(tex, newCopy.GetComponent<SpriteRenderer>().sprite.texture,
                            "Image in copy does not match image assigned");
    }

    [Test]
    public void TestBrightness()
    {
        GameObject copyObj = new GameObject();
        copyObj.AddComponent<Copy>();
        copyObj.AddComponent<SpriteRenderer>();
        copyObj.AddComponent<BoxCollider>();

        Copy newCopy = copyObj.GetComponent<Copy>();
        GameObject child = new GameObject();
        child.AddComponent<SpriteRenderer>();

        child.transform.SetParent(newCopy.transform);
//        Material mat = (Material)AssetDatabase.LoadAssetAtPath("Assets/Resources/Materials/Red.mat", typeof(Material));

        Shader shad = (Shader)AssetDatabase.LoadAssetAtPath("Assets/Resources/Shaders/ImageEffects.shader", typeof(Shader));

        newCopy.curShader = shad;
        Assert.IsNotNull(newCopy.curShader);

        Texture2D tex = new Texture2D(100, 100);

        newCopy.NewCopy(tex);

        // Increase the brightness
        float originalBrightness = newCopy.GetMaterial().GetFloat("_BrightnessAmount");

        newCopy.TestPrivateAttributes(1, "brightness");

        float newBrightness = newCopy.GetMaterial().GetFloat("_BrightnessAmount");

        Assert.AreEqual(originalBrightness + newCopy.GetBrightnessConst(), newBrightness);

        // Decrease the brightness
        originalBrightness = newCopy.GetMaterial().GetFloat("_BrightnessAmount");

        newCopy.TestPrivateAttributes(-1, "brightness");

        newBrightness = newCopy.GetMaterial().GetFloat("_BrightnessAmount");

        Assert.AreEqual(originalBrightness - newCopy.GetBrightnessConst(), newBrightness);
    }

    [Test]
    public void TestContrast()
    {
        GameObject copyObj = new GameObject();
        copyObj.AddComponent<Copy>();
        copyObj.AddComponent<SpriteRenderer>();
        copyObj.AddComponent<BoxCollider>();

        Copy newCopy = copyObj.GetComponent<Copy>();
        GameObject child = new GameObject();
        child.AddComponent<SpriteRenderer>();

        child.transform.SetParent(newCopy.transform);
//        Material mat = (Material)AssetDatabase.LoadAssetAtPath("Assets/Resources/Materials/Red.mat", typeof(Material));

        Shader shad = (Shader)AssetDatabase.LoadAssetAtPath("Assets/Resources/Shaders/ImageEffects.shader", typeof(Shader));

        newCopy.curShader = shad;
        Assert.IsNotNull(newCopy.curShader);

        Texture2D tex = new Texture2D(100, 100);

        newCopy.NewCopy(tex);

        // Increase the contrast
        float originalContrast = newCopy.GetMaterial().GetFloat("_ContrastAmount");

        newCopy.TestPrivateAttributes(1, "contrast");

        float newContrast = newCopy.GetMaterial().GetFloat("_ContrastAmount");

        Assert.AreEqual(originalContrast + newCopy.GetContrastConst(), newContrast);

        // decrease the contrast
        originalContrast = newCopy.GetMaterial().GetFloat("_ContrastAmount");

        newCopy.TestPrivateAttributes(-1, "contrast");

        newContrast = newCopy.GetMaterial().GetFloat("_ContrastAmount");

        Assert.AreEqual(originalContrast - newCopy.GetContrastConst(), newContrast);
    }

    [Test]
    // Test the resize functionality of copy
    public void TestResize()
    {
        GameObject copyObj = new GameObject();
        copyObj.AddComponent<Copy>();
        copyObj.AddComponent<SpriteRenderer>();
        copyObj.AddComponent<BoxCollider>();

        Copy newCopy = copyObj.GetComponent<Copy>();
        GameObject child = new GameObject();
        child.AddComponent<SpriteRenderer>();

        child.transform.SetParent(newCopy.transform);
//        Material mat = (Material)AssetDatabase.LoadAssetAtPath("Assets/Resources/Materials/Red.mat", typeof(Material));

        Shader shad = (Shader)AssetDatabase.LoadAssetAtPath("Assets/Resources/Shaders/ImageEffects.shader", typeof(Shader));

        newCopy.curShader = shad;
        Assert.IsNotNull(newCopy.curShader);


        Texture2D tex = new Texture2D(100, 100);

        newCopy.NewCopy(tex);

        // Increase the size
        Vector3 originalSize = newCopy.transform.localScale;

        newCopy.TestPrivateAttributes(1, "resize");

        Vector3 newSize = newCopy.transform.localScale;

        Assert.AreEqual(originalSize * newCopy.GetResizeScale(), newSize);

        // Decrease the size
        originalSize = newCopy.transform.localScale;

        newCopy.TestPrivateAttributes(-1, "resize");

        newSize = newCopy.transform.localScale;

        Assert.AreEqual(originalSize / newCopy.GetResizeScale(), newSize);
    }

    
}
   

