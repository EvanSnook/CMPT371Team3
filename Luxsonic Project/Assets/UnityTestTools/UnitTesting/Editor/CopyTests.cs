using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        Texture2D tex = new Texture2D(100, 100);


        newCopy.NewCopy(tex);

        // The properties of the display should not be null
        Assert.IsNotNull(newCopy.imageRenderer.sprite, "The sprite on the new display was null.");
        Assert.IsNotNull(newCopy.myTransform, "The transform on the new display is Null");
    }

    [Test]
    // Test the Display and Hide buttons functions by checking
    // the size of the display's button list.
    public void TestDisplayAndHideButtons()
    {
        GameObject copyObj = new GameObject();
        copyObj.AddComponent<Copy>();
        copyObj.AddComponent<SpriteRenderer>();
        copyObj.AddComponent<BoxCollider>();
        Copy newDisp = copyObj.GetComponent<Copy>();
        Texture2D tex = new Texture2D(100, 100);

        GameObject buttonObj = new GameObject();
        buttonObj.AddComponent<Button>();
        newDisp.button = buttonObj.GetComponent<Button>();

        newDisp.NewCopy(tex);

        newDisp.DisplayButtons();

        // The size of the button list should be greater than 0
        Assert.Greater(newDisp.GetButtons().Count, 0, "The list of buttons for the display was empty");

        newDisp.HideButtons();

        // The size of the button list should be 0
        Assert.AreEqual(newDisp.GetButtons().Count, 0, "The list of buttons was not cleared properly");
    }

}
   

