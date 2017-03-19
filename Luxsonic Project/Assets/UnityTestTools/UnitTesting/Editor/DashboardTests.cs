using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

[TestFixture]
[Category("Unit Tests")]
public class DashboardTests {

    [Test]
    public void DisplayMenu()
    {
		GameObject dashObj = new GameObject();
		dashObj.AddComponent<Dashboard>();
		Dashboard dash = dashObj.GetComponent<Dashboard>();

		dash.planePrefab = new GameObject();
		dash.SetCopyButtons( new List<VRButton>());
		GameObject buttonObj = new GameObject();
//		TextMesh btnMesh = new TextMesh ();
		buttonObj.AddComponent<VRButton> ();
		buttonObj.AddComponent<TextMesh> ();
		dash.button = buttonObj.GetComponent<VRButton>();

		dash.DisplayMenu ();
		VRButton[] myButtons = Transform.FindObjectsOfType<VRButton> ();
		Assert.Greater (myButtons.Length, 0);
		bool loadIn = false;
		bool quitIn = false;
		bool minimizeIn = false;
		bool contrastIn = false;
		bool rotateIn = false;
		bool brightnessIn = false;
		bool resizeIn = false;
		bool filterIn = false;
		bool closeIn = false;

		for (int i = 0; i < myButtons.Length; i= i + 1){
			VRButton button =  (VRButton) myButtons.GetValue(i);
			switch (button.name) {
			case "Load":
				// If the load button was clicked
				loadIn = true;
				break;
			case "Quit":
				// If the quit button was clicked
				quitIn = true;
				break;
			case "Minimize":
				// If the minimize button was clicked
				minimizeIn = true;
				break;
			case "Contrast":
				// If the quit button was clicked
				contrastIn = true;
				break;
			case "Rotate":
				// If the quit button was clicked
				rotateIn = true;
				break;
			case "Brightness":
				// If the quit button was clicked
				brightnessIn = true;
				break;
			case "Resize":
				// If the quit button was clicked
				resizeIn = true;
				break;
			case "Filter":
				// If the quit button was clicked
				filterIn = true;
				break;
			case "Close":
				// If the quit button was clicked
				closeIn = true;
				break;
			}
		}
		Assert.AreEqual (loadIn, true);
		Assert.AreEqual (quitIn, true);
		Assert.AreEqual (minimizeIn, true);
		Assert.AreEqual (contrastIn, true);
		Assert.AreEqual (rotateIn, true);
		Assert.AreEqual (brightnessIn, true);
		Assert.AreEqual (resizeIn, true);
		Assert.AreEqual (filterIn, true);
		Assert.AreEqual (closeIn, true);

	}
}