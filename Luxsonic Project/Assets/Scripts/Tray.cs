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

    private float currentTrayX;
    private float currentTrayZ;

    [SerializeField]
    private VRButton buttonPrefab;

    private VRButton scrollUpButton;
    private VRButton scrollDownButton;

    [SerializeField]
    private Vector3 scrollUpButtonPosition;
    [SerializeField]
    private Vector3 scrollDownButtonPosition;

    private LinkedList<Texture2D> images;
    private Texture2D firstPosition;

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
        newButton.transform.localPosition = new Vector3(pos.x, pos.y, 0.0f);
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
        images = new LinkedList<Texture2D>();
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

    private void ScrollUp()
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
            //thumbToRemove.Value.SetActive(false);
            this.SafeDelete(thumbToRemove.Value);
            thumbToRemove = this.thumbnails.First;

            int j = 0;
            LinkedListNode<Texture2D> temp = current;

            while (j < (this.trayNumColumns + this.trayNumRows))
            {
                temp = temp.Next;
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
            current = current.Next;
        }

        foreach (GameObject thumb in this.thumbnails)
        {
            if (positions.Count > 0)
            {
                thumb.transform.position = positions.First.Value;
                positions.RemoveFirst();
            }else
            {
                break;
            }
        }
        // add the this.images at first item + (this.numCOls + numRows) to the list of thumbs by instantiating them
        // for each thumbnail, set its position to the next position in the position list

    }

    private void ScrollDown()
    {

    }

    private void SafeDelete(GameObject obj)
    {
        if (Application.isEditor)
        {
            Destroy(obj);
        }
        else
        {
            DestroyImmediate(obj);
        }
    }
}

