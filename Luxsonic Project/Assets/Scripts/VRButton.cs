using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class defines an interactable 3D button for use with VR
/// A class using this button must implement the IVRButton interface
/// </summary>
public class VRButton : MonoBehaviour {

    public string name;
    public GameObject manager;  // The object that creates and contains the functionality for this button.
    public TextMesh textObject;
    bool pressed = false;
    int timer;

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

    public void OnCollisionExit(Collision collision)
    {
        pressed = false;
    }
    // Use this for initialization
    void Start () {
        textObject = gameObject.GetComponentInChildren<TextMesh>();
        textObject.text = name;
        timer = 0;
	}

    private void Update()
    {
        if(timer > 0)
        {
            timer = timer - 1;
        }
    }

    void OnMouseDown()
    {
        manager.SendMessage("VRButtonClicked", name);
    }
}
