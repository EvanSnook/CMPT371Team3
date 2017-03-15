using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        newFileBrowser.GetListOfDirectoryPaths().Add("C://");

        //newFileBrowser.
    }
}
