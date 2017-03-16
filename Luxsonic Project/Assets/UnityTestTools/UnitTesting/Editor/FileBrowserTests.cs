using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Assertions;
using NUnit.Framework;

[TestFixture]
[Category("Unit Tests")]
public class FileBrowserTests{

	[Test]
    public void CreateButtonsTest()
    {
        GameObject fileBrowser = new GameObject();
        fileBrowser.AddComponent<FileBrowser1>();
        FileBrowser1 newFileBrowser = fileBrowser.GetComponent<FileBrowser1>();

        // Testing the functions GetCurrentDirectories() and GetCurrentFiles() can retrieve paths
        newFileBrowser.SetCurrentDirectory("C:\\");
        newFileBrowser.GetCurrentDirectories();
        newFileBrowser.GetCurrentFiles();
        Assert.AreNotEqual(0, newFileBrowser.GetListOfFilePaths().Count);
        Assert.AreNotEqual(0, newFileBrowser.GetListOfDirectoryPaths().Count);



        //newFileBrowser.
    }
}
