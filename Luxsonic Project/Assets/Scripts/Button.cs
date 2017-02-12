using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class defines an interactable 3D button for use with VR
/// A class using this button must have a 'ButtonClicked(string name)' function
/// </summary>
public class Button : MonoBehaviour {

    public string name;
    public GameObject manager;  // The object that creates and contains the functionality for this button.
    public TextMesh textObject; 

    public void OnCollisionEnter(Collision collision)
    {
        manager.SendMessage("ButtonClicked", name);
        textObject.color = Color.black;
    }
    // Use this for initialization
    void Start () {
        textObject = gameObject.GetComponentInChildren<TextMesh>();
        textObject.text = name;
	}
}
