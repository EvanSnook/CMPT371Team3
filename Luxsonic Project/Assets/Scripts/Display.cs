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
public class Display : MonoBehaviour, IVRButton
{

    // The depth at which the copy will be placed in front of the user
    public float copyDepth;

    // The list of images that have been loaded 
    List<Texture2D> images = new List<Texture2D>();

    // The list of display images that the user will be able to scroll through
    LinkedList<GameObject> displayImages = new LinkedList<GameObject>();

    // The list of copies currently in view
    List<GameObject> copies = new List<GameObject>();

    // The object prefab to use as a copy
    public GameObject copyPrefab;
    // The object prefab to use as the tray
    public GameObject trayPrefab;
    // The object prefab to use as the display images 
    public GameObject displayImagePrefab;

    // Indicates whether a tray has already been created in the scene
    public bool trayCreated = false;
    // The tray that will be exhibited in the scene
    private Tray tray;

    // The position to create the tray object
    public Vector3 trayPosition;
    // The rotation to spawn the tray at
    public Vector3 trayRotation;

    // An array containing the positions of the display images
    // currently set to the minimal value, real value is set within Unity editor
    public Vector3[] displayImagePositions = new Vector3[1];
    public Vector3[] displayImageRotations = new Vector3[1];

    // The button prefab that will be used for all buttons
    public VRButton buttonPrefab;

    //Left and right buttons to scroll through the images in Display
    private VRButton leftScrollButton;
    private VRButton rightScrollButton;

    // Define positions for the scroll buttons
    public Vector3 leftScrollPosition;
    public Vector3 leftScrollRotation;
    public Vector3 rightScrollPosition;
    public Vector3 rightScrollRotation;

    // Indicates whether the scroll buttons are visible to the user	
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
        Assert.IsNotNull(image, "Image passed into Display is null");
        images.Add(image);

        // Create a game object to display the new image on
        GameObject displayImage = Instantiate(displayImagePrefab, Vector3.zero, Quaternion.Euler(Vector3.zero));
        displayImage.transform.parent = gameObject.transform;
        displayImage.SetActive(false);
        displayImage.GetComponent<SpriteRenderer>().sprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height),
            new Vector2(0.5f, 0.5f));

        displayImages.AddLast(displayImage);

        // If the number of display images is less or equal to the length of the display image positions array
        // redraw the display iamges, else create the scroll bar buttons
        if (displayImages.Count <= displayImagePositions.Length)
        {
            redrawDisplayImages();
        }
        else
        {
            if (!this.scrollButtonsVisible)
            {
                createScrollButtons();
                this.scrollButtonsVisible = true;
            }
        }

        CreateTray(image);
    }


    /// <summary>
    /// Function createScrollButtons() will create the left and right scroll buttons to
    /// browse through all images in the Display
    /// Pre:: there are more display images than the length of the display image positions array
    /// Post:: instantiation of left and right scroll buttons and added to the hierarchy
    /// Return: nothing
    /// </summary>
    private void createScrollButtons()
    {
        // Create the left scroll button
        CreateScrollButton("Left", this.leftScrollButton, this.leftScrollPosition, this.leftScrollRotation);
        // Create the right scroll button
        CreateScrollButton("Right", this.rightScrollButton, this.rightScrollPosition, this.rightScrollRotation);
    }


    /// <summary>
    /// CreateScrollButton will create a single instance of a scroll button
    /// Pre:: buttonPrefab must not be null
    /// Post:: creation of a new scroll button
    /// Return:: nothing
    /// </summary>
    /// <param name="buttonName">The name of the button "Left" or "Right"</param>
    /// <param name="button">the button attribute we are instantiating</param>
    /// <param name="position">position of the button</param>
    /// <param name="rotation">rotation of the button</param>
    public void CreateScrollButton(string buttonName, VRButton button, Vector3 position, Vector3 rotation)
    {
        button = Instantiate(buttonPrefab, position,
            Quaternion.Euler(rotation));
        button.transform.parent = gameObject.transform;

        button.textObject = button.GetComponentInChildren<TextMesh>();
        button.textObject.text = buttonName;

        button.name = buttonName;
        button.manager = this.gameObject;
    }

    /// <summary>
    /// Function CreateTray() creates and displays the tray of thumbnails
    /// The tray exhibits all images that are available to the user to cycle through in the display
    /// Pre:: nothing
    /// Post: a new Tray instantiated and added to the hierarchy
    /// Return:: nothing
    /// </summary>
    public void CreateTray(Texture2D image)
    {
        // If a Tray does not exist already, create a Tray
        if (!trayCreated)
        {
            GameObject newTray = Instantiate(trayPrefab, trayPosition, Quaternion.Euler(trayRotation));
            newTray.transform.parent = gameObject.transform;

            this.tray = newTray.GetComponent<Tray>();
            this.tray.manager = this;
            this.tray.Setup(image);
            this.trayCreated = true;
        }
        else
        {
            this.tray.UpdateTray(image);
        }
    }

    /// <summary>
    /// Function CreateCopy will instantiate a new Copy in the space at the center of the user's view
	/// A Copy is the object that a user can manipulate and move within the workspace
    /// Pre:: Texture2D image
    /// Post:: a new Copy is instantiated and added to the hierarchy
    /// Return:: nothing
    /// </summary>
    public void CreateCopy(Texture2D image)
    {
        Assert.IsNotNull(image, "Creating new Copy from Display image was null");

        // Retrieve the center point of the camera view and instantiate a Copy on that location
        Vector3 trans = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2,
            Camera.main.nearClipPlane));
        trans.z = copyDepth;
        GameObject newCop = Instantiate(copyPrefab, trans, new Quaternion(0, 0, 0, 0));
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
    /// Pre:: there are more display images than the length of display iamges positions array
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
	/// Pre:: there are more display images than the length of display iamges positions array
    /// Post:: positions and activation of GameObjects in displayImages is changed.
    /// Return:: nothing
    /// </summary>
    private void ScrollRight()
    {
        GameObject temp = displayImages.Last.Value;
        LinkedListNode<GameObject> t2 = displayImages.First;
        // Start from the first image and find the last image being displayed
        for (int i = 1; i < displayImagePositions.Length; i++)
        {
            t2 = t2.Next;
        }
        t2.Value.SetActive(false);
        displayImages.RemoveLast();
        displayImages.AddFirst(temp);
        redrawDisplayImages();
    }

    /// <summary>
    /// Draw the scrolling display images from the list of display images
	/// The number of display images drawn is determined by the length of the display
	/// image positions array
	/// Pre:: there are display images to be drawn
	/// Post:: display images are drawn in the workspace
	/// Return:: Nothing
    /// </summary>
    private void redrawDisplayImages()
    {
        int i = 0;
        // for each display image, if there are less than the display image positions array length
        // draw the image in the specified position and set it active
        foreach (GameObject img in displayImages)
        {
            if (i < displayImagePositions.Length)
            {
                img.gameObject.GetComponent<Transform>().position = displayImagePositions[i];
                img.gameObject.GetComponent<Transform>().rotation = Quaternion.Euler(displayImageRotations[i]);
                img.SetActive(true);
                i++;
            }
            else
            {
                break;
            }
        }
    }
}

