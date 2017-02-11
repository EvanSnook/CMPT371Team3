using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ImageManager : MonoBehaviour {

    List<Texture2D> images = new List<Texture2D>();
    List<Display> displays = new List<Display>();
    List<GameObject> thumbnails = new List<GameObject>();

    public GameObject thumbnail;

    public void AddImages(Texture2D image)
    {
        Assert.IsNotNull(image, "Image passed into ImageManager is null");
        images.Add(image);
    }

    public void CreateTray()
    {
        thumbnails.Clear();
        int x = 0;
        int z = 0;
        int incrementor = 50;
        foreach (Texture2D image in images)
        {
            if(x>= 100)
            {
                x = 0;
                z += incrementor;
            }
            x += incrementor;
            if( z>=100 && x >= 100)
            {
                break;
            }
            Instantiate(thumbnail, new Vector3(x, 0, z), new Quaternion(0, 0, 0, 0));
            thumbnail.GetComponent < SpriteRenderer >().sprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f));
            thumbnails.Add(thumbnail);
        }
    }
    public void CreateDisplay() {
        //Display currentDisplay = new Display(Texture2D);
        //FirePoint.transform.position = Camera.main.ScreenToWorldPoint(Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane));
    }

}
