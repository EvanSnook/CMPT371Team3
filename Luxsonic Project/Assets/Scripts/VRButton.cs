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

    public VRButton(ButtonAttributes attributes)
    {
        this.attributes = attributes;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="attributes"></param>
    /// <param name="manager"></param>
    public void Initialise(ButtonAttributes attributes, GameObject manager)
    {
        this.manager = manager;
        this.attributes = attributes;
        transform.localPosition = attributes.position;
        gameObject.transform.GetChild(0).GetComponent<TextMesh>().text = attributes.buttonName;
        defaultLocalScale = gameObject.transform.localScale;
    }


    /// <summary>
    /// 
    /// </summary>
    public void ResetPosition()
    {
        transform.localPosition = attributes.position;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="val"></param>
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
                    Invoke("UnpressButton", 1f);
                }
            }
        }
    }


    /// <summary>
    /// GetPressed returns the value of pressed
    /// Pre:: 
    /// Post:: 
    /// Return:: value of pressed
    /// </summary>
    public bool GetPressed()
    {
        return this.pressed;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string GetName()
    {
        return this.attributes.buttonName;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPosition()
    {
        return this.attributes.position;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
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
    /// 
    /// </summary>
    /// <param name="path"></param>
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
    /// 
    /// </summary>
    private void DepressButton()
    {
        gameObject.transform.localScale = new Vector3(this.defaultLocalScale.x, this.defaultLocalScale.y, 50f);
    }


    /// <summary>
    /// 
    /// </summary>
    private void UnpressButton()
    {
        gameObject.transform.localScale = this.defaultLocalScale;
    }
}