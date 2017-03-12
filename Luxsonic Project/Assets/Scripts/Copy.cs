using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// A script to be attached to objects that are to be displayed to the user.
/// This object will have an image attached to it that the user may choose from the loaded images, 
/// and will contain options to control properties of the image through related scripts, as well as being able to
/// control the position and size of the display.
/// </summary>
public class Copy : MonoBehaviour, IVRButton {

	// The width and ehgith of each button
    [SerializeField]
    private float buttonWidth = 100;
    [SerializeField]
    private float buttonHeight = 50;

    [SerializeField]
    private Material copyMaterial;

	// The spawning depth of the buttons
	// TODO: Figure out the actual scale for this matematically
    public float buttonDepth;   

	// The starting position to place the buttons
    private float buttonStartX;
    private float buttonStartY;

	// The transform of the display object in world space
    public Transform myTransform;     
	// The image to render on the display object 
    public SpriteRenderer imageRenderer;  
	// Determines if this instance of a display object is currently selected
    public bool isCurrentImage;  
	// The brigtness of the copy
    public float imageBrightness; 
	// The contrast of the copy
    public float imageContrast;  
	// The current rotation of the copy
    public Vector3 imageRotation;  
	// The current size of the copy
    private bool buttonsVisible;
	//the scale increment for resizing
	[SerializeField]
	private float resizeScale;
    [SerializeField]
    private float brightnessConst;
    [SerializeField]
    private float contrastConst;

    // The buttons for the copy, the buttons are used to allow 
    // modification on the copy (brightness, contrast, etc.)
    private List<VRButton> buttons = new List<VRButton>();
	// The object prefab to use for the buttons
    public VRButton buttonPrefab;
	// the object prefab to use for the slider
    public SliderBar sliderPrefab;
	// The sliders position
    public Vector3 sliderPosition;

	// Indicates whether the brightness slider should be shown
    private enum CurrentSelection { brightness, contrast, resize, rotate, saturation, zoom, filter, close, none};

    private CurrentSelection currentSelection = CurrentSelection.none;

	// The created generic slider
	private SliderBar slider;
	// the scale of the copy
	public float copyScale = 1;

	// Shader for the copy
	private Material curMaterial;
	public Shader curShader;

    // The name of the axis for the left thumbstick
    public string leftThumbX;
    // The name of the axis for hte right thumbstick
    public string rightThumbX;

    private GameObject dashboard;

    private void Start()
    {
        this.dashboard = GameObject.FindGameObjectWithTag("Dashboard");
    }

    /// <summary>
    /// Creates a new Copy object with the Texture2D converted to a sprite stored
    /// in the imageRenderer component.
	/// Pre:: Texture2D image to add
	/// Post:: A new Copy is created for the user to manipulate
	/// Return:: nothing
    /// </summary>
    /// <param name="image"> A Texture2D to use as the image to display to the user </param>
    public void NewCopy(Texture2D image)
    {
		Assert.IsNotNull(image);
		this.imageRenderer = this.GetComponent<SpriteRenderer>();
		this.imageRenderer.sprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f));
		this.myTransform = this.GetComponent<Transform>();
		this.buttonsVisible = false;

		// Get the size of the image sprite and use it to form the bounding box
		Vector2 bbSize = this.GetComponent<SpriteRenderer>().sprite.bounds.size;
		this.GetComponent<BoxCollider>().size = bbSize;
		this.buttonStartX = bbSize.x;
		this.buttonStartY = bbSize.y;

        this.imageRenderer.enabled = true;
        this.imageRenderer.sharedMaterial = new Material(this.curShader);
        this.curMaterial = this.imageRenderer.sharedMaterial;

		this.curMaterial.SetFloat ("_BrightnessAmount", 1);
		this.curMaterial.SetFloat ("_ContrastAmount", 1);
		this.curMaterial.SetFloat ("_SaturationAmount", 1);
    }


    public void Selected()
    {
        this.isCurrentImage = !this.isCurrentImage;
        this.dashboard.SendMessage("CopySelected", this.gameObject);
    }

	/// <summary>
	/// Update this instance with visibile manipulation buttons if it is the current image.
	/// </summary>
    void Update()
    {
        if (this.isCurrentImage)
        {
            switch (currentSelection)
            {
                case CurrentSelection.brightness:
                    this.Brightness(Input.GetAxis(this.rightThumbX));
                    break;

                case CurrentSelection.contrast:
                    this.Contrast(Input.GetAxis(this.rightThumbX));
                    break;

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
	/// Pre:: the button exists
	/// Post:: An action is executed depending on the button
	/// Return:: nothing
    /// </summary>
    /// <param name="button">The name of the button clicked</param>
    public void VRButtonClicked(string button)
    {
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
                DestroyImmediate(this.gameObject);
                break;

            default:        // This should never happen
                Assert.IsTrue(false, "Undefined button");
                break; 

        }
    }

    public void ReceiveSlider(SliderBar slider)
    {
        this.slider = slider;
    }

    /// <summary>
    /// Returns the list of buttons for this display
    /// </summary>
    /// <returns>The list of buttons</returns>
    public List<VRButton> GetButtons()
    {
        return this.buttons;
    }

    /// <summary>
    /// Used mainly for testing with a mouse
    /// </summary>
    private void OnMouseDown()
    {
        this.isCurrentImage = !this.isCurrentImage;
        this.dashboard.SendMessage("CopySelected", this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        this.isCurrentImage = !this.isCurrentImage;
        this.dashboard.SendMessage("CopySelected", this.gameObject);
    }

    /// <summary>
    /// A slider to adjust the brightness is instantiated
    /// Pre:: The brightness button was clicked
    /// Post:: A slider has been instantiated
    /// Return:: nothing
    /// </summary>
    private void Brightness(float input)
    {
        if(input > 0)
        {
            this.slider.Setup(this.curMaterial.GetFloat("_BrightnessAmount") / 2);
            this.curMaterial.SetFloat("_BrightnessAmount", (this.curMaterial.GetFloat("_BrightnessAmount") + this.brightnessConst));
        }
        else if(input < 0)
        {
            this.slider.Setup(this.curMaterial.GetFloat("_BrightnessAmount") / 2);
            this.curMaterial.SetFloat("_BrightnessAmount", (this.curMaterial.GetFloat("_BrightnessAmount") - this.brightnessConst));
        }
    }

	private void Contrast(float input)
	{
        if (input > 0)
        {
            this.slider.Setup(this.curMaterial.GetFloat("_ContrastAmount") / 2);
            this.curMaterial.SetFloat("_ContrastAmount", (this.curMaterial.GetFloat("_ContrastAmount") + this.contrastConst));
        }
        else if (input < 0)
        {
            this.slider.Setup(this.curMaterial.GetFloat("_ContrastAmount") / 2);
            this.curMaterial.SetFloat("_ContrastAmount", (this.curMaterial.GetFloat("_ContrastAmount") - this.contrastConst));
        }
    }

	/// <summary>
	/// The Slider updates the image depending on the value being modified
	/// Pre:: The Slider has been instantiated and returns a value
	/// Post:: The image is updated according to the Slider value
	/// Return:: nothing
	/// </summary>
	/// <returns>The update.</returns>
	/// <param name="value">Value.</param>
    /*public void SliderUpdate(float value)
    {
		// If the brightness is on, update the value of the image according to the slider
		if (this.brightnessOn) {
			Debug.Log ("VALUE " + value);
			this.curMaterial.SetFloat ("_BrightnessAmount", (value * 2));
		} else if (this.contrastOn) {
			this.curMaterial.SetFloat ("_ContrastAmount", (value * 2));
		}
        
    }*/


    /// <summary>
    /// Set the material for the copy
    /// </summary>
    /// <param name="mat">The material to set for the copy.</param>
    public void SetCopyMaterial(Material mat)
    {
        this.copyMaterial = mat; ;
    }

	public void Resize(float dir){
		if (dir > 0) {
			Vector3 scale = this.transform.localScale;
			this.transform.localScale = new Vector3 (scale.x * resizeScale, scale.y * resizeScale, scale.z * resizeScale);
		}
		else if (dir < 0) {
			Vector3 scale = this.transform.localScale;
			this.transform.localScale = new Vector3 (scale.x / resizeScale, scale.y / resizeScale, scale.z / resizeScale);
		}
	}

}
