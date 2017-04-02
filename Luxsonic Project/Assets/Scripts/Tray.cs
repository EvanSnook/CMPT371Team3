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
    public VRButton buttonPrefab;

    // References to buttons to scroll the tray
    private VRButton scrollUpButton;
    private VRButton scrollDownButton;

    // The positions of the scroll buttons
    [SerializeField]
    private Vector3 scrollUpButtonPosition;
    [SerializeField]
    private Vector3 scrollDownButtonPosition;

    // The list of images in the scene
    private LinkedList<Texture2D> images;

    // The texture in the images list that marks the first item in the tray
    private Texture2D firstPosition;
    // The texture in the images list that marks the first item in the tray
    private Texture2D lastPosition;

    private int imageID = 0;
    
    // The number of images after the tray 
    private int numAfterTray;
    // The number of images before the tray
    private int numBeforeTray;

    // Use this for initialization
    void Start()
    {
        
    }

    /// <summary>
    /// Update the tray to display the thumbnails
    /// </summary>
    /// <param name="image">The new image to add to the tray</param>
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

        if (Mathf.Abs(z - trayStartZ) >= trayNumRows * trayIncrementor && x >= trayNumRows * trayIncrementor)
        {
            if (this.scrollDownButton == null || this.scrollUpButton == null)
            {
                this.scrollUpButton = this.InitializeButton(ButtonType.TRAY_SCROLL_UP_BUTTON);
                this.scrollDownButton = this.InitializeButton(ButtonType.TRAY_SCROLL_DOWN_BUTTON);
            }
        }
        else
        {

            GameObject newThumb = Instantiate(thumbnailPrefab, new Vector3(x, z, trayDepth), new Quaternion(0, 0, 0, 0));
            newThumb.transform.parent = gameObject.transform;

            newThumb.GetComponent<SpriteRenderer>().sprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f));
            newThumb.transform.localScale = new Vector3(trayThumbnailScale, trayThumbnailScale, 0);
            newThumb.GetComponent<Thumbnail>().manager = this.manager.gameObject;
            newThumb.GetComponent<Thumbnail>().image = image;
            thumbnails.AddLast(newThumb);

            // Save the values of x and z for next time
            this.currentTrayX = x;
            this.currentTrayZ = z;
        }
    }

    /// <summary>
    /// Initializes the scrolling buttons for the tray.
    /// </summary>
    /// <returns>VRButton initialized with correct attributes.</returns>
    /// <param name="index">The type of button</param>
    public VRButton InitializeButton(ButtonType index)
    {
        Vector3 pos = Vector3.zero;
        string newName = "";

        if (index == ButtonType.TRAY_SCROLL_UP_BUTTON)
        {
            pos = this.scrollUpButtonPosition;
            newName = "Scroll Up";
        }
        else if (index == ButtonType.TRAY_SCROLL_DOWN_BUTTON)
        {
            pos = this.scrollDownButtonPosition;
            newName = "Scroll Down";
        }


        VRButton newButton;

        newButton = Instantiate(buttonPrefab, pos, this.transform.rotation);
        newButton.transform.parent = this.transform;

        newButton.type = index;
        newButton.transform.position = new Vector3(pos.x, pos.y, pos.z);
        newButton.name = newName;
        newButton.buttonName = newName;
        newButton.manager = this.gameObject;
        newButton.textObject = newButton.GetComponentInChildren<TextMesh>();
        newButton.textObject.text = newName;
        return newButton;
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
    public void Setup(Texture2D image)
    {
        this.currentTrayX = this.trayStartX;
        this.currentTrayZ = this.trayStartZ;

        this.firstPosition = image;
        this.lastPosition = image;
        images = new LinkedList<Texture2D>();
        this.numAfterTray = 0;
        this.numBeforeTray = 0;
        this.UpdateTray(image);
    }

    public void VRButtonClicked(ButtonType button)
    {
        switch (button)
        {
            case ButtonType.TRAY_SCROLL_UP_BUTTON:
                this.ScrollUp();
                break;

            case ButtonType.TRAY_SCROLL_DOWN_BUTTON:
                this.ScrollDown();
                break;
        }
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

    private void SafeDelete(GameObject obj)
    {
        if (Application.isEditor)
        {
            DestroyImmediate(obj);
        }
        else
        {
            Destroy(obj);
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

        Assert.IsNotNull(this.lastPosition);
        Assert.IsNotNull(this.firstPosition);
        Assert.AreNotEqual(originalLast, this.lastPosition);
        Assert.AreNotEqual(originalFirst, this.firstPosition);
        Assert.AreEqual(originalThumbnailCount, this.thumbnails.Count);
    }

}

