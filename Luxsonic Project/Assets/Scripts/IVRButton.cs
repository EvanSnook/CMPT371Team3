﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The IVRButton interface is an interface to be used with the IVRButton class.  It contains
/// the methods required to use a VRButton
/// </summary>
public interface IVRButton
{
    /// <summary>
    /// This method is called by the VRButton when it is interacted with.
    /// The button sends its name as a parameter to this function using unity's
    /// built in message passing system.  The buttons name can then be used in a
    /// switch-case statement in the desired class to determine the functionality 
    /// it should carry out.
    /// </summary>
    /// <param name="button"></param>
    void VRButtonClicked(string button);
}