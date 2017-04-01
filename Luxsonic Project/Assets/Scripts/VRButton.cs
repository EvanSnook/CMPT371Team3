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
	bool pressed = false;
	// Freashly made buttons cannot be pushed
	[SerializeField]
	private int coolDown;
	// Use this for initialization
	private void FixedUpdate()
	{
		if(coolDown > 0)
		{
			coolDown--;
		}
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
		if (val && coolDown == 0)
		{
			if (type == ButtonType.FILE_BUTTON)
			{
				String[] arguments = new string[2] { "f", path };
				manager.SendMessage("LoadFiles", arguments);
			}
			else if (type == ButtonType.DIRECTORY_BUTTON)
			{
				manager.SendMessage("EnterDirectory", path);
			}
			else if (type == ButtonType.LOAD_DIRECTORY_BUTTON)
			{
				String[] arguments = new string[2] { "d", path };
				manager.SendMessage("LoadFiles", arguments);
			}
			else
			{
				manager.SendMessage("VRButtonClicked", type);
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
		return pressed;
	}
}