﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

using buttons;

/// <summary>
/// A script to be attached to objects that are to be displayed to the user.
/// This object will have an image attached to it that the user may choose from the loaded images, 
/// and will contain options to control properties of the image through related scripts, as well as being able to
/// control the position and size of the copy.
/// </summary>
public class Copy : MonoBehaviour
{
	// The component to render the image on the copy object 
	private SpriteRenderer imageRenderer;

	// Determines if this instance of a copy object is currently selected
	public bool isCurrentImage;
	// The current rotation of the copy
	[SerializeField]
	private Vector3 imageRotation;
	//the scale increment for resizing
	[SerializeField]
	private float resizeScale;
	// The scale increment for brightness
	[SerializeField]
	private float brightnessConst;
	// The scale increment for contrast
	[SerializeField]
	private float contrastConst;
	[SerializeField]
	private float saturationConst;


	// An enum used to determine which modification is currently being made to the image
	private enum CurrentSelection { brightness, contrast, resize, invert, saturation, zoom, close, restore, none };

	// The current selection, defaults to none
	private CurrentSelection currentSelection = CurrentSelection.none;

	// the scale of the copy
	public float copyScale = 1;

	// Shader for the copy
	private Material curMaterial;
	public Shader curShader;

	// The name of the axis for the left thumbstick
	public string leftThumbX;
	// The name of the axis for hte right thumbstick
	public string rightThumbX;

	// The dashboard to for the copy to talk to
	private GameObject dashboard;

	[SerializeField]
	private int outlineScale;

	[SerializeField]
	private float outlineDepth;



	private Texture2D outlineTexture;

	// Define the maximum and minimum size for the copy to be resized to 
	[SerializeField]
	private float maxSize = 100;
	[SerializeField]
	private float minSize = 0.1f;

	// Define the max and min brightness for the images
	[SerializeField]
	private float maxBrightness = 2;
	[SerializeField]
	private float minBrightness = 0;

	// Define the max and min contrast for the images
	[SerializeField]
	private float maxContrast = 2;
	[SerializeField]
	private float minContrast = 0;

	// Define the max and min contrast for the images
	[SerializeField]
	private float maxSaturation = 2;
	[SerializeField]
	private float minSaturation = 0;

	// Places to save the default values of the image
	private float defaultBrightness;
	private float defaultContrast;
	private float defaultSaturation;
	private Vector3 defaultSize;

	//true if a copy is currently being pressed
	private bool pressed;

	private bool invert = false;

	private void Start()
	{
		// Find the dashboard
		this.dashboard = GameObject.FindGameObjectWithTag("Dashboard");
		//while (CollideCheck()) ;

	}

	//public bool CollideCheck()
	//{
	//    foreach(GameObject obj in GameObject.FindGameObjectsWithTag("grabbable")){
	//        if (this.gameObject.GetComponent<Collider>().bounds.Intersects(obj.GetComponent<Collider>().bounds))
	//        {
	//            //this.transform.position = 
	//            return true;
	//        }
	//    }
	//    return false;

	//}

	/// <summary>
	/// Creates a new Copy object with the Texture2D converted to a sprite stored
	/// in the imageRenderer component.
	/// </summary>
	/// <pre>Texture2D image to add</pre>
	/// <post>A new copy is created for the user to manipulate</post>
	/// <param name="image"> A Texture2D to use as the image to display to the user </param>
	public void NewCopy(Texture2D image)
	{
		Assert.IsNotNull(image);
		this.imageRenderer = this.GetComponent<SpriteRenderer>();
		this.imageRenderer.sprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f));


		// Get the size of the image sprite and use it to form the bounding box
		Vector2 bbSize = this.GetComponent<SpriteRenderer>().sprite.bounds.size;
		this.GetComponent<BoxCollider>().size = bbSize;

		// Set up the image renderer and add our material to it
		this.imageRenderer.enabled = true;
		this.imageRenderer.sharedMaterial = new Material(this.curShader);
		this.curMaterial = this.imageRenderer.sharedMaterial;

		// Set all shader values to 1 as default
		this.curMaterial.SetFloat("_BrightnessAmount", 1);
		this.curMaterial.SetFloat("_ContrastAmount", 1);
		this.curMaterial.SetFloat("_SaturationAmount", 1);

		// Save initial values
		this.defaultBrightness = this.curMaterial.GetFloat("_BrightnessAmount");
		this.defaultContrast = this.curMaterial.GetFloat("_ContrastAmount");
		this.defaultSaturation = this.curMaterial.GetFloat ("_SaturationAmount");
		this.defaultSize = this.gameObject.transform.localScale;

		this.outlineTexture = new Texture2D(this.gameObject.GetComponent<SpriteRenderer>().sprite.texture.width + this.outlineScale, this.gameObject.GetComponent<SpriteRenderer>().sprite.texture.height + this.outlineScale);

		this.transform.GetChild(0).transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + this.outlineDepth);
		this.transform.GetChild(0).transform.rotation = this.transform.rotation;
		this.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(outlineTexture, new Rect(0, 0, this.outlineTexture.width, this.outlineTexture.height), new Vector2(0.5f, 0.5f));
		this.transform.GetChild(0).gameObject.SetActive(false);
	}

	/// <summary>
	/// Update this instance with visibile manipulation buttons if it is the current image.
	/// </summary>
	void Update()
	{
		// If we are the current image...
		if (this.isCurrentImage && this.transform.parent == null)
		{ 
			// Check our current selection
			switch (currentSelection)
			{
			// If brightness is selected edit brightness
			case CurrentSelection.brightness:
				this.Brightness(Input.GetAxis(this.rightThumbX));
				break;

				// If contrast is selected edit contrast
			case CurrentSelection.contrast:
				this.Contrast(Input.GetAxis(this.rightThumbX));
				break;

				// If resize if selected edit the size 
			case CurrentSelection.resize:
				this.Resize(Input.GetAxis(this.rightThumbX));
				break;

			case CurrentSelection.saturation:
				this.Saturation(Input.GetAxis(this.rightThumbX));
				break;

			default:
				break;

			}
		}
	}


	/// <summary>
	/// When a button is clicked, execute the code associated with that button
	/// </summary>
	/// <pre>A VR button whose manager is the copy has been pressed</pre>
	/// <post>An action is executed, depending on the selected button</post>
	/// <param name="button">The name of the button clicked</param>
	public void NewOptions(ButtonType button)
	{
		switch (button)
		{
		case ButtonType.NONE:
			this.currentSelection = CurrentSelection.none;
			break;

		case ButtonType.CONTRAST_BUTTON:    // Contrast button clicked
			this.currentSelection = CurrentSelection.contrast;
			break;

		case ButtonType.INVERT_BUTTON:    // Rotate button clicked
			this.currentSelection = CurrentSelection.none;
			this.invert = !this.invert;
			this.Invert();
			break;

		case ButtonType.ZOOM_BUTTON:    // Zoom button clicked
			this.currentSelection = CurrentSelection.zoom;
			break;

		case ButtonType.BRIGHTNESS_BUTTON:    // Brightness button clicked
			this.currentSelection = CurrentSelection.brightness;
			break;

		case ButtonType.RESIZE_BUTTON:    // Resize button clicked
			this.currentSelection = CurrentSelection.resize;
			break;

		case ButtonType.SATURATION_BUTTON:    // Filter button clicked
			this.currentSelection = CurrentSelection.saturation;
			break;

		case ButtonType.RESTORE_COPY_BUTTON:     // Restore button clicked
			this.RestoreDefaults();
			break;

		case ButtonType.CLOSE_BUTTON:    // Close button clicked

			//set current selection to none after copy has been closed
			this.currentSelection = CurrentSelection.none;
			// If the object is being held by the hand...
			if(this.transform.parent != null && this.transform.parent.gameObject.tag == "Hand")
			{
				// Tell the hand to drop it like its hot
				this.transform.parent.gameObject.SendMessage("Drop");
			}
			SafeDelete(this.gameObject);
			break;

		default:        // This happens when no options are selected

			break;

		}
	}


	private void SafeDelete(GameObject obj){
		if (Application.isEditor) {
			Destroy (obj);
		} else {
			DestroyImmediate(obj);
		}
	}



	/// <summary>
	/// Called when the object is interacted with in VR
	/// </summary>
	private void Selected()
	{
		// Toggle isCurrent image and notify the dashboard that this copy has been interacted with
		this.isCurrentImage = !this.isCurrentImage;
		if (isCurrentImage)
		{
			this.transform.GetChild(0).gameObject.SetActive(true);
		}
		else
		{
			this.transform.GetChild(0).gameObject.SetActive(false);
			this.currentSelection = CurrentSelection.none;
		}
		this.dashboard.SendMessage("CopySelected", this.gameObject);
	}


	/// <summary>
	/// Used mainly for testing with a mouse
	/// </summary>
	private void OnMouseDown()
	{
		this.isCurrentImage = !this.isCurrentImage;
		this.dashboard.SendMessage("CopySelected", this.gameObject);
	}


	/// <summary>
	/// Adjusts the brightness of the image attached to the copy according to the input.
	/// The input is based on the Unity input axis system.  A positive input will increase the brightness
	/// and a negative input will decrease the brightness.  The brightnessConst value is used to change the value.
	/// 
	/// <pre>The brightness button is seleced and this.isCurrentImage is true</pre>
	/// <post>The brightness of the associated image has been changed </post>
	/// </summary>
	private void Brightness(float input)
	{
		// If the input is positive, we are increasing the brightness
		if (input > 0 && this.curMaterial.GetFloat("_BrightnessAmount") < this.maxBrightness)
		{
			this.curMaterial.SetFloat("_BrightnessAmount", (this.curMaterial.GetFloat("_BrightnessAmount") + this.brightnessConst));
		}
		// Otherwise, decrease the brightness
		else if (input < 0 && this.curMaterial.GetFloat("_BrightnessAmount") > this.minBrightness)
		{
			this.curMaterial.SetFloat("_BrightnessAmount", (this.curMaterial.GetFloat("_BrightnessAmount") - this.brightnessConst));
		}
	}

	/// <summary>
	/// Adjusts the contrast of the image attached to the copy according to the input.
	/// The input is based on the Unity input axis system.  A positive input will increase the contrast
	/// and a negative input will decrease the contrast.  The contrastConst value is used to change the value.
	/// 
	/// <pre>The contrast button is seleced and this.isCurrentImage is true</pre>
	/// <post>The contrast of the associated image has been changed </post>
	/// </summary>
	private void Contrast(float input)
	{
		// If the input is positive, increase the contrast
		if (input > 0 && this.curMaterial.GetFloat("_ContrastAmount") < this.maxContrast)
		{
			this.curMaterial.SetFloat("_ContrastAmount", (this.curMaterial.GetFloat("_ContrastAmount") + this.contrastConst));
		}
		// Otherwise, decrease the contrast
		else if (input < 0 && this.curMaterial.GetFloat("_ContrastAmount") > this.minContrast)
		{
			this.curMaterial.SetFloat("_ContrastAmount", (this.curMaterial.GetFloat("_ContrastAmount") - this.contrastConst));
		}
	}

	/// <summary>
	/// Adjusts the saturation of the image attached to the copy according to the input.
	/// The input is based on the Unity input axis system.  A positive input will increase the saturation
	/// and a negative input will decrease the saturation.  The saturationConst value is used to change the value.
	/// 
	/// <pre>The saturation button is seleced and this.isCurrentImage is true</pre>
	/// <post>The saturation of the associated image has been changed </post>
	/// </summary>
	private void Saturation(float input)
	{
		// If the input is positive, we are increasing the saturation

		if (input > 0 && this.curMaterial.GetFloat("_SaturationAmount") < this.maxSaturation)
		{
			this.curMaterial.SetFloat("_SaturationAmount", (this.curMaterial.GetFloat("_SaturationAmount") + this.saturationConst));
		}
		// Otherwise, decrease the brightness
		else if (input < 0 && this.curMaterial.GetFloat("_SaturationAmount") > this.minSaturation)
		{
			this.curMaterial.SetFloat("_SaturationAmount", (this.curMaterial.GetFloat("_SaturationAmount") - this.saturationConst));
		}
	}

	/// <summary>
	/// Adjusts the size of the image attached to the copy according to the input.
	/// The input is based on the Unity input axis system.  A positive input will increase the size
	/// and a negative input will decrease the size.  The resizeConst value is used to change the value.
	/// 
	/// <pre>The resize button is seleced and this.isCurrentImage is true</pre>
	/// <post>The size of the associated image has been changed </post>
	/// </summary>
	public void Resize(float input)
	{

		// If the input is positive and we are not too big, increase the size
		if ((input > 0) && (this.transform.localScale.x < this.maxSize) && (this.transform.localScale.y < this.maxSize))
		{
			Vector3 scale = this.transform.localScale;
			this.transform.localScale = new Vector3(scale.x * resizeScale, scale.y * resizeScale, scale.z * resizeScale);
		}
		// Otherwise if we are not too small, decrease the size
		else if ((input < 0) && (this.transform.localScale.x > this.minSize) && (this.transform.localScale.y > this.minSize))
		{
			Vector3 scale = this.transform.localScale;
			this.transform.localScale = new Vector3(scale.x / resizeScale, scale.y / resizeScale, scale.z / resizeScale);
		}
	}

	public void Invert(){
		if (this.invert) {
			this.curMaterial.SetInt ("_Invert", 1);
		} else {
			this.curMaterial.SetInt ("_Invert", 0);
		}
	}

	/// <summary>
	/// Restores the copy values to their original default values
	/// </summary>
	/// <post>The copy values will be set to what they were when the copy was first loaded</post>
	public void RestoreDefaults()
	{
		this.curMaterial.SetFloat("_BrightnessAmount", this.defaultBrightness);
		this.curMaterial.SetFloat("_ContrastAmount", this.defaultContrast);
		this.curMaterial.SetFloat("_SaturationAmount", this.defaultSaturation);
		this.transform.localScale = this.defaultSize;
		this.invert = false;
		this.Invert ();
	}

	//===================================
	// Test hooks
	//===================================

	/// <summary>
	/// Returns the material being used by the copy
	/// </summary>
	/// <returns>The material being used by the copy</returns>
	public Material GetMaterial()
	{
		return this.curMaterial;
	}

	/// <summary>
	/// Returns the brightness constant being used for the image
	/// </summary>
	/// <returns>The brightness constant being used for the image</returns>
	public float GetBrightnessConst()
	{
		return this.brightnessConst;
	}

	/// <summary>
	/// Returns the contrast constant being used for the image
	/// </summary>
	/// <returns>The contrast constant being used for the image</returns>
	public float GetContrastConst()
	{
		return this.contrastConst;
	}

	/// <summary>
	/// Returns the resize scale being used for the image
	/// </summary>
	/// <returns>The resize scale being used for the image</returns>
	public float GetResizeScale()
	{
		return this.resizeScale;
	}

	/// <summary>
	/// Test hook for testing the private functionality of this class
	/// </summary>
	/// <param name="testValue"></param>
	public void TestPrivateAttributes(float testValue, string func)
	{
		switch (func.ToLower())
		{
		case "brightness":
			this.brightnessConst = 0.1f;
			this.Brightness(testValue);
			break;
		case "contrast":
			this.contrastConst = 0.1f;
			this.Contrast(testValue);
			break;
		case "resize":
			this.resizeScale = 0.1f;
			this.Resize(testValue);
			break;

		default:
			break;

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
		if (val) this.Selected();
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