using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
//using UnityEngine.Assertions;
using NUnit.Framework;

[TestFixture]
[Category("Unit Tests")]
public class FileBrowserTests{

	[Test]
    public void CreateButtonsTest()
    {
		if (Application.isEditor) {
			GameObject fileBrowser = new GameObject ();
			fileBrowser.AddComponent<FileBrowser> ();
			FileBrowser newFileBrowser = fileBrowser.GetComponent<FileBrowser> ();

			// Testing the functions GetCurrentDirectories() and GetCurrentFiles() can retrieve paths
			newFileBrowser.SetCurrentDirectory (Directory.GetCurrentDirectory ());
			newFileBrowser.GetCurrentDirectories ();
			newFileBrowser.GetCurrentFiles ();
			//Assert.AreNotEqual(0, newFileBrowser.GetListOfFilePaths().Count);
			Assert.AreNotEqual (0, newFileBrowser.GetListOfDirectoryPaths ().Count);
		} else {
			Assert.Pass ();
		}
	
    }
}
