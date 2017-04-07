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

        GameObject thumbOutline = new GameObject();
        thumbOutline.AddComponent<SpriteRenderer>();
        thumbOutline.transform.SetParent(thumbObject.transform);

        GameObject buttonPrefab = new GameObject();
        buttonPrefab.AddComponent<VRButton>();

        GameObject textObject = new GameObject();
        textObject.AddComponent<TextMesh>();
        textObject.transform.SetParent(buttonPrefab.transform);

        trayObject.GetComponent<Tray>().thumbnailPrefab = thumbObject;
        trayObject.GetComponent<Tray>().buttonPrefab = buttonPrefab;
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

    [Test]
    public void TestScrolling()
    {
        GameObject trayManager = new GameObject();
        trayManager.AddComponent<Display>();

        GameObject trayObject = new GameObject();
        trayObject.AddComponent<Tray>();

        GameObject thumbObject = new GameObject();
        thumbObject.AddComponent<Thumbnail>();
        thumbObject.AddComponent<SpriteRenderer>();

        GameObject thumbOutline = new GameObject();
        thumbOutline.AddComponent<SpriteRenderer>();
        thumbOutline.transform.SetParent(thumbObject.transform);

        GameObject buttonPrefab = new GameObject();
        buttonPrefab.AddComponent<VRButton>();

        GameObject textObject = new GameObject();
        textObject.AddComponent<TextMesh>();
        textObject.transform.SetParent(buttonPrefab.transform);

        trayObject.GetComponent<Tray>().thumbnailPrefab = thumbObject;
        trayObject.GetComponent<Tray>().buttonPrefab = buttonPrefab;
        trayObject.GetComponent<Tray>().manager = trayManager.GetComponent<Display>();
        trayObject.GetComponent<Tray>().trayNumColumns = 3;

        Texture2D tex1 = new Texture2D(100, 100);
        Texture2D tex2 = new Texture2D(100, 100);
        Texture2D tex3 = new Texture2D(100, 100);
        Texture2D tex4 = new Texture2D(100, 100);
        Texture2D tex5 = new Texture2D(100, 100);
        Texture2D tex6 = new Texture2D(100, 100);
        Texture2D tex7 = new Texture2D(100, 100);
        Texture2D tex8 = new Texture2D(100, 100);
        Texture2D tex9 = new Texture2D(100, 100);
        Texture2D tex10 = new Texture2D(100, 100);

        List<Texture2D> textures = new List<Texture2D>();

        textures.Add(tex1);
        textures.Add(tex2);
        textures.Add(tex3);
        textures.Add(tex4);
        textures.Add(tex5);
        textures.Add(tex6);
        textures.Add(tex7);
        textures.Add(tex8);
        textures.Add(tex9);
        textures.Add(tex10);

        trayObject.GetComponent<Tray>().Setup(tex1);
        trayObject.GetComponent<Tray>().UpdateTray(tex1);
        trayObject.GetComponent<Tray>().UpdateTray(tex2);
        trayObject.GetComponent<Tray>().UpdateTray(tex3);
        trayObject.GetComponent<Tray>().UpdateTray(tex4);
        trayObject.GetComponent<Tray>().UpdateTray(tex5);
        trayObject.GetComponent<Tray>().UpdateTray(tex6);
        trayObject.GetComponent<Tray>().UpdateTray(tex7);
        trayObject.GetComponent<Tray>().UpdateTray(tex8);
        trayObject.GetComponent<Tray>().UpdateTray(tex9);
        trayObject.GetComponent<Tray>().UpdateTray(tex10);


        trayObject.GetComponent<Tray>().TestScroll("up");
        trayObject.GetComponent<Tray>().TestScroll("down");

    }

    [Test]
    public void TestHighlightTray()
    {
        GameObject trayManager = new GameObject();
        trayManager.AddComponent<Display>();

        GameObject trayObject = new GameObject();
        trayObject.AddComponent<Tray>();

        GameObject thumbObject = new GameObject();
        thumbObject.AddComponent<Thumbnail>();
        thumbObject.AddComponent<SpriteRenderer>();

        GameObject thumbOutline = new GameObject();
        thumbOutline.AddComponent<SpriteRenderer>();
        thumbOutline.transform.SetParent(thumbObject.transform);

        GameObject buttonPrefab = new GameObject();
        buttonPrefab.AddComponent<VRButton>();

        GameObject textObject = new GameObject();
        textObject.AddComponent<TextMesh>();
        textObject.transform.SetParent(buttonPrefab.transform);

        trayObject.GetComponent<Tray>().thumbnailPrefab = thumbObject;
        trayObject.GetComponent<Tray>().buttonPrefab = buttonPrefab;
        trayObject.GetComponent<Tray>().manager = trayManager.GetComponent<Display>();
        trayObject.GetComponent<Tray>().trayNumColumns = 3;

        Texture2D tex1 = new Texture2D(100, 100);
        Texture2D tex2 = new Texture2D(100, 100);
        Texture2D tex3 = new Texture2D(100, 100);
        Texture2D tex4 = new Texture2D(100, 100);
        Texture2D tex5 = new Texture2D(100, 100);
        Texture2D tex6 = new Texture2D(100, 100);
        Texture2D tex7 = new Texture2D(100, 100);
        Texture2D tex8 = new Texture2D(100, 100);
        Texture2D tex9 = new Texture2D(100, 100);
        Texture2D tex10 = new Texture2D(100, 100);

        List<Texture2D> textures = new List<Texture2D>();

        textures.Add(tex1);
        textures.Add(tex2);
        textures.Add(tex3);
        textures.Add(tex4);
        textures.Add(tex5);
        textures.Add(tex6);
        textures.Add(tex7);
        textures.Add(tex8);
        textures.Add(tex9);
        textures.Add(tex10);

        trayObject.GetComponent<Tray>().Setup(tex1);
        trayObject.GetComponent<Tray>().UpdateTray(tex1);
        trayObject.GetComponent<Tray>().UpdateTray(tex2);
        trayObject.GetComponent<Tray>().UpdateTray(tex3);
        trayObject.GetComponent<Tray>().UpdateTray(tex4);
        trayObject.GetComponent<Tray>().UpdateTray(tex5);
        trayObject.GetComponent<Tray>().UpdateTray(tex6);
        trayObject.GetComponent<Tray>().UpdateTray(tex7);
        trayObject.GetComponent<Tray>().UpdateTray(tex8);
        trayObject.GetComponent<Tray>().UpdateTray(tex9);
        trayObject.GetComponent<Tray>().UpdateTray(tex10);

        Texture2D[] texturesToHighlight = new Texture2D[3];
        texturesToHighlight[0] = tex1;
        texturesToHighlight[1] = tex2;
        texturesToHighlight[2] = tex3;


        trayObject.GetComponent<Tray>().HighlightTray(texturesToHighlight);

        foreach (GameObject thumb in trayObject.GetComponent<Tray>().GetThumbnails())
        {
            if (thumb.GetComponent<Thumbnail>().image == tex1 || thumb.GetComponent<Thumbnail>().image == tex2 || thumb.GetComponent<Thumbnail>().image == tex3)
            {
                Assert.IsTrue(thumb.transform.GetChild(0).transform.gameObject.activeSelf);
            }
            else
            {
                Assert.IsFalse(thumb.transform.GetChild(0).transform.gameObject.activeSelf);
            }
        }
    }
}
