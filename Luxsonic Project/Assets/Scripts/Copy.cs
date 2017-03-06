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
public class Copy : MonoBehaviour, IVRButton, IVRSlider {

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
    public MeshRenderer imageRenderer;  
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
    private bool brightnessOn = false;
	// The created generic slider
	private SliderBar slider;
	// the scale of the copy
	public float copyScale = 1;
    

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
        this.myTransform = this.GetComponent<Transform>();
        //        this.imageRenderer = this.GetComponent<SpriteRenderer>();
        //        this.imageRenderer.sprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f));
        //		renderer.material.SetTexture("_MyTexture", myTexture);
        this.transform.localScale = new Vector3(image.width * copyScale, image.height * copyScale, 1);
		this.imageRenderer = this.GetComponent<MeshRenderer>();
        this.copyMaterial.mainTexture = image;
        this.imageRenderer.sharedMaterial = copyMaterial;
		//this.imageRenderer.sharedMaterial.SetTexture ("_MainTex", image);
        
        
        this.buttonsVisible = false;

        // Get the size of the image sprite and use it to form the bounding box
//        Vector2 bbSize = this.GetComponent<SpriteRenderer>().sprite.bounds.size;
//        this.GetComponent<BoxCollider>().size = bbSize;
//        this.buttonStartX = bbSize.x;
//        this.buttonStartY = bbSize.y;
    }

	/// <summary>
	/// Raises the trigger enter event when the user has selected the Copy
	/// </summary>
	/// <param name="other">Other.</param>
    public void OnTriggerEnter(Collider other)
    {
        this.isCurrentImage = true;
    }

	/// <summary>
	/// Update this instance with visibile manipulation buttons if it is the current image.
	/// </summary>
    void Update()
    {
//        Debug.Log("Current: " + this.isCurrentImage);
//        Debug.Log("Visible: " + this.buttonsVisible);
        // Check if we should create the buttons
        if (this.isCurrentImage && !this.buttonsVisible)
        {
            DisplayButtons();
            this.buttonsVisible = true;
        }

        // Check if we should hide the buttons
        if (!this.isCurrentImage)
        {
            this.buttonsVisible = false;
            HideButtons();
        }
    }

    /// <summary>
    /// Instantiate the buttons for this display
	/// Pre:: A Copy exists to select
	/// Post:: Buttons to manipulate the current Copy is visible
    /// </summary>
    public void DisplayButtons()
    {
        Vector3 contrastButtonPosition = myTransform.position - new Vector3(buttonStartX, buttonStartY, buttonDepth);
        Vector3 rotateButtonPosition = myTransform.position - new Vector3(buttonWidth, 0, buttonDepth);
        Vector3 zoomButtonPosition = myTransform.position - new Vector3(buttonWidth*2, 0, buttonDepth);
        Vector3 brightnessButtonPosition = myTransform.position - new Vector3(buttonWidth*2, buttonHeight, buttonDepth);
        Vector3 resizeButtonPosition = myTransform.position - new Vector3(0, buttonHeight, buttonDepth);
        Vector3 filterButtonPosition = myTransform.position - new Vector3(buttonWidth, buttonHeight, buttonDepth);
        Vector3 closeButtonPosition = myTransform.position - new Vector3(buttonWidth, buttonHeight*2, buttonDepth);

        // Create the buttons
        VRButton contrastButton = Instantiate(buttonPrefab, contrastButtonPosition, new Quaternion(0, 0, 0, 0));
        contrastButton.name = "Contrast";
        contrastButton.manager = this.gameObject;

        VRButton rotateButton = Instantiate(buttonPrefab, rotateButtonPosition, new Quaternion(0, 0, 0, 0));
        rotateButton.name = "Rotate";
        rotateButton.manager = this.gameObject;

        VRButton zoomButton = Instantiate(buttonPrefab, zoomButtonPosition, new Quaternion(0, 0, 0, 0));
        zoomButton.name = "Zoom";
        zoomButton.manager = this.gameObject;

        VRButton brightnessButton = Instantiate(buttonPrefab, brightnessButtonPosition, new Quaternion(0, 0, 0, 0));
        brightnessButton.name = "Brightness";
        brightnessButton.manager = this.gameObject;

        VRButton resizeButton = Instantiate(buttonPrefab, resizeButtonPosition, new Quaternion(0, 0, 0, 0));
        resizeButton.name = "Resize";
        resizeButton.manager = this.gameObject;

        VRButton filterButton = Instantiate(buttonPrefab, filterButtonPosition, new Quaternion(0, 0, 0, 0));
        filterButton.name = "Filter";
        filterButton.manager = this.gameObject;

        VRButton closeButton = Instantiate(buttonPrefab, closeButtonPosition, new Quaternion(0, 0, 0, 0));
        closeButton.name = "Close";
        closeButton.manager = this.gameObject;

        // Add the buttons to the list of buttons
        buttons.Add(contrastButton);
        buttons.Add(rotateButton);
        buttons.Add(zoomButton);
        buttons.Add(brightnessButton);
        buttons.Add(resizeButton);
        buttons.Add(filterButton);
        buttons.Add(closeButton);
    }

    /// <summary>
    /// Hide the buttons from the display by destroying them
	/// Pre:: buttons are visible
	/// Post:: buttons are no longer visible
	/// Return:: nothing
    /// </summary>
    public void HideButtons()
    {
        foreach(VRButton button in buttons)
        {
            DestroyImmediate(button.gameObject);
        }

        buttons.Clear();
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
                // TODO: Implement contrast code
                break;

            case "Rotate":    // Rotate button clicked
                // TODO: Implement Rotate code
                break;

            case "Zoom":    // Zoom button clicked
                // TODO: Implement Zoom code
                break;

            case "Brightness":    // Brightness button clicked
                this.Brightness();
                break;

            case "Resize":    // Resize button clicked
                // TODO: Implement Resize code
                break;

            case "Filter":    // Filter button clicked
                // TODO: Implement Filter code
                break;

            case "Close":    // Close button clicked
                this.HideButtons();
                DestroyImmediate(this.gameObject);
                break;

            default:        // This should never happen
                Assert.IsTrue(false, "Undefined button");
                break; 

        }
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
        this.isCurrentImage = true;
    }

	/// <summary>
	/// A slider to adjust the brightness is instantiated
	/// Pre:: The brightness button was clicked
	/// Post:: A slider has been instantiated
	/// Return:: nothing
	/// </summary>
    private void Brightness()
    {
		// If the brightness is not on, rreate a slider and display it in the scene
        if (!this.brightnessOn)
        {
            this.slider = Instantiate(sliderPrefab, sliderPosition, new Quaternion(0, 0, 0, 0));
            this.slider.manager = this.gameObject;
			Light light = this.slider.manager.GetComponent<Light> ();
			this.slider.Setup(light.color.r);
            this.brightnessOn = true;
        }
        else
        {
			// If the brightness button is pressed again, hide the slider
			DestroyImmediate(this.slider.gameObject);
            this.brightnessOn = false;
			Debug.Log ("Brightness Close");
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
    public void SliderUpdate(float value)
    {
		// If the brightness is on, update the value of the image according to the slider
        if (this.brightnessOn)
        {
			Light light = this.GetComponent<Light> ();
			light.color = new Color(value, value, value);
        }//else if ()
        
    }

    /// <summary>
    /// Set the material for the copy
    /// </summary>
    /// <param name="mat">The material to set for the copy.</param>
    public void SetCopyMaterial(Material mat)
    {
        this.copyMaterial = mat; ;
    }
}
