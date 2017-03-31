using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

[TestFixture]
[Category("Unit Tests")]
public class TrayTests : MonoBehaviour
{

    [Test]
    public void TestUpdateTray()
    {
        GameObject trayManager = new GameObject();
        trayManager.AddComponent<Display>();

        GameObject trayObject = new GameObject();
        trayObject.AddComponent<Tray>();

        GameObject thumbObject = new GameObject();
        thumbObject.AddComponent<Thumbnail>();
        thumbObject.AddComponent<SpriteRenderer>();

        GameObject buttonPrefab = new GameObject();
        buttonPrefab.AddComponent<VRButton>();

        GameObject textObject = new GameObject();
        textObject.AddComponent<TextMesh>();
        textObject.transform.SetParent(buttonPrefab.transform);

        trayObject.GetComponent<Tray>().thumbnailPrefab = thumbObject;
        trayObject.GetComponent<Tray>().buttonPrefab = buttonPrefab.GetComponent<VRButton>();
        trayObject.GetComponent<Tray>().manager = trayManager.GetComponent<Display>();
        trayObject.GetComponent<Tray>().trayNumColumns = 3;

        Texture2D tex1 = new Texture2D(100, 100);
        Texture2D tex2 = new Texture2D(100, 100);
        Texture2D tex3 = new Texture2D(100, 100);

        List<Texture2D> textures = new List<Texture2D>();

        textures.Add(tex1);
        textures.Add(tex2);
        textures.Add(tex3);

        trayObject.GetComponent<Tray>().Setup(tex1);
        trayObject.GetComponent<Tray>().UpdateTray(tex1);
        trayObject.GetComponent<Tray>().UpdateTray(tex2);
        trayObject.GetComponent<Tray>().UpdateTray(tex3);
        // The tray should have 3 thumbnails in its list
        Assert.AreEqual(3, trayObject.GetComponent<Tray>().GetThumbnails().Count, "The list of thumbnails in the tray is empty.");
    }
}
