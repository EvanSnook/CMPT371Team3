using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// A script to control the Workspace Manager menu system. 
/// This script creates and defines the funcionality for the menu buttons.
/// </summary>
public class Dashboard : MonoBehaviour, IVRButton {
    
    //[SerializeField]
    //private float buttonWidth = 100; //commented out due to not being used
    //[SerializeField]
    //private float buttonHeight = 50;

    public GameObject loadBar;  // The file system to load images with
    public Display display; // Creates a display object in dashboard

    public Transform myTransform;   
    public VRButton button;       // The button object to use as a button

    // Define where to instantiate the buttons
    //First vector for each button contains position (x,y,z) and second contains rotation (x,y,z)
    // These are the load, quit and minimize buttons seen on runtime
    public Vector3 loadButtonPosition;
    public Vector3 loadButtonRotation;

    public Vector3 quitButtonPosition;
    public Vector3 quitButtonRotation;

    public Vector3 minimizeButtonPosition;
    public Vector3 minimizeButtonRotation;

    public Vector3 contrastButtonPosition;
    public Vector3 contrastButtonRotation;

    public Vector3 rotateButtonPosition;
    public Vector3 rotateButtonRotation;

    public Vector3 zoomButtonPosition;
    public Vector3 zoomButtonRotation;

    public Vector3 brightnessButtonPosition;
    public Vector3 brightnessButtonRotation;

    public Vector3 resizeButtonPosition;
    public Vector3 resizeButtonRotation;

    public Vector3 filterButtonPosition;
    public Vector3 filterButtonRotation;

    public Vector3 closeButtonPosition;
    public Vector3 coseButtonRotation;

    // The sliders position
    public Vector3 sliderPosition;
    

    //Images used to test load functionality
    public Texture2D[] dummyImages;

    // Reference to the buttons
    private VRButton loadButton;
    private VRButton quitButton;
    private VRButton minimizeButton;

    // the object prefab to use for the slider
    public SliderBar sliderPrefab;

    private SliderBar slider;

    //Are the Buttons visable to the user?
    private bool minimized = false;

    public GameObject currentCopy;

    private List<VRButton> copyButtons;

    public void Start()
    {
        this.myTransform = this.GetComponent<Transform>();
        display = GameObject.FindGameObjectWithTag("Display").GetComponent<Display>();
        this.copyButtons = new List<VRButton>();
        DisplayMenu();
    }

    

    /// <summary>
    /// Function DisplayMenu() Creates the load, quit and minimize buttons for the menu
    /// Pre:: nothing
    /// Post:: Creation of the load, quit and minimize buttons
    /// </summary>
    public void DisplayMenu(){

        // Create the load button to access the filesystem
        this.loadButton = Instantiate(button, loadButtonPosition, 
            Quaternion.Euler(loadButtonRotation));
        loadButton.transform.parent = gameObject.transform;

        this.loadButton.name = "Load";
        this.loadButton.manager = this.gameObject;

        this.loadButton.textObject = this.loadButton.GetComponentInChildren<TextMesh>();
        this.loadButton.textObject.text = "Load";



        // Create the Quit button 
        this.quitButton = Instantiate(button, quitButtonPosition,
            Quaternion.Euler(quitButtonRotation));
        quitButton.transform.parent = gameObject.transform;

        this.quitButton.name = "Quit";
        this.quitButton.manager = this.gameObject;
        this.quitButton.textObject = this.quitButton.GetComponentInChildren<TextMesh>();
        this.quitButton.textObject.text = "Quit";

        // Create the Minimize button 
        this.minimizeButton = Instantiate(button, minimizeButtonPosition,
            Quaternion.Euler(minimizeButtonRotation));
        minimizeButton.transform.parent = gameObject.transform;

        this.minimizeButton.name = "Minimize";
        this.minimizeButton.manager = this.gameObject;
        this.minimizeButton.textObject = this.minimizeButton.GetComponentInChildren<TextMesh>();
        this.minimizeButton.textObject.text = "Minimize";

        // Create the buttons
        VRButton contrastButton = Instantiate(button, contrastButtonPosition, new Quaternion(0, 0, 0, 0));
        contrastButton.name = "Contrast";
        contrastButton.manager = this.gameObject;
        contrastButton.textObject = contrastButton.GetComponentInChildren<TextMesh>();
        contrastButton.textObject.text = "Contrast";


        VRButton rotateButton = Instantiate(button, rotateButtonPosition, new Quaternion(0, 0, 0, 0));
        rotateButton.name = "Rotate";
        rotateButton.manager = this.gameObject;
        rotateButton.textObject = rotateButton.GetComponentInChildren<TextMesh>();
        rotateButton.textObject.text = "Rotate";

        VRButton zoomButton = Instantiate(button, zoomButtonPosition, new Quaternion(0, 0, 0, 0));
        zoomButton.name = "Zoom";
        zoomButton.manager = this.gameObject;
        zoomButton.textObject = zoomButton.GetComponentInChildren<TextMesh>();
        zoomButton.textObject.text = "Zoom";

        VRButton brightnessButton = Instantiate(button, brightnessButtonPosition, new Quaternion(0, 0, 0, 0));
        brightnessButton.name = "Brightness";
        brightnessButton.manager = this.gameObject;
        brightnessButton.textObject = brightnessButton.GetComponentInChildren<TextMesh>();
        brightnessButton.textObject.text = "Brightness";

        VRButton resizeButton = Instantiate(button, resizeButtonPosition, new Quaternion(0, 0, 0, 0));
        resizeButton.name = "Resize";
        resizeButton.manager = this.gameObject;
        resizeButton.textObject = resizeButton.GetComponentInChildren<TextMesh>();
        resizeButton.textObject.text = "Resize";

        VRButton filterButton = Instantiate(button, filterButtonPosition, new Quaternion(0, 0, 0, 0));
        filterButton.name = "Filter";
        filterButton.manager = this.gameObject;
        filterButton.textObject = filterButton.GetComponentInChildren<TextMesh>();
        filterButton.textObject.text = "Load";

        VRButton closeButton = Instantiate(button, closeButtonPosition, new Quaternion(0, 0, 0, 0));
        closeButton.name = "Close";
        closeButton.manager = this.gameObject;
        closeButton.textObject = closeButton.GetComponentInChildren<TextMesh>();
        closeButton.textObject.text = "Close";

        this.slider = Instantiate(this.sliderPrefab, this.sliderPosition, new Quaternion(0, 0, 0, 0));

        // Add the buttons to the list of buttons
        copyButtons.Add(contrastButton);
        copyButtons.Add(rotateButton);
        copyButtons.Add(zoomButton);
        copyButtons.Add(brightnessButton);
        copyButtons.Add(resizeButton);
        copyButtons.Add(filterButton);
        copyButtons.Add(closeButton);
    }

    /// <summary>
    /// Function VRButtonClicked is called by a button object when it is interacted with.
    /// Pre:: string representing the name of the button selected
    /// Post:: Execution of funcion associated with string given
    /// Return:: nothing
    /// </summary>
    /// <param name="button">The name of the button</param>
    public void VRButtonClicked(string button)
    {
        switch (button)
        {
            case "Load":
                // If the load button was clicked
                Load();
                break;
            case "Quit":
                // If the quit button was clicked
                Quit();
                break;
            case "Minimize":
                // If the minimize button was clicked
                Minimize();
                break;

            default:    // A copy option was clicked
                if (this.currentCopy != null)
                {
                    this.currentCopy.SendMessage("ReceiveSlider", this.slider);
                    this.currentCopy.SendMessage("VRButtonClicked", button); 
                }
                break;

        }

    }

    public void CopySelected(GameObject copy)
    {
        if (copy.GetComponent<Copy>().isCurrentImage)
        {
            this.currentCopy = copy;
        }
        else
        {
            this.currentCopy = null;
        }
    }

    /// <summary>
    /// Function Quit() adds functionality for the quit button. When called, the program
    /// will terminate
    /// Pre:: nothing
    /// Post:: program termination
    /// Return:: nothing
    /// </summary>
    private void Quit()
    {
        print("Quit Button clicked");
        Application.Quit();
    }

    /// <summary>
    /// Function Load() adds functionality for when the load button is clicked.  Currently
    /// it will select a random image stored in the Assets and add it to the Display
    /// Pre:: nothing
    /// Post:: New Texture2D given and added to the Display class
    /// Return:: nothing
    /// </summary>
    private void Load()
    {
        display.AddImage(dummyImages[Random.Range(0, dummyImages.Length)]);
        //Instantiate(loadBar, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
    }

    /// <summary>
    /// Function Minimize() adds functionality for the minimize button.  It will minimize disable
    /// the other buttons if they are currently visable or make them enabled if they are currently 
    /// disabled
    /// Pre:: nothing
    /// Post:: Buttons set to either Active or Not Active, minimized attribute changed
    /// Return:: nothing
    /// </summary>
    private void Minimize()
    {
        if (this.minimized) {
            
            this.loadButton.gameObject.SetActive(true);
            this.quitButton.gameObject.SetActive(true);
            //this.display.gameObject.SetActive(true);
            foreach (VRButton button in copyButtons)
            {
                button.gameObject.SetActive(true);
            }

            this.slider.gameObject.SetActive(true);
            this.minimized = false;
            

        } else {

            this.loadButton.gameObject.SetActive(false);
            this.quitButton.gameObject.SetActive(false);
            //this.display.gameObject.SetActive(false);

            foreach (VRButton button in copyButtons)
            {
                button.gameObject.SetActive(false);
            }
            this.slider.gameObject.SetActive(false);
            this.minimized = true;
        }
    }
}
