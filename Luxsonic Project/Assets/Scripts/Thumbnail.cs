using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents a thumbnail object that is interactable in VR
/// </summary>
public class Thumbnail : MonoBehaviour {

	// The texture being displayed on the thumbnail
    public Texture2D image;
	// The GameObject that the texture will report to
    public GameObject manager;

	/// <summary>
	/// Start this instance.
	/// </summary>
    private void Start()
    {
        // Use the size of the sprite to create the bounding box
        Vector2 size = this.GetComponent<SpriteRenderer>().sprite.bounds.size;
        this.GetComponent<BoxCollider>().size = size;
        
    }

    /*public void OnCollisionEnter(Collision collision)
    {
        manager.SendMessage("CreateCopy", image);
    }*/

	/// <summary>
	/// Raises the mouse down event. Notify the manager, that a Copy should be made
	/// </summary>
    void OnMouseDown()
    {
        manager.SendMessage("CreateCopy", image);
    }
}
