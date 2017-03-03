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

    [SerializeField]
    private float buttonWidth = 100;
    [SerializeField]
    private float buttonHeight = 50;

    public float buttonDepth;   // The spawning depth of the buttons
                                // TODO: Figure out the actual scale for this matematically

    private float buttonStartX;
    private float buttonStartY;

    public Transform myTransform;     // The transform of the display object in world space
    public SpriteRenderer imageRenderer;   // The image to render on the display object 
    public bool isCurrentImage;     // Determines if this instance of a display object is currently selected
    public float imageBrightness;   // The brigtness of the display
    public float imageContrast;     // The contrast of the display
    public Vector3 imageRotation;   // The current rotation of the display
    public float currentSize;       // The current size of the display
    private bool buttonsVisible;

    private List<VRButton> buttons = new List<VRButton>();
    public VRButton button;
    public SliderBar slider;

    public Vector3 sliderPosition;

    private bool brightnessOn = false;
    private SliderBar brightnessSlider;
    

    /// <summary>
    /// Creates a new Copy object with the Texture2D converted to a sprite stored
    /// in the imageRenderer component.
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


    public void OnTriggerEnter(Collider other)
    {
        this.isCurrentImage = true;
    }

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
        VRButton contrastButton = Instantiate(button, contrastButtonPosition, new Quaternion(0, 0, 0, 0));
        contrastButton.name = "Contrast";
        contrastButton.manager = this.gameObject;

        VRButton rotateButton = Instantiate(button, rotateButtonPosition, new Quaternion(0, 0, 0, 0));
        rotateButton.name = "Rotate";
        rotateButton.manager = this.gameObject;

        VRButton zoomButton = Instantiate(button, zoomButtonPosition, new Quaternion(0, 0, 0, 0));
        zoomButton.name = "Zoom";
        zoomButton.manager = this.gameObject;

        VRButton brightnessButton = Instantiate(button, brightnessButtonPosition, new Quaternion(0, 0, 0, 0));
        brightnessButton.name = "Brightness";
        brightnessButton.manager = this.gameObject;

        VRButton resizeButton = Instantiate(button, resizeButtonPosition, new Quaternion(0, 0, 0, 0));
        resizeButton.name = "Resize";
        resizeButton.manager = this.gameObject;

        VRButton filterButton = Instantiate(button, filterButtonPosition, new Quaternion(0, 0, 0, 0));
        filterButton.name = "Filter";
        filterButton.manager = this.gameObject;

        VRButton closeButton = Instantiate(button, closeButtonPosition, new Quaternion(0, 0, 0, 0));
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
                this.brightness();
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
    /// <returns></returns>
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

    private void brightness()
    {
		// If the brightness is not on, rreate a slider and display it in the scene
        if (!this.brightnessOn)
        {
            this.brightnessSlider = Instantiate(slider, sliderPosition, new Quaternion(0, 0, 0, 0));
            this.brightnessSlider.manager = this.gameObject;
			Light light = this.brightnessSlider.manager.GetComponent<Light> ();
			this.brightnessSlider.Setup(light.color.r);
            this.brightnessOn = true;
        }
        else
        {
			// If the brightness button is pressed again, hide the slider
			DestroyImmediate(this.brightnessSlider.gameObject);
            this.brightnessOn = false;
			Debug.Log ("Brightness Close");
        }

    }

    public void SliderUpdate(float value)
    {
		// If the brightness is on, update the value of the image according to the slider
        if (this.brightnessOn)
        {
			Light light = this.GetComponent<Light> ();
			light.color = new Color(value, value, value);
        }//else if ()
        
    }
}
