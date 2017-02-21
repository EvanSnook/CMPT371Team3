using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// The Display class manages images for the system.
/// This class contains a list of Texture2D elements that can be used with the Copy class.
/// It contains functions to add textures to the texture list, and create Copies.
/// This class also displays an 'image tray' of images to select from.
/// </summary>
public class Display : MonoBehaviour {

    public float displayDepth;

    List<Texture2D> images = new List<Texture2D>(); // The list of images that have been loaded 
    List<GameObject> copies = new List<GameObject>();   // The list of displays currently in view
    
    public GameObject copyObj;   // The object to use as a display
    public GameObject trayObj;      // The object to use as the tray
    public bool trayCreated = false;
    private Tray tray;

    public Vector3 trayPosition;    // The position to create the tray object
    public Vector3 trayRotation;    // The rotation to spawn the tray at

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
        if(!trayCreated)
        {
            GameObject newTray = Instantiate(trayObj, trayPosition, Quaternion.Euler(trayRotation));
            this.tray = newTray.GetComponent<Tray>();
            this.tray.manager = this;
            this.tray.UpdateTray(this.images);
            this.trayCreated = true;
        }else
        {
            this.tray.UpdateTray(this.images);
        }
    }

    /// <summary>
    /// Instantiate a new display in the space at the center of the user's view
    /// </summary>
    public void CreateCopy(Texture2D image) {
        Assert.IsNotNull(image, "Creating new Copy from Display image was null");
        Vector3 trans = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane));
        trans.z = displayDepth;
        GameObject newCop = Instantiate(copyObj, trans, new Quaternion(0,0,0,0));
        Copy cop = newCop.GetComponent<Copy>();
        cop.NewCopy(image);
        copies.Add(newCop);  
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
    /// Returnt the list of displays in the manager
    /// </summary>
    /// <returns>A list of Display ojects</returns>
    public List<GameObject> GetCopies()
    {
        return this.copies;
    }

}
