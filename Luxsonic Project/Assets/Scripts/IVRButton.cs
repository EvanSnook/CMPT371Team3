using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using buttons;

namespace buttons
{
    public enum ButtonType
    {
        MENU_BUTTON, COPY_MODIFIER, FILE_BUTTON, DIRECTORY_BUTTON,
    };
}


/// <summary>
/// The IVRButton interface is an interface to be used with the IVRButton class.  It contains
/// the methods required to use a VRButton
/// </summary>
public interface IVRButton
{
    /// <summary>
    /// Creates new button, and applies passed in attributes. 
    /// </summary>
    /// <param name="attributes">Attributes to be applied to the new button</param>
    /// <param name="prefab">Prefab to instantiate as a button</param>
    /// <returns>Newly created button GameObject</returns>
    GameObject CreateButton(ButtonAttributes attributes, GameObject prefab);
}