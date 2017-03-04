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

    //Left and right buttons to scroll through the images in Display
    private VRButton left;
    private VRButton right;

    // Define positions for the scroll buttons
    public Vector3 leftScrollPosition;
    public Vector3 leftScrollRotation;
    public Vector3 rightScrollPosition;
    public Vector3 rightScrollRotation;

    private bool scrollButtonsVisible = false;

    /// <summary>
    /// AddImage() will add an image to the list of loaded images.  It will also create a new
    /// displayImage and add it to the list.  It will create the Tray if it does not already exist
    /// and create the scroll bar if it is not currently present
    /// Pre:: image Texture2D to add
    /// Post:: creation of Tray, adds Texture2D to images list and adds new GameObject to displayImages
    /// list.
    /// Return:: nothing
    /// </summary>
    /// <param name="image">The texture for the image to add</param>
    public void AddImage(Texture2D image)
    {
        Assert.IsNotNull(image, "Image passed into ImageManager is null");
        images.Add(image);

        // Create a game object to display the new image on
        GameObject displayImage = Instantiate(displayImageObj, Vector3.zero, Quaternion.Euler(Vector3.zero));
        displayImage.transform.parent = gameObject.transform;
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


    /// <summary>
    /// Function displayScrollButtons() will create the left and right scroll buttons to
    /// browse through all images in the Display
    /// Pre:: nothing
    /// Post:: creation of left and rigfht scrol buttons
    /// Return: nothing
    /// </summary>
    private void displayScrollButtons()
    {
        // Create the left scroll button
        VRButton leftScrollButton = Instantiate(button, leftScrollPosition,
            Quaternion.Euler(leftScrollRotation));
        leftScrollButton.transform.parent = gameObject.transform;

        left = leftScrollButton;

        leftScrollButton.name = "Left";
        leftScrollButton.manager = this.gameObject;

        // Create the right scroll button
        VRButton rightScrollButton = Instantiate(button, rightScrollPosition,
            Quaternion.Euler(rightScrollRotation));
        rightScrollButton.transform.parent = gameObject.transform;

        right = rightScrollButton;

        rightScrollButton.name = "Right";
        rightScrollButton.manager = this.gameObject;

        
    }
    
    /// <summary>
    /// Function CreateTray() creates and displays the tray of thumbnail images
    /// Pre:: nothing
    /// Post: creation of new Tray
    /// Return:: nothing
    /// </summary>
    public void CreateTray()
    {
        if(!trayCreated)
        {
            GameObject newTray = Instantiate(trayObj, trayPosition, Quaternion.Euler(trayRotation));
            newTray.transform.parent = gameObject.transform;
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
    /// Function CreateCopy will instantiate a new Copy in the space at the center of the user's view
    /// Pre:: Texture2D image
    /// Post:: creation of a new copy
    /// Return:: nothing
    /// </summary>
    public void CreateCopy(Texture2D image) {
        Assert.IsNotNull(image, "Creating new Copy from Display image was null");
        Vector3 trans = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 
            Camera.main.nearClipPlane));
        trans.z = displayDepth;
        GameObject newCop = Instantiate(copyObj, trans, new Quaternion(0,0,0,0));
        Copy cop = newCop.GetComponent<Copy>();
        cop.NewCopy(image);
        copies.Add(newCop);  
    }

    /// <summary>
    /// GetImages() will return the list of images in the manager
    /// Pre:: nothing
    /// Post:: nothing
    /// Return:: List of Texture2D
    /// </summary>
    /// <returns>A list of Texture2D elements</returns>
    public List<Texture2D> GetImages()
    {
        return this.images;
    }

    

    /// <summary>
    /// GetCopies() will return the list of Copies in the manager
    /// Pre:: nothing
    /// Post: nothing
    /// </summary>
    /// <returns>A list of Display ojects</returns>
    public List<GameObject> GetCopies()
    {
        return this.copies;
    }

    /// <summary>
    /// Function VRButtonClicked() will take in a string and perform and action
    /// based on the string given to it.
    /// </summary>
    /// <param name="button"></param>
    public void VRButtonClicked(string button)
    {
        switch (button)
        {
            case "Left":
                ScrollLeft();
                break;
            case "Right":
                ScrollRight();
                break;
        }
    }

    /// <summary>
    /// ScrollLeft() will shift the images displayed to the user in the displayImages
    /// list to the left.
    /// Pre:: nothing
    /// Post:: positions and activation of GameObjects in displayImages is changed.
    /// Return:: nothing
    /// </summary>
    private void ScrollLeft()
    {
        GameObject temp = displayImages.First.Value;
        temp.SetActive(false);
        displayImages.RemoveFirst();
        displayImages.AddLast(temp);
        redrawDisplayImages();
    }

    /// <summary>
    /// ScrollRightt() will shift the images displayed to the user in the displayImages
    /// list to the right.
    /// Pre:: nothing
    /// Post:: positions and activation of GameObjects in displayImages is changed.
    /// Return:: nothing
    /// </summary>
    private void ScrollRight()
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
