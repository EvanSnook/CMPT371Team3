using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

[TestFixture]
[Category("System Tests")]
public class SystemTests : MonoBehaviour {

	[Test]
	public void copyExists()
	{
		Dashboard dashboard = new Dashboard ();
		Display testDisplay = new Display ();
		dashboard.display = testDisplay;

		//dashboard.dissplay = GameObject.FindGameObjectWithTag("DisplayImage").GetComponent<Display>();

		dashboard.VRButtonClicked ("Load"); // Load in the default image.
		testDisplay.CreateCopy(testDisplay.GetImages()[0]);								// Creating the first copy

		/* ELEGANT, LONG SOLUTION 
			GameObject[] copies = new GameObject[1];
			copies = GameObject.FindGameObjectsWithTag ("Copy");
			Assert.AreEqual(copies.GetLength, 1);
		*/

		//Debug.Log (GameObject.FindGameObjectsWithTag ("Copy").GetLength);

		/* ONE LINER SOL'N */
		Assert.AreEqual(GameObject.FindGameObjectsWithTag ("Copy").GetLength, 1);		//Check that there is a copy created. 
	}

	[Test]
	public void copyDifferentThanDisplay()
	{
		Dashboard dashboard = new Dashboard ();
		Display display = new Display ();

		dashboard.display = GameObject.FindGameObjectWithTag("DisplayImage").GetComponent<Display>();
		dashboard.VRButtonClicked ("Load"); // Load in the default images.
		display.CreateCopy(display.GetImages[0]); // Creating the first copy

		//Ensure image is same.... && Game object is different... 
		Assert (GameObject.FindGameObjectWithTag("DisplayImage").GetComponent<SpriteRenderer>().sprite == GameObject.FindGameObjectWithTag("Copy").GetComponent<SpriteRenderer>().sprite &&
			GameObject.FindGameObjectWithTag("DisplayImage") != GameObject.FindGameObjectsWithTag("Copy)"));
	}

	[Test]
	public void copyDifferentThanTray(){ 

	}
}


//do sprite comparison with display.images4