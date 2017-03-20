using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Tray class represents a tray of thumbnails in the workspace that is used to
/// keep track of all loaded images
/// </summary>
public class Tray : MonoBehaviour
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
    List<GameObject> thumbnails = new List<GameObject>();

    private float currentTrayX;
    private float currentTrayZ;

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
            // TODO: Add scrolling or end tray here
        }

        GameObject newThumb = Instantiate(thumbnailPrefab, new Vector3(x, z, trayDepth), new Quaternion(0, 0, 0, 0));
        newThumb.transform.parent = gameObject.transform;

        newThumb.GetComponent<SpriteRenderer>().sprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f));
        newThumb.transform.localScale = new Vector3(trayThumbnailScale, trayThumbnailScale, 0);
        newThumb.GetComponent<Thumbnail>().manager = this.manager.gameObject;
        newThumb.GetComponent<Thumbnail>().image = image;
        thumbnails.Add(newThumb);

        // Save the values of x and z for next time
        this.currentTrayX = x;
        this.currentTrayZ = z;
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
    /// Called to initialize the tray for the first time
    /// </summary>
    /// <param name="image">The first image to add to the tray</param>
    public void Setup(Texture2D image)
    {
        this.currentTrayX = this.trayStartX;
        this.currentTrayZ = this.trayStartZ;

        this.UpdateTray(image);
    }
}

