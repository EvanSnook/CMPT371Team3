using System.Collections;
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
        Material mat = (Material)AssetDatabase.LoadAssetAtPath("Assets/Resources/Materials/Red.mat", typeof(Material));

        Shader shad = (Shader)AssetDatabase.LoadAssetAtPath("Assets/Resources/Shaders/ImageEffects.shader", typeof(Shader));

        newCopy.curShader = shad;
        Assert.IsNotNull(newCopy.curShader);

        newCopy.SetCopyMaterial(mat);
        Texture2D tex = new Texture2D(100, 100);

        newCopy.NewCopy(tex);

        // The properties of the display should not be null
        Assert.IsNotNull(newCopy.myTransform, "The transform on the new display is Null");
        Assert.AreEqual(tex, newCopy.GetComponent<SpriteRenderer>().sprite.texture,
                            "Image in copy does not match image assigned");
    }

    [Test]
    [Ignore("Test no longer relevant")]
    // Test the Display and Hide buttons functions by checking
    // the size of the display's button list.
    public void TestDisplayAndHideButtons()
    {
        GameObject copyObj = new GameObject();
        copyObj.AddComponent<Copy>();
        copyObj.AddComponent<SpriteRenderer>();
        copyObj.AddComponent<BoxCollider>();
        Copy newCopy = copyObj.GetComponent<Copy>();
        Texture2D tex = new Texture2D(100, 100);

        Material mat = (Material)AssetDatabase.LoadAssetAtPath("Assets/Resources/Materials/Red.mat", typeof(Material));
        Shader shad = (Shader)AssetDatabase.LoadAssetAtPath("Assets/Resources/Shaders/ImageEffects.shader", typeof(Shader));

        newCopy.curShader = shad;
        Assert.IsNotNull(newCopy.curShader);

        newCopy.SetCopyMaterial(mat);

        GameObject buttonObj = new GameObject();
        buttonObj.AddComponent<VRButton>();
        newCopy.buttonPrefab = buttonObj.GetComponent<VRButton>();

        newCopy.NewCopy(tex);

        //newCopy.DisplayButtons();

        // The size of the button list should be greater than 0
        Assert.Greater(newCopy.GetButtons().Count, 0, "The list of buttons for the display was empty");
        Assert.AreEqual(newCopy.GetButtons().Count, 7, "The correct number of buttons have not been created");
        //newCopy.HideButtons();

        // The size of the button list should be 0
        Assert.AreEqual(newCopy.GetButtons().Count, 0, "The list of buttons was not cleared properly");
    }

    [Test]
    [Ignore("Test No Longer Relevant")]
    // Test the functionality button clicks and make sure the
    // switch cases work
    public void TestVRButtonClicks()
    {
        GameObject copyObj = new GameObject();
        copyObj.AddComponent<Copy>();
        copyObj.AddComponent<SpriteRenderer>();
        copyObj.AddComponent<BoxCollider>();
        Copy newCopy = copyObj.GetComponent<Copy>();
        Texture2D tex = new Texture2D(100, 100);

        Material mat = (Material)AssetDatabase.LoadAssetAtPath("Assets/Resources/Materials/Red.mat", typeof(Material));
        Shader shad = (Shader)AssetDatabase.LoadAssetAtPath("Assets/Resources/Shaders/ImageEffects.shader", typeof(Shader));
           
        newCopy.curShader = shad;
        Assert.IsNotNull(newCopy.curShader);

        newCopy.SetCopyMaterial(mat);

        GameObject buttonObj = new GameObject();
        buttonObj.AddComponent<VRButton>();
        newCopy.buttonPrefab = buttonObj.GetComponent<VRButton>();

        newCopy.NewCopy(tex);

        Assert.AreEqual(newCopy.GetButtons().Count, 7, "The correct number of buttons do not exist");

        newCopy.VRButtonClicked("Close");

        Assert.AreEqual(newCopy.GetButtons().Count, 0, "Close case in VRButtonClicks() failed");
    }
}
   

