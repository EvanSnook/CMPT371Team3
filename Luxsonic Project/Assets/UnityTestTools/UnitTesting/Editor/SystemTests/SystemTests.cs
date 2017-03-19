using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using NUnit.Framework;

using buttons;

[TestFixture]
[Category("System Tests")]
public class SystemTests : MonoBehaviour {

	[Ignore("FAILING TEST: CopyExists...Cannot load images.")]
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

	[Ignore("FAILING TEST: CopyDifferentThanDiplay...Cannot load images.")]
	[Category("CopyImages")]
	public void CopyDifferentThanDisplay()
	{
		Dashboard dashboard = new Dashboard ();
		Display testDisplay = new Display ();

		dashboard.display = testDisplay;
		dashboard.VRButtonClicked (ButtonType.LOAD_BUTTON); // Load in the default images.
		testDisplay.CreateCopy(testDisplay.GetImages()[0]); // Creating the first copy

		//Ensure image is same.... && Game object is different... 
		Sprite dImage = GameObject.FindGameObjectsWithTag("DisplayImage")[0].GetComponent<SpriteRenderer>().sprite;
		Sprite cImage =  GameObject.FindGameObjectsWithTag("Copy")[0].GetComponent<SpriteRenderer>().sprite;
		GameObject dObj = GameObject.FindGameObjectsWithTag("DisplayImage")[0];
		GameObject cObj = GameObject.FindGameObjectsWithTag ("Copy")[0];
		Assert.AreEqual(dImage,cImage);
		Assert.AreNotEqual(dObj, cObj);
	}

	[Ignore("FAILING TEST: CopyDifferentThanTray...Cannot load images.")]
	[Category("CopyImages")]
	public void CopyDifferentThanTray(){ 
		Dashboard dashboard = new Dashboard ();
		Display testDisplay = new Display ();
		dashboard.display = testDisplay;
		dashboard.VRButtonClicked (ButtonType.LOAD_BUTTON); // Load in the default images.
		testDisplay.CreateCopy(testDisplay.GetImages()[0]); 

		Sprite tSprite = GameObject.FindGameObjectsWithTag("Thumbnail")[0].GetComponent<SpriteRenderer>().sprite;
		Sprite cSprite =  GameObject.FindGameObjectsWithTag("Copy")[0].GetComponent<SpriteRenderer>().sprite;
		GameObject tObj = GameObject.FindGameObjectsWithTag("Thumbnail")[0];
		GameObject cObj = GameObject.FindGameObjectsWithTag ("Copy")[0];

		Assert.AreEqual(tSprite,cSprite);
		Assert.AreNotEqual(tObj,cObj);

	}

	[Ignore("FAILING TEST: RemoveCopyFromWorkspace...Cannot load images.")]
	[Category("RemoveCopies")]
	public void RemoveCopyFromWorkspace(){ 
		Dashboard dashboard = new Dashboard ();
		Display testDisplay = new Display ();
		dashboard.display = testDisplay;
		dashboard.VRButtonClicked (ButtonType.LOAD_BUTTON); // Load in the default images.
		testDisplay.CreateCopy (testDisplay.GetImages () [0]); 
		Copy testCopy = GameObject.FindGameObjectsWithTag("Copy")[0].GetComponent<Copy>();
		testCopy.NewOptions ("Close");

		// Tests there are zero copies in workspace.
		int copiesInWorkspace = GameObject.FindGameObjectsWithTag ("Copy").Length;
		Assert.AreEqual (copiesInWorkspace, 0);

		// Tests that there is one image in workspace.
		int imagesInWorkspace = GameObject.FindGameObjectsWithTag ("DisplayImage").Length;
		Assert.AreEqual (imagesInWorkspace, 1);

		// Test that there is one thumbnail in workspace.
		int thumbnailsInWorkspace = GameObject.FindGameObjectsWithTag ("Thumbnail").Length;
		Assert.AreEqual (thumbnailsInWorkspace, 1);
	}

	[Ignore("FAILING TEST: NextButtonWith4...Cannot load images.")]
	[Category("Left/Right")]
	public void NextButtonWith4(){ 
		Dashboard dashboard = new Dashboard ();
		Display testDisplay = new Display ();
		dashboard.display = testDisplay;
		//load 4 images to active left and right buttons
		dashboard.VRButtonClicked (ButtonType.LOAD_BUTTON);
		dashboard.VRButtonClicked (ButtonType.LOAD_BUTTON);
		dashboard.VRButtonClicked (ButtonType.LOAD_BUTTON);
		dashboard.VRButtonClicked (ButtonType.LOAD_BUTTON);

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
		Assert.AreEqual (rightButtonSuccess, true);

		GameObject[] displayImagesList = GameObject.FindGameObjectsWithTag ("DisplayImage");
		GameObject firstImageInList = displayImagesList [0];
		GameObject secondImageInList = displayImagesList [1];
		testDisplay.VRButtonClicked (ButtonType.LEFT_BUTTON);
		int size = displayImagesList.Length;

		//Assert first element has moved to end and second element has taken first spot.
		Assert.AreEqual(firstImageInList, displayImagesList[size-1]);
		Assert.AreEqual(secondImageInList, displayImagesList[0]);

	}

	[Ignore("FAILING TEST: PrevButtonWith4...Cannot load images.")]
	[Category("Left/Right")]
	public void PrevButtonWith4(){ 
		Dashboard dashboard = new Dashboard ();
		Display testDisplay = new Display ();
		dashboard.display = testDisplay;
		//load 4 images to active left and right buttons
		dashboard.VRButtonClicked (ButtonType.LOAD_BUTTON);
		dashboard.VRButtonClicked (ButtonType.LOAD_BUTTON);
		dashboard.VRButtonClicked (ButtonType.LOAD_BUTTON);
		dashboard.VRButtonClicked (ButtonType.LOAD_BUTTON);

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
		Assert.AreEqual (rightButtonSuccess, true);

		GameObject[] displayImagesList = GameObject.FindGameObjectsWithTag ("DisplayImage");
		int size = displayImagesList.Length;

		GameObject lastImageInList = displayImagesList [size-1];
		GameObject firstImageInList = displayImagesList [0];
		testDisplay.VRButtonClicked (ButtonType.RIGHT_BUTTON);

		Assert.AreEqual(lastImageInList, displayImagesList[0]);
		Assert.AreEqual(firstImageInList, displayImagesList[1]);
	}

	// This test checks the case where the "Left" button is pressed when there are no images.
	[Ignore("Failing test... Handling not built in yet.")]
	[Category ("Left/Right")]
	public void NextButtonNoImages(){
		Dashboard dashboard = new Dashboard ();
		Display testDisplay = new Display ();
		dashboard.display = testDisplay;

/*
		try{
			testDisplay.VRButtonClicked ("Left");
		} 
		catch (Exception e){
			// Assert that a NullReferenceException was thrown.
			Assert.AreEqual(NullReferenceException, e.InnerException);
		}
*/ 
		// Assert that a NullReferenceException is thrown when button is clicked "left"
		Assert.Throws<NullReferenceException>( () => testDisplay.VRButtonClicked (ButtonType.LEFT_BUTTON));

		// Ensure there are no images or thumbnails in the workspace.
		int amountOfImagesOnDisplay = GameObject.FindGameObjectsWithTag ("DisplayImage").Length;
		Assert.AreEqual(amountOfImagesOnDisplay, 0);
		int amountOfThumbnailsOnDisplay = GameObject.FindGameObjectsWithTag ("Thumbnail").Length;
		Assert.AreEqual (amountOfThumbnailsOnDisplay, 0);
	}


	// This test checks the case where the "Right" button is pressed when there are no images.
	[Ignore("Failing test... Handling not built in yet.")]
	[Category ("Left/Right")]
	public void PrevButtonNoImages(){
		Dashboard dashboard = new Dashboard ();
		Display testDisplay = new Display ();
		dashboard.display = testDisplay;

/*
		try{
			Display.VRButtonClicked ("Right");
		} 
		catch (Exception e){
			Assert.AreEqual (NullReferenceException, e);
		}
*/
		// Assert that a NullReferenceException is thrown when button is clicked "Right"
		Assert.Throws<NullReferenceException>( () => testDisplay.VRButtonClicked (ButtonType.RIGHT_BUTTON));

		int amountOfImagesOnDisplay = GameObject.FindGameObjectsWithTag ("DisplayImage").Length;
		Assert.AreEqual(amountOfImagesOnDisplay, 0);

		int amountOfThumbnailsOnDisplay = GameObject.FindGameObjectsWithTag ("Thumbnail").Length;
		Assert.AreEqual (amountOfThumbnailsOnDisplay, 0);
	}
}

