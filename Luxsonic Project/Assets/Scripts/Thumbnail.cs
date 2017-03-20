using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents a thumbnail object that is interactable in VR
/// </summary>
public class Thumbnail : MonoBehaviour
{

    // The texture being displayed on the thumbnail
    public Texture2D image;
    // The GameObject that the texture will report to
    public GameObject manager;
    //
    private bool pressed;

    /// <summary>
    /// Start this instance.
    /// </summary>
    private void Start()
    {
        // Use the size of the sprite to create the bounding box
        Vector2 size = this.GetComponent<SpriteRenderer>().sprite.bounds.size;
        this.GetComponent<BoxCollider>().size = size;

    }



    /// <summary>
    /// Raises the mouse down event. Notify the manager, that a Copy should be made
    /// </summary>
    void OnMouseDown()
    {
        manager.SendMessage("CreateCopy", image);
    }

    public void Selected()
    {
        manager.SendMessage("CreateCopy", image);
    }

    /// <summary>
    /// SetPressed sets the value of pressed to the value of val
    /// Pre:: 
    /// Post:: pressed is set to the value of val
    /// Return:: nothing
    /// </summary>
    public void SetPressed(bool val)
    {
        pressed = val;
        if (val) this.Selected();
    }
    /// <summary>
    /// GetPressed returns the value of pressed
    /// Pre:: 
    /// Post:: 
    /// Return:: value of pressed
    /// </summary>
    public bool GetPressed()
    {
        return pressed;
    }
}
