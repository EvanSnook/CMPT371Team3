using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using buttons;

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

		dash.buttonAttributes = new Dashboard.ButtonAttributes[] {
			new Dashboard.ButtonAttributes("Load", new Vector3(0, 0.3f, 0)),
			new Dashboard.ButtonAttributes("Quit", new Vector3(0, 0, 0)),
			new Dashboard.ButtonAttributes("Minimize", new Vector3(0, -0.3f, 0)),
			new Dashboard.ButtonAttributes("Contrast", new Vector3(-0.3f, 0.2f, 0)),
			new Dashboard.ButtonAttributes("Rotate", new Vector3(0, 0.2f, 0)),
			new Dashboard.ButtonAttributes("Zoom", new Vector3(0.3f, 0.2f, 0)),
			new Dashboard.ButtonAttributes("Brightness", new Vector3(-0.3f, -0.2f, 0)),
			new Dashboard.ButtonAttributes("Resize", new Vector3(0, -0.2f, 0)),
			new Dashboard.ButtonAttributes("Filter", new Vector3(0.3f, -0.2f, 0)),
			new Dashboard.ButtonAttributes("Close", new Vector3(0, -0.7f, 0)),
			new Dashboard.ButtonAttributes("Restore", new Vector3(-0.3f, -0.7f, 0))
		};


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

	[Test]
	public void VRButtonClicked()
	{
		//Setup conditions
		//create dashboard object
		GameObject dashObj = new GameObject();
		dashObj.AddComponent<Dashboard>();
		Dashboard dash = dashObj.GetComponent<Dashboard>();

		//initialize neccesary members of dashboard
		//setup display
		GameObject displayObj = new GameObject();
		displayObj.AddComponent<Display>();
		Display disp = displayObj.GetComponent<Display>();
		dash.display = disp;

		GameObject trayObject = new GameObject();
		trayObject.AddComponent<Tray>();
		disp.trayPrefab = trayObject;

		GameObject thumbObject = new GameObject();
		thumbObject.AddComponent<Thumbnail>();
		thumbObject.AddComponent<SpriteRenderer>();
		trayObject.GetComponent<Tray>().thumbnailPrefab = thumbObject;

		GameObject dispImgObj = new GameObject();
		dispImgObj.AddComponent<SpriteRenderer>();
		dispImgObj.tag = "DisplayImage";
		disp.displayImagePrefab = dispImgObj;
		dash.dummyImages = new Texture2D[1];
		dash.dummyImages.SetValue (new Texture2D(5,6), 0);

        dash.buttonAttributes = new Dashboard.ButtonAttributes[] {
			new Dashboard.ButtonAttributes("Load", new Vector3(0, 0.3f, 0)),
			new Dashboard.ButtonAttributes("Quit", new Vector3(0, 0, 0)),
			new Dashboard.ButtonAttributes("Minimize", new Vector3(0, -0.3f, 0)),
			new Dashboard.ButtonAttributes("Contrast", new Vector3(-0.3f, 0.2f, 0)),
			new Dashboard.ButtonAttributes("Rotate", new Vector3(0, 0.2f, 0)),
			new Dashboard.ButtonAttributes("Zoom", new Vector3(0.3f, 0.2f, 0)),
			new Dashboard.ButtonAttributes("Brightness", new Vector3(-0.3f, -0.2f, 0)),
			new Dashboard.ButtonAttributes("Resize", new Vector3(0, -0.2f, 0)),
			new Dashboard.ButtonAttributes("Filter", new Vector3(0.3f, -0.2f, 0)),
			new Dashboard.ButtonAttributes("Close", new Vector3(0, -0.7f, 0)),
			new Dashboard.ButtonAttributes("Restore", new Vector3(-0.3f, -0.7f, 0))
		};

		//set up dashboard
		dash.planePrefab = new GameObject();
		dash.SetCopyButtons( new List<VRButton>());
		GameObject buttonObj = new GameObject();
		TextMesh btnMesh = new TextMesh ();
		buttonObj.AddComponent<VRButton> ();
		buttonObj.AddComponent<TextMesh> ();
		dash.button = buttonObj.GetComponent<VRButton>();
		dash.DisplayMenu ();

		//Test VRButtonClicked with "Load" option
		int myImages = dash.display.GetImages().Count;
		Assert.AreEqual (0, myImages);
        dash.VRButtonClicked(ButtonType.LOAD_BUTTON);
		myImages = dash.display.GetImages().Count;
		Assert.AreEqual (1, myImages);

		//Test VRButtonClicked with "Minimize" option
		Assert.AreEqual(false, dash.getMinimized());
		dash.VRButtonClicked(ButtonType.MINIMIZE_BUTTON);
		Assert.AreEqual(true, dash.getMinimized());
		dash.VRButtonClicked(ButtonType.MINIMIZE_BUTTON);
		Assert.AreEqual(false, dash.getMinimized());

		//Test VRButtonClicked with "Brightness" option
		dash.currentCopies = new List<GameObject>();
		Assert.AreEqual(0, dash.getCurrentSelection().CompareTo(ButtonType.NONE));
		dash.VRButtonClicked(ButtonType.BRIGHTNESS_BUTTON);
		Assert.AreEqual(0, dash.getCurrentSelection().CompareTo(ButtonType.BRIGHTNESS_BUTTON));

		//Test VRButtonClicked with "Contrast" option
		Assert.AreEqual(0, dash.getCurrentSelection().CompareTo(ButtonType.BRIGHTNESS_BUTTON));
		dash.VRButtonClicked(ButtonType.CONTRAST_BUTTON);
		Assert.AreEqual(0, dash.getCurrentSelection().CompareTo(ButtonType.CONTRAST_BUTTON));

		//Test VRButtonClicked with "Resize" option
		Assert.AreEqual(0, dash.getCurrentSelection().CompareTo(ButtonType.CONTRAST_BUTTON));
		dash.VRButtonClicked(ButtonType.RESIZE_BUTTON);
		Assert.AreEqual(0, dash.getCurrentSelection().CompareTo(ButtonType.RESIZE_BUTTON));

		//Test VRButtonClicked with "Close" option
		Assert.AreEqual(0, dash.getCurrentSelection().CompareTo(ButtonType.RESIZE_BUTTON));
		dash.VRButtonClicked(ButtonType.CLOSE_BUTTON);
        Assert.AreEqual(0, dash.getCurrentSelection().CompareTo(ButtonType.NONE));
	}
}