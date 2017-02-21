using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

[TestFixture]
[Category("Unit Tests")]
public class TrayTests : MonoBehaviour {

    [Test]
    public void TestUpdateTray()
    {
        GameObject trayManager = new GameObject();
        trayManager.AddComponent<ImageManager>();

        GameObject trayObject = new GameObject();
        trayObject.AddComponent<Tray>();

        GameObject thumbObject = new GameObject();
        thumbObject.AddComponent<Thumbnail>();
        thumbObject.AddComponent<SpriteRenderer>();

        trayObject.GetComponent<Tray>().thumbnail = thumbObject;
        trayObject.GetComponent<Tray>().manager = trayManager.GetComponent<ImageManager>();
        trayObject.GetComponent<Tray>().trayNumColumns = 3;

        Texture2D tex1 = new Texture2D(100, 100);
        Texture2D tex2 = new Texture2D(100, 100);
        Texture2D tex3 = new Texture2D(100, 100);

        List<Texture2D> textures = new List<Texture2D>();

        textures.Add(tex1);
        textures.Add(tex2);
        textures.Add(tex3);

        trayObject.GetComponent<Tray>().UpdateTray(textures);
        // The tray should have 3 thumbnails in its list
        Assert.AreEqual(3, trayObject.GetComponent<Tray>().GetThumbnails().Count, "The list of thumbnails in the tray is empty.");
    }
}
