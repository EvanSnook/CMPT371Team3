using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using buttons;

[System.Serializable]
public class ButtonAttributes
{
    // Button name
    public string buttonName;

    // type of button
    public ButtonType type;

    // The local position of the button, relative to its parent plane
    public Vector3 position;

    // The local rotation of the button, relative to its parent plane
    public Vector3 rotation;

    // defines if the button gets "pushed in" when pressed
    public bool depressable;

    // defines if the button automatically pushes out after a certain amount of time
    public bool autoPushOut;

    //name of function to call when button is pushed
    public string buttonFunction;

    // parameters for potential functions. Used if necessary
    public string[] buttonParameters;
}
