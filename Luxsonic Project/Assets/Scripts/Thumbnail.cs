using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents a thumbnail object that is interactable in VR
/// </summary>
public class Thumbnail : MonoBehaviour {

    public Texture2D image;
    public GameObject manager;

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

    void OnMouseDown()
    {
        manager.SendMessage("CreateCopy", image);
    }
}
