using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using buttons;
namespace buttons
{
    public enum ButtonType
    {
        LOAD_BUTTON, QUIT_BUTTON, MINIMIZE_BUTTON, CONTRAST_BUTTON,
        ROTATE_BUTTON, ZOOM_BUTTON, BRIGHTNESS_BUTTON,
        RESIZE_BUTTON, FILTER_BUTTON, CLOSE_BUTTON, RESTORE_COPY_BUTTON, SELECT_ALL_COPIES_BUTTON,
        DESELECT_ALL_COPIES_BUTTON, TRAY_SCROLL_UP_BUTTON, TRAY_SCROLL_DOWN_BUTTON, NONE, DIRECTORY_BUTTON, FILE_BUTTON,
		BACK_BUTTON, CANCEL_BUTTON, LEFT_BUTTON, RIGHT_BUTTON, FILE_DOWN, FILE_UP, DIRECTORY_DOWN, DIRECTORY_UP,LOAD_DIRECTORY_BUTTON,

    };
}
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
    void VRButtonClicked(ButtonType button);
}