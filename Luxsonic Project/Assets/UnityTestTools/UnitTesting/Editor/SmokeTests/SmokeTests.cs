using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using NUnit.Framework;
using System.IO;
using UnityEditor.SceneManagement;

public class SmokeTests : MonoBehaviour
{

    private TestContext testContextInstance;

    [Test]
    [Category("Dependencies")]
    public void OVRCheck()
    {
        String addPath = "Assets/OVR";
        String targetPath = Path.Combine(getAssetPath(), addPath);
        Assert.IsTrue(Directory.Exists(targetPath) && (Directory.GetDirectories(targetPath).Length > 0) && (Directory.GetFiles(targetPath).Length > 0));
    }


    [Test]
    [Category("Dependencies")]
    public void OVRAvatarCheck()
    {
        String addPath = "Assets/OvrAvatar";
        String targetPath = Path.Combine(getAssetPath(), addPath);
        Assert.IsTrue(Directory.Exists(targetPath) && (Directory.GetDirectories(targetPath).Length > 0) && (Directory.GetFiles(targetPath).Length > 0));
    }

    [Test]
    [Category("Dependencies")]
    public void gamePadBundleCheck()
    {
        String addPath = "Assets/Plugins/OVRGamepad.bundle/Contents";
        String targetPath = Path.Combine(getAssetPath(), addPath);
        Assert.IsTrue(Directory.Exists(targetPath) && (Directory.GetDirectories(targetPath).Length > 0) && (Directory.GetFiles(targetPath).Length > 0));
    }

    [Test]
    [Category("Dependencies")]
    public void OVRAvatarSettingsCheck()
    {
        String addPath = "Assets/Resources";
        String targetPath = Path.Combine(getAssetPath(), addPath);
        Assert.IsTrue(Directory.Exists(targetPath) && (Directory.GetFiles(targetPath).Length > 0));
    }

    [Test]
    [Category("Dependencies")]
    public void imageEffectsCheck()
    {
        String addPath = "Assets/Standard Assets/Editor/ImageEffects";
        String targetPath = Path.Combine(getAssetPath(), addPath);
        Assert.IsTrue(Directory.Exists(targetPath) && (Directory.GetFiles(targetPath).Length > 0));
    }

    [Test]
    [Category("Dependencies")]
    public void EffectsCheck()
    {
        String addPath = "Assets/Standard Assets/Effects";
        String targetPath = Path.Combine(getAssetPath(), addPath);
        Assert.IsTrue(Directory.Exists(targetPath) && (Directory.GetDirectories(targetPath).Length > 0) && (Directory.GetFiles(targetPath).Length > 0));
    }

    private String getAssetPath()
    {
        String absPath = Directory.GetCurrentDirectory();
        return absPath;
    }

    // this test cannot pass because of how cloud build and unity interact
    //[Test]
    //[Category("Scenes")]
    //public void defaultSceneCheck()
    //{
    //    Assert.IsTrue(EditorSceneManager.GetSceneByBuildIndex(0).IsValid());
    //}

}

