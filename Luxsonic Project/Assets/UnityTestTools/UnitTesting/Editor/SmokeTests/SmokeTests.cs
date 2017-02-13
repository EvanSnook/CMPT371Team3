using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using NUnit.Framework;

[TestFixture]
public class SmokeTests {

	[Test]
	public void OVRCheck()
	{

		String addPath = "Assets/OVR";
		String OVRPath = Path.Combine (getAssetPath(), addPath);
		Console.WriteLine (OVRPath);
		if (Directory.Exists (OVRPath)) {
			Assert.Pass ();
		} else {
			Assert.Fail ();
		}
	}


	[Test]
	public void OVRAvatarCheck()
	{

		String addPath = "Assets/OvrAvatar";
		String OVRPath = Path.Combine (getAssetPath(), addPath);
		Console.WriteLine (OVRPath);
		if (Directory.Exists (OVRPath)) {
			Assert.Pass ();
		} else {
			Assert.Fail ();
		}
	}


	private String getAssetPath(){
		String absPath = Directory.GetCurrentDirectory();
		String relPath = "../../../";
		String newString = Path.Combine (absPath, relPath);
		String myNewString = Path.GetFullPath(newString);
		return myNewString;
	}

}
