using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// The Image Manager class manages images for the system.
/// This class contains a list of Texture2D elements that can be used with the display class.
/// It contains functions to add textures to the texture list, and create displays.
/// This class also displays an 'image tray' of images to select from.
/// </summary>
public class ImageManager : MonoBehaviour {

    public float trayStartX;        // The top left x coordinate for the tray to display
    public float trayStartZ;        // The top left z coordinate for the tray to display
    public float trayDepth;         // The depth of the tray
    public int trayNumColumns = 1;      // The number of columns for the tray to display, default 1
    public int trayNumRows = 1;         // The number of rows for the tray to display, default 1
    public float trayIncrementor = 1;   // The distance between each thumbnail
    public float trayThumbnailScale;// The scale for the images in the tray

    public float displayDepth;

    List<Texture2D> images = new List<Texture2D>(); // The list of images that have been loaded 
    List<GameObject> displays = new List<GameObject>();   // The list of displays currently in view
    List<GameObject> thumbnails = new List<GameObject>();   // The list of thumbnails being displayed in the tray

    public GameObject thumbnail;    // The object to use as a thumbnail
    public GameObject displayObj;   // The object to use as a display

    /// <summary>
    /// Add an image to the list of loaded images
    /// </summary>
    /// <param name="image">The texture for the image to add</param>
    public void AddImage(Texture2D image)
    {
        Assert.IsNotNull(image, "Image passed into ImageManager is null");
        images.Add(image);
        CreateTray();
    }

    /// <summary>
    /// Create and display the tray of thumbnail images
    /// </summary>
    public void CreateTray()
    {
        // Destroy the current tray  TODO: Make more efficient
        foreach (GameObject t in thumbnails)
        {
            Destroy(t);
        }

        thumbnails.Clear(); // Clear the list so we can recalculate the thumbnails
        float x = trayStartX;
        float z = trayStartZ;
        // For each image in the list, create a thumbnail and display it in the tray
        foreach (Texture2D image in images)
        {
            if(Mathf.Abs(x - trayStartX)>= trayNumColumns * trayIncrementor)
            {
                x = trayStartX;
                z -= trayIncrementor;
            }
            x += trayIncrementor;

            if( Mathf.Abs(z - trayStartZ) >= trayNumRows * trayIncrementor && x >= trayNumRows * trayIncrementor)
            {
                break;
            }
            Debug.Log("Going to instantiate thumb");
            GameObject newThumb = Instantiate(thumbnail, new Vector3(x, z, trayDepth), new Quaternion(0, 0, 0, 0));
            Debug.Log("Instantiated new thumb");
            newThumb.GetComponent < SpriteRenderer >().sprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f));
            newThumb.transform.localScale = new Vector3(trayThumbnailScale, trayThumbnailScale, 0);
            newThumb.GetComponent<Thumbnail>().manager = this.gameObject;
            newThumb.GetComponent<Thumbnail>().image = image;
            thumbnails.Add(newThumb);
        }
    }

    /// <summary>
    /// Instantiate a new display in the space at the center of the user's view
    /// </summary>
    public void CreateDisplay(Texture2D image) {
        Assert.IsNotNull(image, "Creating new display from ImageManager image was null");
        Vector3 trans = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane));
        trans.z = displayDepth;
        GameObject newDisp = Instantiate(displayObj, trans, new Quaternion(0,0,0,0));
        Display disp = newDisp.GetComponent<Display>();
        disp.NewDisplay(image);
        displays.Add(newDisp);  
    }

    /// <summary>
    /// Return the list of images in the manager
    /// </summary>
    /// <returns>A list of Texture2D elements</returns>
    public List<Texture2D> GetImages()
    {
        return this.images;
    }

    /// <summary>
    /// Return the list of thumbnails in the manager
    /// </summary>
    /// <returns>A list of Thumbnail objects</returns>
    public List<GameObject> GetThumbnails()
    {
        return this.thumbnails;
    }

    /// <summary>
    /// Returnt the list of displays in the manager
    /// </summary>
    /// <returns>A list of Display ojects</returns>
    public List<GameObject> GetDisplays()
    {
        return this.displays;
    }

}
