using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using buttons;
using System;


/// <summary>
/// This class defines an interactable 3D button for use with VR
/// A class using this button must implement the IVRButton interface
/// </summary>
[System.Serializable]
public class VRButton : MonoBehaviour
{
    // holds generic information about the button
    public ButtonAttributes attributes;

    // The local rotation of the button, relative to its parent plane
    private Vector3 defaultLocalScale;

    // The object that creates and contains the functionality for this button.
    public GameObject manager;

    // Text on button
    public TextMesh textObject;

    // Buttons state
    private bool pressed = false;


    /// <summary>
    /// Constructor for the button
    /// </summary>
    /// <param name="attributes">The attributes to create the button with</param>
    public VRButton(ButtonAttributes attributes)
    {
        this.attributes = attributes;
    }


    /// <summary>
    /// Initialize the button with its attributes
    /// </summary>
    /// <param name="attributes">The attributes to create the button with</param>
    /// <param name="manager">The manager object that the button reports to</param>
    public void Initialise(ButtonAttributes attributes, GameObject manager)
    {
        this.manager = manager;
        this.attributes = attributes;
        transform.localPosition = attributes.position;
        gameObject.transform.GetChild(0).GetComponent<TextMesh>().text = attributes.buttonName;
        defaultLocalScale = gameObject.transform.localScale;
    }


    /// <summary>
    /// Reset the position of the button
    /// </summary>
    /// <post>The button's position has been reset</post>
    public void ResetPosition()
    {
        transform.localPosition = attributes.position;
        defaultLocalScale = gameObject.transform.localScale;
    }


    /// <summary>
    /// Called when the button has been clicked.
    /// </summary>
    /// <param name="val">Whether the button can be activated yet</param>
    public void ButtonClicked(bool val)
    {
        this.pressed = val;
        if (val)
        {
            if (!(String.IsNullOrEmpty(attributes.buttonFunction)))
            {
                if (this.attributes.buttonParameters != null)
                {
                    this.manager.SendMessage(attributes.buttonFunction, attributes.buttonParameters);
                }
                else
                {
                    this.manager.SendMessage(attributes.buttonFunction);
                }

                if (this.attributes.depressable)
                {
                    DepressButton();
                }

                if (this.attributes.autoPushOut)
                {
                    TimedUnpress();
                }
            }
        }
    }


    /// <summary>
    /// GetPressed returns the value of pressed
    /// </summary>
    /// <returns>value of pressed</returns>
    public bool GetPressed()
    {
        return this.pressed;
    }


    /// <summary>
    /// Get the name of the button
    /// </summary>
    /// <returns>The name of the button</returns>
    public string GetName()
    {
        return this.attributes.buttonName;
    }


    /// <summary>
    /// Get the position of the button
    /// </summary>
    /// <returns>The position of the button</returns>
    public Vector3 GetPosition()
    {
        return this.attributes.position;
    }


    /// <summary>
    ///  Get the path associated with the button
    /// </summary>
    /// <returns>The path associated with the button</returns>
    public string GetPath()
    {

        if (this.attributes.type == ButtonType.FILE_BUTTON)
        {
            return this.attributes.buttonParameters[0];
        }
        else
        {
            return null;
        }
    }


    /// <summary>
    /// Sets the path associated with the button
    /// </summary>
    /// <param name="path">The path to associate with the button</param>
    public void SetPath(string path)
    {
        if (path == null)
        {
            this.attributes.buttonParameters = null;
        }
        else
        {
            this.attributes.buttonParameters = new string[1];
            this.attributes.buttonParameters[0] = path;
        }
    }


    /// <summary>
    /// Depress the button for visual clicking effect
    /// </summary>
    private void DepressButton()
    {
        gameObject.transform.localScale = new Vector3(this.defaultLocalScale.x, this.defaultLocalScale.y, (this.defaultLocalScale.z/2f));
    }


    /// <summary>
    /// Un-depress the button for visual clicking effect
    /// </summary>
    public void UnpressButton()
    {
        gameObject.transform.localScale = this.defaultLocalScale;
    }


    /// <summary>
    /// Unpress the button after 0.5 seconds
    /// </summary>
    public void TimedUnpress()
    {
        Invoke("UnpressButton", 0.5f);
    }
}