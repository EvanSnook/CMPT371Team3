using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


/// <summary>
/// A script to be attached to objects that are to be displayed to the user.
/// This object will have an image attached to it that the user may choose from the loaded images, 
/// and will contain options to control properties of the image through related scripts, as well as being able to
/// control the position and size of the copy.
/// </summary>
public class Copy : MonoBehaviour, IVRButton
{
    // The transform of the copy object in world space
    private Transform myTransform;
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


    // An enum used to determine which modification is currently being made to the image
    private enum CurrentSelection { brightness, contrast, resize, rotate, saturation, zoom, filter, close, none };
    
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

    private void Start()
    {
        // Find the dashboard
        this.dashboard = GameObject.FindGameObjectWithTag("Dashboard");
    }

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
        this.myTransform = this.GetComponent<Transform>();
        

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
    }

    /// <summary>
    /// Update this instance with visibile manipulation buttons if it is the current image.
    /// </summary>
    void Update()
    {
        // If we are the current image...
        if (this.isCurrentImage)
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
    public void VRButtonClicked(string button)
    {
        Assert.IsNotNull(button);
        switch (button)
        {
            case "Contrast":    // Contrast button clicked
                this.currentSelection = CurrentSelection.contrast;
                break;

            case "Rotate":    // Rotate button clicked
                // TODO: Implement Rotate code
                break;

            case "Zoom":    // Zoom button clicked
                // TODO: Implement Zoom code
                break;

            case "Brightness":    // Brightness button clicked
                this.currentSelection = CurrentSelection.brightness;
                break;

            case "Resize":    // Resize button clicked
                this.currentSelection = CurrentSelection.resize;
                break;

            case "Filter":    // Filter button clicked
                // TODO: Implement Filter code
                break;

            case "Close":    // Close button clicked
                this.currentSelection = CurrentSelection.close;
                this.gameObject.SetActive(false);
                //DestroyImmediate(this.gameObject);
                break;

            default:        // This should never happen
                Assert.IsTrue(false, "Undefined button");
                break;

        }
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
    /// Called when the object is interacted with in VR
    /// </summary>
    /// <param name="collision">The collision detected</param>
    private void OnCollisionEnter(Collision collision)
    {
        // Toggle isCurrent image and notify the dashboard that this copy has been interacted with
        this.isCurrentImage = !this.isCurrentImage;
		if (isCurrentImage) {
			this.transform.GetChild (0).gameObject.SetActive (true);
		} else {
			this.transform.GetChild (0).gameObject.SetActive (false);
		}
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
        if (input > 0)
        {
            this.curMaterial.SetFloat("_BrightnessAmount", (this.curMaterial.GetFloat("_BrightnessAmount") + this.brightnessConst));
        }
        // Otherwise, decrease the brightness
        else if (input < 0)
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
        if (input > 0)
        {
            this.curMaterial.SetFloat("_ContrastAmount", (this.curMaterial.GetFloat("_ContrastAmount") + this.contrastConst));
        }
        // Otherwise, decrease the contrast
        else if (input < 0)
        {
            this.curMaterial.SetFloat("_ContrastAmount", (this.curMaterial.GetFloat("_ContrastAmount") - this.contrastConst));
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
        // If the input is positive, increase the size
        if (input > 0)
        {
            Vector3 scale = this.transform.localScale;
            this.transform.localScale = new Vector3(scale.x * resizeScale, scale.y * resizeScale, scale.z * resizeScale);
        }
        // Otherwise, decrease the size
        else if (input < 0)
        {
            Vector3 scale = this.transform.localScale;
            this.transform.localScale = new Vector3(scale.x / resizeScale, scale.y / resizeScale, scale.z / resizeScale);
        }
    }

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
        switch (func.ToLower()) {
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
}
