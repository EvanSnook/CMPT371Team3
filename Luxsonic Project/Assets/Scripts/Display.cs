using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

using buttons;


/// <summary>
/// The Display class manages images for the system.
/// This class contains a list of Texture2D elements that can be used with the Copy class.
/// It contains functions to add textures to the texture list, and create Copies.
/// This class also displays an 'image tray' of images to select from.
/// </summary>
public class Display : MonoBehaviour, IVRButton
{
    // Holds the attributes for each button to be instantiated
    [SerializeField]
    private List<ButtonAttributes> buttonList = new List<ButtonAttributes>();

    // Holds references to the instantiated buttons
    private List<GameObject> buttonObjects = new List<GameObject>();

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
	public GameObject buttonPrefab;

	// Indicates whether the scroll buttons are visible to the user	
	private bool scrollButtonsVisible = false;

	// Current ID for copies whose textures do not have names
	private int copyId = 0;

    private void Start()
    {
        foreach (ButtonAttributes attributes in buttonList)
        {
            buttonObjects.Add(CreateButton(attributes, buttonPrefab));
        }
        ToggleScrollButtons();
    }


	/// <summary>
	/// AddImage() will add an image to the list of loaded images.  It will also create a new
	/// displayImage and add it to the list.  It will create the Tray if it does not already exist
	/// and create the scroll bar if it is not currently present.
	/// </summary>
	/// <param name="image">The texture for the image to add</param>
    /// <param name="patientInfo">A dictionary of patient info keyed by field</param>
	/// <pre>Image Texture2D to add</pre>
	/// <post> Creation of tray, adds Texture2D to images list and adds new GameObject to displayImages</post>
	public void AddImage(Texture2D image, Dictionary<string,string> patientInfo)
	{
		Assert.IsNotNull(image, "Image passed into Display is null");
		images.Add(image);

		// Create a game object to display the new image on
		GameObject displayImage = Instantiate(displayImagePrefab, Vector3.zero, Quaternion.Euler(Vector3.zero));
		displayImage.transform.parent = gameObject.transform;
		displayImage.SetActive(false);
		displayImage.GetComponent<SpriteRenderer>().sprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height),
			new Vector2(0.5f, 0.5f));

		foreach (Transform t in displayImage.gameObject.transform.GetChild(0))
		{
			if (t.tag == "PatientName")
			{
				t.GetComponent<TextMesh>().text = patientInfo["PatientName"];
			}
			else if (t.tag == "PatientId")
			{
				t.GetComponent<TextMesh>().text = patientInfo["PatientID"];
			}
			else if (t.tag == "PatientDateOfBirth")
			{
				string text =  patientInfo["PatientBirthDate"];
				text = text.Insert(4, "/");
				text = text.Insert(7, "/");
				t.GetComponent<TextMesh>().text = text;
			}
			else if (t.tag == "PatientSex")
			{
				t.GetComponent<TextMesh>().text = patientInfo["PatientSex"];
			}
			else if (t.tag == "StudyDescription")
			{
				t.GetComponent<TextMesh>().text = patientInfo["StudyDescription"];
			}
			else
			{
				// Unsupported Key
			}
		}

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
                ToggleScrollButtons();
				this.scrollButtonsVisible = true;
			}
		}

		CreateTray(image);
	}


    /// <summary>
    /// Creates new button, and applies passed in attributes. 
    /// </summary>
    /// <param name="attributes">Attributes to be applied to the new button</param>
    /// <param name="prefab">Prefab to instantiate as a button</param>
    /// <returns>Newly created button GameObject</returns>
    public GameObject CreateButton(ButtonAttributes attributes, GameObject buttonPrefab)
    {
        GameObject newButton = Instantiate(buttonPrefab, attributes.position,
            Quaternion.Euler(attributes.rotation));

        newButton.transform.parent = gameObject.transform;

        newButton.GetComponent<VRButton>().Initialise(attributes, this.gameObject);
        newButton.name = attributes.buttonName;

        return newButton;
    }



    /// <summary>
    /// Display the scrolling buttons for the tray
    /// </summary>
    /// <post>The buttons to allow the user to scroll through the tray are displayed</post>
    void ToggleScrollButtons()
    {
        foreach (GameObject button in buttonObjects)
        {
            string name = button.GetComponent<VRButton>().GetName();
            if (name == "Left" || name == "Right")
            {
                button.SetActive(!button.activeSelf);
            }
        }
    }


	/// <summary>
	/// Function CreateTray() creates and displays the tray of thumbnails
	/// The tray exhibits all images that are available to the user to cycle through in the display
	/// <pre>Nothing</pre>
	/// <post>A new Tray instantiated and added to the hierarchy</post>
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
			Texture2D[] textureToSend = new Texture2D[1];
			textureToSend [0] = image;
			this.tray.gameObject.SendMessage ("HighlightTray", textureToSend);
		}
		else
		{
			this.tray.UpdateTray(image);
			if (this.images.Count <= this.displayImagePositions.Length) {
				Texture2D[] textureToSend = new Texture2D[this.displayImagePositions.Length];
				int i = 0;
				foreach (Texture2D t in this.images) {
					textureToSend [i] = t;
					i++;
				}
				this.tray.gameObject.SendMessage ("HighlightTray", textureToSend);
			}
		}

        Assert.IsNotNull(this.tray, "The tray is null.");
        Assert.IsTrue(this.tray.GetImages().Contains(image), "The tray does not contain the new image.");
	}


	/// <summary>
	/// Function CreateCopy will instantiate a new Copy in the space at the center of the user's view
	/// A Copy is the object that a user can manipulate and move within the workspace
	/// </summary>
	/// <pre>Texture2D image</pre>
	/// <post>A new Copy is instantiated and added to the hierarchy</post>
	public void CreateCopy(Texture2D image)
	{
		Assert.IsNotNull(image, "Creating new Copy from Display image was null");

		// Retrieve the center point of the camera view and instantiate a Copy on that location
		Vector3 trans = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2,
			Camera.main.nearClipPlane));
		trans.z = copyDepth;
		GameObject newCop = Instantiate(copyPrefab, trans, new Quaternion(0, 0, 0, 0));
		Copy cop = newCop.GetComponent<Copy>();

		// If the image texture does not have a name, we will give it a generic name
		if (image.name == "")
		{
			cop.name = "Copy " + this.copyId;
			this.copyId++;
			// We can use the name of the texture
		} else {
			cop.name = image.name;
		}
		cop.NewCopy(image);
		copies.Add(newCop);
	}


	/// <summary>
	/// GetImages() will return the list of images in the manager
	/// </summary>
	/// <pre>nothing</pre>
	/// <post>nothing</post>
	/// <returns>A list of Texture2D elements</returns>
	public List<Texture2D> GetImages()
	{
		return this.images;
	}


	/// <summary>
	/// GetCopies() will return the list of Copies in the manager
	/// </summary>
	/// <pre>nothing</pre>
	/// <post>nothing</post>
	/// <returns>A list of Display ojects</returns>
	public List<GameObject> GetCopies()
	{
		return this.copies;
	}


	/// <summary>
	/// ScrollLeft() will shift the images displayed to the user in the displayImages
	/// list to the left.
	/// </summary>
	/// <pre>there are more display images than the length of display iamges positions array</pre>
	/// <post>positions and activation of GameObjects in displayImages is changed.</post>
	private void ScrollLeft()
	{
        Debug.Log("User has scrolled the display left.");
		// If there are not enough images to scroll through
		Assert.IsTrue(this.images.Count >= this.displayImagePositions.Length, "There are no more images to scroll through.");

		if (this.images.Count >= this.displayImagePositions.Length)
		{
			GameObject temp = displayImages.First.Value;
			temp.SetActive(false);
			displayImages.RemoveFirst();
			displayImages.AddLast(temp);
			redrawDisplayImages();
		}
	}


	/// <summary>
	/// ScrollRightt() will shift the images displayed to the user in the displayImages
	/// list to the right.
	/// </summary>
	/// <pre>there are more display images than the length of display iamges positions array</pre>
	/// <post>positions and activation of GameObjects in displayImages is changed.</post>
	private void ScrollRight()
	{
        Debug.Log("User has scrolled the display right.");
		// If there are not enough images to scroll through
		Assert.IsTrue(this.images.Count >= this.displayImagePositions.Length, "There are no more images to scroll through.");

		if (this.images.Count >= this.displayImagePositions.Length)
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
	}


	/// <summary>
	/// Draw the scrolling display images from the list of display images
	/// The number of display images drawn is determined by the length of the display
	/// image positions array
	/// </summary>
	/// <pre>there are display images to be drawn</pre>
	/// <post>display images are drawn in the workspace</post>
	private void redrawDisplayImages()
	{
		int i = 0;
		// for each display image, if there are less than the display image positions array length
		// draw the image in the specified position and set it active
		Texture2D[] texturesToSend = new Texture2D[displayImagePositions.Length];
		foreach (GameObject img in displayImages)
		{
			if (i < displayImagePositions.Length)
			{
				img.gameObject.GetComponent<Transform>().position = displayImagePositions[i];
				img.gameObject.GetComponent<Transform>().rotation = Quaternion.Euler(displayImageRotations[i]);
				img.SetActive(true);
				texturesToSend[i] = img.GetComponent<DisplayImage>().image;
				i++;


			}
			else
			{
				break;
			}
		}
		if (this.tray != null) {
			this.tray.gameObject.SendMessage ("HighlightTray", texturesToSend);
		}
	}

    //=================================
    // TEST HOOKS
    //================================  


    /// <summary>
    /// Test the scrolling functionality by scrolling through all images 
    /// </summary>
    /// <param name="numberOfImages"></param>
    public void TestScrollLeftAndRight(int numberOfImages)
	{
		GameObject firstElement = this.displayImages.First.Value;
		// Scroll left until we are back at the start
		for(int i = 0; i < numberOfImages; i++)
		{
			GameObject currentElement = this.displayImages.First.Value;
			this.ScrollLeft();
			Assert.AreNotEqual(this.displayImages.First.Value, currentElement, "The first image of the display is equal to the current element.");
			Assert.AreEqual(this.displayImages.Last.Value, currentElement, "The last image of the display is not equal to the current element.");
		}

		Assert.AreEqual(firstElement, this.displayImages.First.Value, "The first element is not equal to the first element of the display.");

		firstElement = this.displayImages.First.Value;

		// Scroll right until we are back at the start
		for (int i = 0; i < numberOfImages; i++)
		{
			GameObject currentElement = this.displayImages.First.Value;
			this.ScrollRight();
			Assert.AreNotEqual(this.displayImages.First.Value, currentElement, "The first value of displayImages is the current element.");
			Assert.AreEqual(this.displayImages.First.Next.Value, currentElement, "The next value in the list is not the current element.");
		}


	} 
    


	/// <summary>
	/// Used to remove copies from the display when they are removed from the workspace
	/// </summary>
	/// <param name="copy">The copy object to remove</param>
	public void RemoveCopy(GameObject copy)
	{
		this.copies.Remove(copy);
	}

   
    /// <summary>
    /// Set the copy list to the given list.
    /// 
    /// Used for testing purposes
    /// </summary>
    /// <param name="copies"></param>
    public void SetCopies(List<GameObject> copies)
    {
        this.copies = copies;
    }

     
}

