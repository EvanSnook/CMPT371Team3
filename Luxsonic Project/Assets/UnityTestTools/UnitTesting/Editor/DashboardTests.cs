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

		GameObject dispObj = new GameObject();
		dispObj.AddComponent<Display>();

		GameObject buttonObj = new GameObject();
		TextMesh btnMesh = new TextMesh ();
		buttonObj.AddComponent<VRButton> ();
		buttonObj.AddComponent<TextMesh> ();
		dash.button = buttonObj.GetComponent<VRButton>();

		dash.Start ();


        Assert.Pass();
    }
}
