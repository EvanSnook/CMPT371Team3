using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class SystemTests : MonoBehaviour {


	[Test]
	[Category("copyImage")]
	public void OVRCheck()
	{
		Dashboard dashboard = new Dashboard ();
		Display display = new Display ();
		dashboard.display = GameObject.FindGameObjectWithTag("ImageManager").GetComponent<Display>();
		dashboard.VRButtonClicked ("Load");
		Copy[] copies = GameObject.FindGameObjectsWithTag("Copy");
			

		display.AddImage(
	}


}
