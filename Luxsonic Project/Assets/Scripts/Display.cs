using System;
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
public class Display : MonoBehaviour, IVRButton {

    public float displayDepth;

    List<Texture2D> images = new List<Texture2D>(); // The list of images that have been loaded 
    LinkedList<GameObject> displayImages = new LinkedList<GameObject>();

    List<GameObject> copies = new List<GameObject>();   // The list of displays currently in view
    
    public GameObject copyObj;   // The object to use as a display
    public GameObject trayObj;      // The object to use as the tray
    public GameObject displayImageObj; // The object to display display images with

    public bool trayCreated = false;
    private Tray tray;

    public Vector3 trayPosition;    // The position to create the tray object
    public Vector3 trayRotation;    // The rotation to spawn the tray at

    public Vector3[] displayImagePositions = new Vector3[1];

    public VRButton button;

    // Define positions for the scroll buttons
    public Vector3 leftScrollPosition;
    public Vector3 leftScrollRotation;
    public Vector3 rightScrollPosition;
    public Vector3 rightScrollRotation;

    private bool scrollButtonsVisible = false;

    /// <summary>
    /// Add an image to the list of loaded images
    /// </summary>
    /// <param name="image">The texture for the image to add</param>
    public void AddImage(Texture2D image)
    {
        Assert.IsNotNull(image, "Image passed into ImageManager is null");
        images.Add(image);

        // Create a game object to display the new image on
        GameObject displayImage = Instantiate(displayImageObj, Vector3.zero, Quaternion.Euler(Vector3.zero));
        displayImage.SetActive(false);
        displayImage.GetComponent<SpriteRenderer>().sprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f));
        
        displayImages.AddLast(displayImage);

        
        if (displayImages.Count <= displayImagePositions.Length)
        {
            redrawDisplayImages();
        }
        else
        {
            if (!this.scrollButtonsVisible)
            {
                displayScrollButtons();
                this.scrollButtonsVisible = true;
            }
        }

        CreateTray();
    }

    private void displayScrollButtons()
    {
        // Create the left scroll button
        VRButton leftScrollButton = Instantiate(button, leftScrollPosition,
            Quaternion.Euler(leftScrollRotation));

        leftScrollButton.name = "Left";
        leftScrollButton.manager = this.gameObject;

        // Create the right scroll button
        VRButton rightScrollButton = Instantiate(button, rightScrollPosition,
            Quaternion.Euler(rightScrollRotation));

        rightScrollButton.name = "Right";
        rightScrollButton.manager = this.gameObject;
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


    public void VRButtonClicked(string button)
    {
        switch (button)
        {
            case "Left":
                scrollLeft();
                break;
            case "Right":
                scrollRight();
                break;
        }
    }

    /// <summary>
    /// Scroll the display left
    /// </summary>
    private void scrollLeft()
    {
        GameObject temp = displayImages.First.Value;
        temp.SetActive(false);
        displayImages.RemoveFirst();
        displayImages.AddLast(temp);
        redrawDisplayImages();
    }

    /// <summary>
    /// Scroll the display Right
    /// </summary>
    private void scrollRight()
    {
        GameObject temp = displayImages.Last.Value;
        LinkedListNode<GameObject> t2 = displayImages.First;
        for(int i = 1; i < displayImagePositions.Length; i++)
        {
            t2 = t2.Next;
        }
        t2.Value.SetActive(false);
        displayImages.RemoveLast();
        displayImages.AddFirst(temp);
        redrawDisplayImages();
    }

    /// <summary>
    /// Draw the scrolling display images
    /// </summary>
    private void redrawDisplayImages()
    {
        int i = 0;
        foreach (GameObject img in displayImages)
        {
            if (i < displayImagePositions.Length)
            {
                img.gameObject.GetComponent<Transform>().position = displayImagePositions[i];
                img.SetActive(true);
                i++;
            }else
            {
                break;
            }
        }
    }
}
