using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

using buttons;
using System;

/// <summary>
/// The Tray class represents a tray of thumbnails in the workspace that is used to
/// keep track of all loaded images
/// </summary>
public class Tray : MonoBehaviour, IVRButton
{
    // Holds the attributes for each button to be instantiated
    [SerializeField]
    private List<ButtonAttributes> buttonList = new List<ButtonAttributes>();

    // Holds references to the instantiated buttons
    private List<GameObject> buttonObjects = new List<GameObject>();

	// The top left x coordinate for the tray to display
	public float trayStartX;
	// The top left z coordinate for the tray to display
	public float trayStartZ;
	// The depth of the tray
	public float trayDepth;
	// The number of columns for the tray to display, default 1
	public int trayNumColumns = 1;
	// The number of rows for the tray to display, default 1
	public int trayNumRows = 1;
	// The distance between each thumbnail
	public float trayIncrementor = 1;
	// The scale for the images in the tray
	public float trayThumbnailScale = 1;

	// The prefab to use as a thumbnail
	public GameObject thumbnailPrefab;
	// Manages everything in dashboard
	public Display manager;

	// The list of thumbnails being displayed in the tray
	LinkedList<GameObject> thumbnails = new LinkedList<GameObject>();

	// The current positions to put the next thumbnail
	private float currentTrayX;
	private float currentTrayZ;

	// The prefab used to create a new button
	public GameObject buttonPrefab;

	// The list of images in the scene
	private LinkedList<Texture2D> images;

	// The texture in the images list that marks the first item in the tray
	private Texture2D firstPosition;
	// The texture in the images list that marks the first item in the tray
	private Texture2D lastPosition;

	// The number of images after the tray 
	private int numAfterTray;
	// The number of images before the tray
	private int numBeforeTray;

    // The texture to use for outlining objects in the tray
	private Texture2D outlineTexture;

    // The scale of the outline for the thumbnails
	[SerializeField]
	private int outlineScale;

    // The distance fromt the thumbnail that the outline should be
	[SerializeField]
	private float outlineDepth;


    /// <summary>
    /// Called when the object is created
    /// </summary>
    private void Start()
    {
        foreach (ButtonAttributes attributes in buttonList)
        {
            buttonObjects.Add(CreateButton(attributes, buttonPrefab));
        }
        ToggleScrollButtons();
    }


    /// <summary>
    /// Creates new button, and applies passed in attributes. 
    /// </summary>
    /// <param name="attributes">Attributes to be applied to the new button</param>
    /// <param name="buttonPrefab">Prefab to instantiate as a button</param>
    /// <returns>Newly created button GameObject</returns>
    public GameObject CreateButton(ButtonAttributes attributes, GameObject buttonPrefab)
    {
        GameObject newButton;

        newButton = Instantiate(buttonPrefab, attributes.position,
        Quaternion.Euler(attributes.rotation));

        newButton.GetComponent<VRButton>().Initialise(attributes, this.gameObject);
        newButton.name = attributes.buttonName;

        return newButton;
    }


    /// <summary>
    /// Display the scrolling buttons for the tray
    /// </summary>
    /// <pre>The current number of images in the tray is equal to the max</pre>
    /// <post>The buttons to allow the user to scroll through the tray are displayed</post>
    void ToggleScrollButtons()
    {
        foreach (GameObject button in buttonObjects)
        {
            string name = button.GetComponent<VRButton>().GetName();
            if (name == "Up" || name == "Down")
            {
                button.SetActive(!button.activeSelf);
            }
        }
    }


	/// <summary>
	/// Update the tray to display the thumbnails
	/// </summary>
	/// <param name="image">The new image to add to the tray</param>
    /// <pre>The tray has been initialized via the Setup() method</pre>
    /// <post>The tray will be updated to contain the given image</post>
	public void UpdateTray(Texture2D image)
	{
		this.images.AddLast(image);


		if(this.thumbnails.Count < this.trayNumColumns * this.trayNumRows)
		{
			this.lastPosition = image;
		}else
		{
			this.numAfterTray++;
		}

		// Get the last position we used
		float x = this.currentTrayX;
		float z = this.currentTrayZ;

		if (Mathf.Abs(x - trayStartX) >= trayNumColumns * trayIncrementor)
		{
			x = trayStartX;
			z -= trayIncrementor;
		}


		x += trayIncrementor;


		if (Mathf.Abs(z - trayStartZ) >= trayNumRows * trayIncrementor)// && x >= trayNumColumns * trayIncrementor)
		{	
            ToggleScrollButtons();
		}
		else
		{

			GameObject newThumb = Instantiate(thumbnailPrefab, new Vector3(x, z, trayDepth), new Quaternion(0, 0, 0, 0));
			newThumb.transform.parent = gameObject.transform;

			newThumb.GetComponent<SpriteRenderer>().sprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f));
			newThumb.transform.localScale = new Vector3(trayThumbnailScale, trayThumbnailScale, 0);
			newThumb.GetComponent<Thumbnail>().manager = this.manager.gameObject;
			newThumb.GetComponent<Thumbnail>().image = image;

			this.outlineTexture = new Texture2D(newThumb.GetComponent<SpriteRenderer>().sprite.texture.width + this.outlineScale, newThumb.GetComponent<SpriteRenderer>().sprite.texture.height + this.outlineScale);

			newThumb.transform.GetChild (0).transform.localPosition = new Vector3 (0, 0, 0);
			newThumb.transform.GetChild(0).transform.rotation = newThumb.transform.rotation;

			newThumb.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(outlineTexture, new Rect(0, 0, this.outlineTexture.width, this.outlineTexture.height), new Vector2(0.5f, 0.5f));
			newThumb.transform.GetChild(0).gameObject.SetActive(false);

			thumbnails.AddLast(newThumb);

			// Save the values of x and z for next time
			this.currentTrayX = x;
			this.currentTrayZ = z;
		}
	}


	/// <summary>
	/// Return the list of thumbnails in the manager
	/// </summary>
	/// <returns>A list of Thumbnail objects</returns>
	public LinkedList<GameObject> GetThumbnails()
	{
		return this.thumbnails;
	}


	/// <summary>
	/// Called to initialize the tray for the first time
	/// </summary>
	/// <param name="image">The first image to add to the tray</param>
    /// <post>The tray will be initialized with the correct starting positions 
    /// and the given image will be added to the tray</post>
	public void Setup(Texture2D image)
	{
        Assert.IsNotNull(image, "The given image was null.");
		this.currentTrayX = this.trayStartX;
		this.currentTrayZ = this.trayStartZ;

		this.firstPosition = image;
		this.lastPosition = image;
		images = new LinkedList<Texture2D>();
		this.numAfterTray = 0;
		this.numBeforeTray = 0;
		this.UpdateTray(image);
	}


	/// <summary>
	/// Moves the images in the tray up to allow the user to see images below the boundaries of the tray.
	/// Each image is shuffled to the left in its row.  If an image is at the far left of its row, it will move to the 
	/// farthest right position of the row above.  
	/// </summary>
	/// <pre>There are images after the bottom of the tray</pre>
	/// <post>The images in the tray have different positions</post>
	private void ScrollUp()
	{
        Debug.Log("The user has scrolled the tray up.");
		Assert.IsTrue(this.images.Count > this.thumbnails.Count, "There are no more images in the tray to scroll through.");
		Assert.IsTrue(this.numAfterTray > 0, "There are no images after the tray to scroll through.");

		if (this.numAfterTray > 0)
		{
			// For each item, add the item's position to the list
			LinkedList<Vector3> positions = new LinkedList<Vector3>();

			foreach (GameObject thumb in this.thumbnails)
			{
				LinkedListNode<GameObject> next = this.thumbnails.Find(thumb);

				positions.AddLast(next.Value.transform.position);
			}

			// remove the first numberOfColumns items from the list of thumbs
			int i = 0;
			LinkedListNode<GameObject> thumbToRemove = this.thumbnails.First;
			LinkedListNode<Texture2D> current = this.images.Find(this.firstPosition);

			while (i < this.trayNumColumns)
			{
				this.thumbnails.RemoveFirst();
				this.SafeDelete(thumbToRemove.Value);
				thumbToRemove = this.thumbnails.First;
				this.numBeforeTray++;

				// add the this.images at first item + (this.numCOls + numRows) to the list of thumbs by instantiating them
				int j = 0;
				LinkedListNode<Texture2D> temp = current;

				while (j < (this.trayNumColumns * this.trayNumRows))
				{
					temp = temp.Next;
					if (temp.Value.Equals(this.images.Last.Value))
					{
						j = this.trayNumColumns * this.trayNumRows;
						i = this.trayNumColumns;
					}
					j++;
				}


				GameObject newThumb = Instantiate(this.thumbnailPrefab, Vector3.zero, Quaternion.Euler(Vector3.zero));
				newThumb.transform.parent = gameObject.transform;

				newThumb.GetComponent<SpriteRenderer>().sprite = Sprite.Create(temp.Value, new Rect(0, 0, temp.Value.width, temp.Value.height), new Vector2(0.5f, 0.5f));
				newThumb.transform.localScale = new Vector3(trayThumbnailScale, trayThumbnailScale, 0);
				newThumb.GetComponent<Thumbnail>().manager = this.manager.gameObject;
				newThumb.GetComponent<Thumbnail>().image = temp.Value;

				this.thumbnails.AddLast(newThumb);
				i++;
				this.numAfterTray--;
				current = current.Next;
			}

			// for each thumbnail, set its position to the next position in the position list
			foreach (GameObject thumb in this.thumbnails)
			{
				if (positions.Count > 0)
				{
					thumb.transform.position = positions.First.Value;
					positions.RemoveFirst();
				}
				else
				{
					break;
				}
			}

			this.firstPosition = this.thumbnails.First.Value.GetComponent<Thumbnail>().image;
			this.lastPosition = this.thumbnails.Last.Value.GetComponent<Thumbnail>().image;


		}
	}


	/// <summary>
	/// Moves the images in the tray down to allow the user to see images above the boundaries of the tray.
	/// Each image is shuffled to the right in its row.  If an image is at the far right of its row, it will move to the 
	/// farthest left position of the row below.  
	/// </summary>
	/// <pre>There are images before the top of the tray</pre>
	/// <post>The images in the tray have different positions</post>
	private void ScrollDown()
	{
        Debug.Log("The user has scrolled the tray down.");
		Assert.IsTrue(this.numBeforeTray > 0, "There are no images after the tray to scroll through.");

		if (this.numBeforeTray > 0)
		{
			// For each item, add the item's position to the list
			LinkedList<Vector3> positions = new LinkedList<Vector3>();

			foreach (GameObject thumb in this.thumbnails)
			{
				LinkedListNode<GameObject> next = this.thumbnails.Find(thumb);

				positions.AddLast(next.Value.transform.position);
			}

			// remove the last numberOfColumns items from the list of thumbs
			int i = 0;
			LinkedListNode<GameObject> thumbToRemove = this.thumbnails.Last;
			LinkedListNode<Texture2D> current = this.images.Find(this.lastPosition);

			while (i < this.trayNumColumns)
			{
				this.thumbnails.RemoveLast();
				this.SafeDelete(thumbToRemove.Value);
				thumbToRemove = this.thumbnails.Last;
				this.numAfterTray++;

				// add the this.images at first item + (this.numCOls + numRows) to the list of thumbs by instantiating them
				int j = 0;
				LinkedListNode<Texture2D> temp = current;

				while (j < (this.trayNumColumns * this.trayNumRows))
				{
					if (temp.Previous != null)
					{
						temp = temp.Previous;
					}else { 
						j = this.trayNumColumns * this.trayNumRows;
						i = this.trayNumColumns;
					}
					j++;
				}


				GameObject newThumb = Instantiate(this.thumbnailPrefab, Vector3.zero, Quaternion.Euler(Vector3.zero));
				newThumb.transform.parent = gameObject.transform;

				newThumb.GetComponent<SpriteRenderer>().sprite = Sprite.Create(temp.Value, new Rect(0, 0, temp.Value.width, temp.Value.height), new Vector2(0.5f, 0.5f));
				newThumb.transform.localScale = new Vector3(trayThumbnailScale, trayThumbnailScale, 0);
				newThumb.GetComponent<Thumbnail>().manager = this.manager.gameObject;
				newThumb.GetComponent<Thumbnail>().image = temp.Value;

				this.thumbnails.AddFirst(newThumb);
				i++;
				this.numBeforeTray--;
				current = current.Previous;
			}

			// for each thumbnail, set its position to the next position in the position list
			foreach (GameObject thumb in this.thumbnails)
			{
				if (positions.Count > 0)
				{
					thumb.transform.position = positions.First.Value;
					positions.RemoveFirst();
				}
				else
				{
					break;
				}
			}

			this.firstPosition = this.thumbnails.First.Value.GetComponent<Thumbnail>().image;
			this.lastPosition = this.thumbnails.Last.Value.GetComponent<Thumbnail>().image;

		}
	}

    /// <summary>
    /// Safely remove an object from the scene using the appropriate method
    /// </summary>
    /// <param name="obj"></param>
    /// <pre>The given object exists</pre>
    /// <post>The given object has been deleted from the scene</post>
	private void SafeDelete(GameObject obj)
	{
        Assert.IsNotNull(obj, "The object to delete was null.");
		if (Application.isEditor)
		{
			DestroyImmediate(obj);
		}
		else
		{
			Destroy(obj);
		}
	}


	/// <summary>
	/// Highlight the given images in the tray
	/// </summary>
	/// <param name="textures">The list of images to highlight</param>
    /// <pre>The tray has been created</pre>
    /// <post>The given images have been highlighted in the tray</post>
	public void HighlightTray(Texture2D[] textures)
	{
		// Turn off all highlights
		foreach(GameObject thumbnail in this.thumbnails)
		{
			thumbnail.transform.GetChild(0).transform.gameObject.SetActive(false);
		}

		// Highlight the images that were passed to us
		for(int i = 0; i < textures.Length; i++)
		{
			foreach(GameObject thumbnail in this.thumbnails)
			{
				if(thumbnail.GetComponent<Thumbnail>().image == textures[i])
				{
					thumbnail.transform.GetChild(0).transform.gameObject.SetActive(true);
				}
			}
		}
	}

	//========================================
	// TEST HOOKS
	//========================================

	/// <summary>
	/// Used to test the scrolling up functionality of the tray
	/// </summary>
	/// <param name="direction">"up" to test scroll up. "down" to test scroll down</param>
	public void TestScroll(string direction)
	{
		Texture2D originalLast = this.lastPosition;
		Texture2D originalFirst = this.firstPosition;
		int originalThumbnailCount = this.thumbnails.Count;

		if (direction.ToLower() == "up") {
			this.ScrollUp();
		}else if(direction.ToLower() == "down")
		{
			this.ScrollDown();
		}else
		{
			Assert.IsTrue(true, "Invalid parameter to TestScroll()");
		}

		Assert.IsNotNull(this.lastPosition, "The last tray position was null.");
		Assert.IsNotNull(this.firstPosition, "The first tray position was null.");
		Assert.AreNotEqual(originalLast, this.lastPosition, "The original last position is the same as the new last position.");
		Assert.AreNotEqual(originalFirst, this.firstPosition, "The original first position is the same as the new first position.");
		Assert.AreEqual(originalThumbnailCount, this.thumbnails.Count, "We lost some thumbnails.");
	}

    /// <summary>
    /// Return the list of images in the tray
    /// </summary>
    /// <returns>The list of images in the tray</returns>
    public LinkedList<Texture2D> GetImages()
    {
        return this.images;
    }

}

