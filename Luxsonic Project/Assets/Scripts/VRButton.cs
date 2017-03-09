using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class defines an interactable 3D button for use with VR
/// A class using this button must implement the IVRButton interface
/// </summary>
public class VRButton : MonoBehaviour {

    // Button name
    public string name;
    // string to store a potental path
    public string path;
    // The object that creates and contains the functionality for this button.
    public GameObject manager;
    // Text on button
    public TextMesh textObject;
    // Buttons state
    bool pressed = false;
    // Times button enters
    int timer;
    

    /// <summary>
    /// onCollisionEnter deals with colliders entering the button
    /// Checks to make sure the colliding object is a hand and the 
    /// button isnt already pressed
    /// Pre:: timer = 0
    /// Post: sets button pressed to true
    /// Post: set timer to zero
    /// Return:: nothing
    /// </summary>
    public void OnCollisionEnter(Collision collision)
    {
        if ((timer == 0) && (pressed == false) && (collision.gameObject.tag == "Hand"))
        {
            manager.SendMessage("VRButtonClicked", name);
            textObject.color = Color.black;
            timer = 200;
            pressed = true;
        }
    }

    /// <summary>
    /// onCollisionExit deals with button collisions when collder exits
    /// sets button pressed to false when hand exits button
    /// Pre:: pressed = true
    /// Post: pressed = false
    /// Return:: nothing
    /// </summary>
    public void OnCollisionExit(Collision collision)
    {
        pressed = false;
    }
    // Use this for initialization
    void Start () {
        timer = 0;
	}

    // Update constantly updates the timer
    // Brings back timer whenever it exceeds zero
    private void Update()
    {
        if(timer > 0)
        {
            timer = timer - 1;
        }
    }

    // When mouve is pressed send clicked message to manager
    void OnMouseDown()
    {
        manager.SendMessage("VRButtonClicked", name);
    }
}
