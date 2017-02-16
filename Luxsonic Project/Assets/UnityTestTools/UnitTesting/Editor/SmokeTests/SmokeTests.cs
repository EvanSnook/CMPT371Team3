﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;

[TestFixture]
public class SmokeTests {

    private TestContext testContextInstance;

    [Test]
	[Category("Dependencies")]
	public void OVRCheck(){
		String addPath = "Assets/OVR";
		String targetPath = Path.Combine (getAssetPath(), addPath);
		Console.WriteLine (targetPath);
        Assert.IsTrue(Directory.Exists(targetPath) && (Directory.GetDirectories(targetPath).Length > 0) && (Directory.GetFiles(targetPath).Length > 0));
	}


	[Test]
	[Category("Dependencies")]
	public void OVRAvatarCheck(){
		String addPath = "Assets/OvrAvatar";
		String targetPath = Path.Combine (getAssetPath (), addPath);
		Console.WriteLine (targetPath);
        Assert.IsTrue(Directory.Exists(targetPath) && (Directory.GetDirectories(targetPath).Length > 0) && (Directory.GetFiles(targetPath).Length > 0)) ;
	}

	[Test]
	[Category("Dependencies")]
	public void gamePadBundleCheck(){
		String addPath = "Assets/Plugins/OVRGamepad.bundle/Contents";
		String targetPath = Path.Combine (getAssetPath (), addPath);
		Console.WriteLine (targetPath);
        Assert.IsTrue(Directory.Exists(targetPath) && (Directory.GetDirectories(targetPath).Length > 0) && (Directory.GetFiles(targetPath).Length > 0)) ;
	}

	[Test]
	[Category("Dependencies")]
	public void OVRAvatarSettingsCheck(){
		String addPath = "Assets/Resources";
		String targetPath = Path.Combine (getAssetPath (), addPath);
		Console.WriteLine (targetPath);
        Assert.IsTrue(Directory.Exists(targetPath) && (Directory.GetFiles(targetPath).Length > 0)) ;
	}

	[Test]
	[Category("Dependencies")]
	public void imageEffectsCheck(){
		String addPath = "Assets/Standard Assets/Editor/ImageEffects";
		String targetPath = Path.Combine (getAssetPath (), addPath);
		Console.WriteLine (targetPath);
        Assert.IsTrue(Directory.Exists(targetPath) && (Directory.GetFiles(targetPath).Length > 0));
	}

	[Test]
	[Category("Dependencies")]
	public void EffectsCheck(){
		String addPath = "Assets/Standard Assets/Effects";
		String targetPath = Path.Combine (getAssetPath (), addPath);
		Console.WriteLine (targetPath);
        Assert.IsTrue(Directory.Exists(targetPath) && (Directory.GetDirectories(targetPath).Length > 0) && (Directory.GetFiles(targetPath).Length > 0)) ;
	}
		
	private String getAssetPath(){
		String absPath = Directory.GetCurrentDirectory ();
		String newString = Directory.GetParent(absPath).FullName;
		newString = Directory.GetParent(newString).FullName;
		newString = Directory.GetParent(newString).FullName;
		return newString;
	}

    // this test cannot pass because of priviledges in unity editor
	//[Test]
	//[Category("Scenes")]
	//public void defaultSceneCheck(){
    //    Assert.IsTrue(EditorSceneManager.GetSceneByBuildIndex(0).IsValid()) ;
	//}
	 
}
