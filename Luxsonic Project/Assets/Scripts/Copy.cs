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
		this.imageRenderer = this.GetComponent<SpriteRenderer>();
		this.imageRenderer.sprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f));
		this.myTransform = this.GetComponent<Transform>();
		this.buttonsVisible = false;

		// Get the size of the image sprite and use it to form the bounding box
		Vector2 bbSize = this.GetComponent<SpriteRenderer>().sprite.bounds.size;
		this.GetComponent<BoxCollider>().size = bbSize;
		this.buttonStartX = bbSize.x;
		this.buttonStartY = bbSize.y;
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
//			Light light = this.slider.manager.GetComponent<Light> ();
			this.slider.Setup(0);
            this.brightnessOn = true;
			AdjustBrightness (52);
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
			Debug.Log ("VALUE " + value);
//			AdjustBrightness ((int)(value * 100));
        }//else if ()
        
    }

	private void AdjustBrightness(int brightness){
		Debug.Log ("BRIGHTNESS " + brightness);
		int brightnessInt = Convert.ToInt32(brightness);
		int mappedBrightness = (51 * brightnessInt) / 10 - 255;
		//Make an empty Texture the same same as the original 
		Texture2D bitmapImage = new Texture2D(imageRenderer.sprite.texture.width, imageRenderer.sprite.texture.height);
		Texture2D original = this.imageRenderer.sprite.texture;
		Debug.Log (bitmapImage);
		if (mappedBrightness < -255) mappedBrightness = -255;
		if (mappedBrightness > 255) mappedBrightness = 255;
		Color32 color;
		for (int i = 0; i < bitmapImage.width; i++)
		{
			for (int j = 0; j < bitmapImage.height; j++)
			{
				color = original.GetPixel(i, j);
				int cR = (int)color.r + mappedBrightness;
				int cG = (int)color.g + mappedBrightness;
				int cB = (int)color.b + mappedBrightness;
				if (cR < 0) cR = 0;
				if (cR > 255) cR = 255;
				if (cG < 0) cG = 0;
				if (cG > 255) cG = 255;
				if (cB < 0) cB = 0;
				if (cB > 255) cB = 255;
				bitmapImage.SetPixel(i, j, new Color32((byte) cR, (byte) cG, (byte) cB, 255));
			}
		}
		//Apply all SetPixel changes
		bitmapImage.Apply();
		//Connect texture to material of GameObject this script is attached to 
		//this.GetComponent<SpriteRenderer>().sprite.texture = bitmapImage;
		this.imageRenderer.sprite = Sprite.Create(bitmapImage, 
			new Rect(0, 0, bitmapImage.width, bitmapImage.height), 
			new Vector2(0.5f, 0.5f));
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
