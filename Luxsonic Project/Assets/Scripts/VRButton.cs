using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using buttons;
using System;
/// <summary>
/// This class defines an interactable 3D button for use with VR
/// A class using this button must implement the IVRButton interface
/// </summary>
public class VRButton : MonoBehaviour
{
	// Button name
	public string buttonName;
	// Button Type
	public ButtonType type;
	// string to store a potental path
	public string path;
	// The object that creates and contains the functionality for this button.
	public GameObject manager;
	// Text on button
	public TextMesh textObject;
	// Buttons state
	private bool pressed;
	// Use this for initialization
	public bool allowPress;
	void Start()
	{
		this.pressed = false;
		allowPress = true;
	}
	// When mouve is pressed send clicked message to manager
	void OnMouseDown()
	{
		if (type == ButtonType.DIRECTORY_BUTTON)
		{
			Debug.Log("Directory button pushed");
			manager.SendMessage("EnterDirectory", path);
		}
		else if (type == ButtonType.FILE_BUTTON)
		{
			manager.SendMessage("ConvertAndSendImage", path);
		}
		else
		{
			manager.SendMessage("VRButtonClicked", type);
		}
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
		if (val)
		{
			if (type == ButtonType.FILE_BUTTON)
			{
				Debug.Log("Hitting a file");
				manager.SendMessage("ConvertAndSendImage", path);
			}
			else if (type == ButtonType.DIRECTORY_BUTTON)
			{
				manager.SendMessage("EnterDirectory", path);
			}
			else
			{
				manager.SendMessage("VRButtonClicked", type);
			}
		} 
		else
		{
			manager.SendMessage("UnpressButton");
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
		return pressed;
	}
}