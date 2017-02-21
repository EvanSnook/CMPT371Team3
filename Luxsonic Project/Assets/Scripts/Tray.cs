using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Tray class represents a tray of thumbnails in the workspace that is used to
/// keep track of all loaded images
/// </summary>
public class Tray : MonoBehaviour {

    public float trayStartX;        // The top left x coordinate for the tray to display
    public float trayStartZ;        // The top left z coordinate for the tray to display
    public float trayDepth;         // The depth of the tray
    public int trayNumColumns = 1;      // The number of columns for the tray to display, default 1
    public int trayNumRows = 1;         // The number of rows for the tray to display, default 1
    public float trayIncrementor = 1;   // The distance between each thumbnail
    public float trayThumbnailScale = 1;// The scale for the images in the tray

    public GameObject thumbnail;    // The object to use as a thumbnail
    public Display manager;

    List<GameObject> thumbnails = new List<GameObject>();   // The list of thumbnails being displayed in the tray

    // Use this for initialization
    void Start () {
		
	}

    /// <summary>
    /// Update the tray to display the thumbnails
    /// </summary>
    /// <param name="images">The list of images to turn into thumbnails</param>
    public void UpdateTray(List<Texture2D> images)
    {
        
        // Destroy the current tray  TODO: Make more efficient
        foreach (GameObject t in thumbnails)
        {
            DestroyImmediate(t);
        }

        thumbnails.Clear(); // Clear the list so we can recalculate the thumbnails
        float x = trayStartX;
        float z = trayStartZ;
        // For each image in the list, create a thumbnail and display it in the tray
        foreach (Texture2D image in images)
        {
            if (Mathf.Abs(x - trayStartX) >= trayNumColumns * trayIncrementor)
            {
                x = trayStartX;
                z -= trayIncrementor;
            }
            x += trayIncrementor;

            if (Mathf.Abs(z - trayStartZ) >= trayNumRows * trayIncrementor && x >= trayNumRows * trayIncrementor)
            {
                break;
            }
            //Debug.Log("Going to instantiate thumb");
            GameObject newThumb = Instantiate(thumbnail, new Vector3(x, z, trayDepth), new Quaternion(0, 0, 0, 0));
            //Debug.Log("Instantiated new thumb");
            newThumb.GetComponent<SpriteRenderer>().sprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f));
            newThumb.transform.localScale = new Vector3(trayThumbnailScale, trayThumbnailScale, 0);
            newThumb.GetComponent<Thumbnail>().manager = this.manager.gameObject;
            newThumb.GetComponent<Thumbnail>().image = image;
            thumbnails.Add(newThumb);
        }
    }

    /// <summary>
    /// Return the list of thumbnails in the manager
    /// </summary>
    /// <returns>A list of Thumbnail objects</returns>
    public List<GameObject> GetThumbnails()
    {
        return this.thumbnails;
    }
}
