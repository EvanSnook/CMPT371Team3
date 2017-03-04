using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The IVRSlider is an interface to use the VR Slider object as part of a 
/// user interface.
/// </summary>
public interface IVRSlider {

    /// <summary>
    /// Called by the slider when its value has been changed
    /// </summary>
    /// <param name="value">The value being returned by the slider</param>
    void SliderUpdate(float value);
}
