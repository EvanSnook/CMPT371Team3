using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

[TestFixture]
[Category("System Tests")]
public class SystemTests : MonoBehaviour {

	GameObject dashboard;
	[Test]
	[Category("CopyImages")]
	public void CopyExists()
	{
		Dashboard dashboard = new Dashboard ();
		Display testDisplay = new Display ();
		dashboard.display = testDisplay;

		//dashboard.dissplay = GameObject.FindGameObjectWithTag("DisplayImage").GetComponent<Display>();

		testDisplay.AddImage(dashboard.dummyImages[0]); // Load in the default image.
		testDisplay.CreateCopy(testDisplay.GetImages()[0]);								// Creating the first copy

		Assert.AreEqual(GameObject.FindGameObjectsWithTag ("Copy").Length, 1);		//Check that there is a copy created. */
	}

	[Test]
	[Category("CopyImages")]
	public void CopyDifferentThanDisplay()
	{
		Dashboard dashboard = new Dashboard ();
		Display testDisplay = new Display ();

		dashboard.display = testDisplay;
		dashboard.VRButtonClicked ("Load"); // Load in the default images.
		testDisplay.CreateCopy(testDisplay.GetImages()[0]); // Creating the first copy

		//Ensure image is same.... && Game object is different... 
		Sprite dImage = GameObject.FindGameObjectsWithTag("DisplayImage")[0].GetComponent<SpriteRenderer>().sprite;
		Sprite cImage =  GameObject.FindGameObjectsWithTag("Copy")[0].GetComponent<SpriteRenderer>().sprite;
		GameObject dObj = GameObject.FindGameObjectsWithTag("DisplayImage")[0];
		GameObject cObj = GameObject.FindGameObjectsWithTag ("Copy")[0];
		Assert ((dImage == cImage)  && (dObj !=cObj));
	}

	[Test]
	[Category("CopyImages")]
	public void CopyDifferentThanTray(){ 
		Dashboard dashboard = new Dashboard ();
		Display testDisplay = new Display ();
		dashboard.display = testDisplay;
		dashboard.VRButtonClicked ("Load"); // Load in the default images.
		testDisplay.CreateCopy(testDisplay.GetImages()[0]); 

		Sprite tSprite = GameObject.FindGameObjectsWithTag("Thumbnail")[0].GetComponent<SpriteRenderer>().sprite;
		Sprite cSprite =  GameObject.FindGameObjectsWithTag("Copy")[0].GetComponent<SpriteRenderer>().sprite;
		GameObject tObj = GameObject.FindGameObjectsWithTag("Thumbnail")[0];
		GameObject cObj = GameObject.FindGameObjectsWithTag ("Copy")[0];
		Assert ((tSprite == cSprite)  && (tObj !=cObj));

	}

	[Test]
	[Category("RemoveCopies")]
	public void RemoveCopyFromWorkspace(){ 
		Dashboard dashboard = new Dashboard ();
		Display testDisplay = new Display ();
		dashboard.display = testDisplay;
		dashboard.VRButtonClicked ("Load"); // Load in the default images.
		testDisplay.CreateCopy(testDisplay.GetImages()[0]); 
		Copy testCopy = GameObject.FindGameObjectsWithTag ("Copy") [0];
		testCopy.VRButtonClicked ("Close");

		Copy[] copiesInWorkspace = GameObject.FindGameObjectsWithTag ("Copy");
		Assert.AreEqual (copiesInWorkspace.Length, 0);
	}

	[Test]
	[Category("Left/Right")]
	public void NextButtonWith4(){ 
		Dashboard dashboard = new Dashboard ();
		Display testDisplay = new Display ();
		dashboard.display = testDisplay;
		//load 4 images to active left and right buttons
		dashboard.VRButtonClicked ("Load");
		dashboard.VRButtonClicked ("Load");
		dashboard.VRButtonClicked ("Load");
		dashboard.VRButtonClicked ("Load");

		bool leftButtonSuccess = false;
		bool rightButtonSuccess = false;
		GameObject[] buttonsList = GameObject.FindGameObjectsWithTag ("MenuButton");
		for (int i = 0; i < buttonsList.Length; i++) {
			string buttonName = buttonsList [i].GetComponent<VRButton> ().name;
			if (buttonName.CompareTo ("Left") == 0) {
				leftButtonSuccess = true;
			}
			else if (buttonName.CompareTo ("Right") == 0) {
				rightButtonSuccess = true;
			}
		}
		//Assert left and right buttons exist
		Assert.AreEqual (leftButtonSuccess, true);
		Assert.AreEqual (righttButtonSuccess, true);

		GameObject[] displayImagesList = GameObject.FindGameObjectsWithTag ("DisplayImage");
		GameObject firstEl = displayImagesList [0];
		GameObject secondEl = displayImagesList [1];
		testDisplay.VRButtonClicked ("Left");
		int size = displayImagesList.Length;
		Assert.AreEqual(firstEl, displayImagesList[size-1]);
		Assert.AreEqual(secondEl, displayImagesList[0]);

	}
}

