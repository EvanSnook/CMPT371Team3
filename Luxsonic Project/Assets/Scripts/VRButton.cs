﻿using System.Collections;
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
    

    // Use this for initialization
    void Start () {
        textObject = gameObject.GetComponentInChildren<TextMesh>();
        textObject.text = name;
        path = null;
	}

    // When mouve is pressed send clicked message to manager
    void OnMouseDown()
    {
        manager.SendMessage("VRButtonClicked", name);
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
        if (val) manager.SendMessage("VRButtonClicked", name);
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
