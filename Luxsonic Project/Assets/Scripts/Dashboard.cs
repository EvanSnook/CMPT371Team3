using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

using buttons;




/// <summary>
/// A script to control the Workspace Manager menu system. 
/// This script creates and defines the funcionality for the menu buttons.
/// </summary>
public class Dashboard : MonoBehaviour, IVRButton
{
	

	/// <summary>
	/// Class defining where to instantiate buttons.
	/// Holds the name, position and rotation of each button in the inspector.
	/// </summary>
	[System.Serializable]
	public class ButtonAttributes {
		// The name and text to appear on the button
		public string buttonName;

		// The local position of the button, relative to its parent plane
		public Vector3 position;

		public string getName(){
			return this.buttonName;
		}

		public Vector3 getPosition(){
			return this.position;
		}

	}



	// Holds the attributes for each button, indexed by the ButtonType
	public ButtonAttributes[] buttonAttributes = new ButtonAttributes[10];

	// The file system to load images with
    public GameObject loadBar;  

	// Creates a display object in dashboard
    public Display display; 

	// Prefab for the menu planes, which hold menu buttons
    public GameObject planePrefab;

	// The button object to use for instantiating buttons
    public VRButton button;       

    //Images used to test load functionality
    public Texture2D[] dummyImages;

    // Reference to the buttons
    private VRButton loadButton;
    private VRButton quitButton;
    private VRButton minimizeButton;

    //Are the Buttons visable to the user?
    private bool minimized = false;

	// A list of the Copy objects instantiated in the workspace
    public List<GameObject> currentCopies;

	// List of the buttons that can be used to modify Copy objects
    private List<VRButton> copyButtons;

	// Reference to the plane holding the Load, Quit and Minimize buttons
    private GameObject menuPlane;

	// Position, rotation and scale of the menu plane in world space
    public Vector3 menuPlanePosition;
    public Vector3 menuPlaneRotation;
	public Vector3 menuPlaneScale;

	// Reference to the plane holding the buttons for modifying Copy objects
    private GameObject copyButtonsPlane;

	// Position, rotation and scale of the copy plan in world space
    public Vector3 copyPlanePosition;
    public Vector3 copyPlaneRotation;
	public Vector3 copyPlaneScale;

    // The current selection, defaults to none
    private ButtonType currentSelection = ButtonType.NONE;

	// Built-in Unity method called at the beginning of the Scene
    public void Start()
    {
        display = GameObject.FindGameObjectWithTag("Display").GetComponent<Display>();
        this.copyButtons = new List<VRButton>();
        DisplayMenu();
    }

	/// <summary>
	/// Initializes a VRButton, given attributes of a button indexed by ButtonType.
	/// Pre:: index must be between 0 and buttonAttributes.Length - 1
	/// Post:: A new button has been instantiated with the correct attributes
	/// </summary>
	/// <returns>VRButton initialized with correct attributes.</returns>
	/// <param name="index">The type of button as an index into buttonAttributes.</param>
	/// <param name="copyPlane">If set to <c>true</c>, use copyPlane position and rotation;
	/// 	otherwise, use menuPlane position and rotation.</param>
	public VRButton InitializeButton(ButtonType index, bool copyPlane=false){

		Assert.IsTrue ((int)index < buttonAttributes.Length);
		Assert.IsTrue ((int)index >= 0);

		Vector3 pos = buttonAttributes [(int)index].getPosition ();
		string newName = buttonAttributes [(int)index].getName ();
		Debug.Log (newName);
		VRButton newButton;

		if (copyPlane) {
			newButton = Instantiate(button, pos,
				Quaternion.Euler(copyPlaneRotation));
			newButton.transform.parent = this.copyButtonsPlane.transform;
		} else {
			newButton = Instantiate(button, pos,
				Quaternion.Euler(menuPlaneRotation));
			newButton.transform.parent = this.menuPlane.transform;
		}
		newButton.type = index;
		newButton.transform.localPosition = new Vector3(pos.x, pos.y, 0.0f);
		newButton.name = newName;
		newButton.buttonName = newName;
		newButton.manager = this.gameObject;
		newButton.textObject = newButton.GetComponentInChildren<TextMesh>();
		newButton.textObject.text = newName;
		return newButton;
	}


    /// <summary>
    /// Function DisplayMenu() Creates the load, quit and minimize buttons for the menu
    /// Pre:: nothing
    /// Post:: Creation of the load, quit and minimize buttons
    /// </summary>
    public void DisplayMenu()
    {

        this.menuPlane = Instantiate(planePrefab, this.menuPlanePosition, Quaternion.Euler(this.menuPlaneRotation));
        this.menuPlane.transform.parent = this.gameObject.transform;
        this.menuPlane.transform.localScale = this.menuPlaneScale;

        this.copyButtonsPlane = Instantiate(planePrefab, this.copyPlanePosition, Quaternion.Euler(this.copyPlaneRotation));
        this.copyButtonsPlane.transform.parent = this.gameObject.transform;
        this.copyButtonsPlane.transform.localScale = this.copyPlaneScale;

        // Create the load button to access the filesystem
		this.loadButton = InitializeButton(ButtonType.LOAD_BUTTON);

        // Create the Quit button 
		this.quitButton = InitializeButton(ButtonType.QUIT_BUTTON);

        // Create the Minimize button 
		this.minimizeButton = InitializeButton(ButtonType.MINIMIZE_BUTTON);

        // Create the Copy modification buttons
		copyButtons.Add(InitializeButton(ButtonType.CONTRAST_BUTTON, true));
		copyButtons.Add(InitializeButton(ButtonType.ROTATE_BUTTON, true));
		copyButtons.Add(InitializeButton(ButtonType.ZOOM_BUTTON, true));
		copyButtons.Add(InitializeButton(ButtonType.BRIGHTNESS_BUTTON, true));
		copyButtons.Add(InitializeButton(ButtonType.RESIZE_BUTTON, true));
		copyButtons.Add(InitializeButton(ButtonType.FILTER_BUTTON, true));
		copyButtons.Add(InitializeButton(ButtonType.CLOSE_BUTTON, true));

    }

    /// <summary>
    /// Function VRButtonClicked is called by a button object when it is interacted with.
    /// Pre:: string representing the name of the button selected
    /// Post:: Execution of funcion associated with string given
    /// Return:: nothing
    /// </summary>
    /// <param name="button">The name of the button</param>
    public void VRButtonClicked(ButtonType button)
    {
        switch (button)
        {
            case ButtonType.LOAD_BUTTON:
                // If the load button was clicked
                Load();
                break;
            case ButtonType.QUIT_BUTTON:
                // If the quit button was clicked
                Quit();
                break;
            case ButtonType.MINIMIZE_BUTTON:
                // If the minimize button was clicked
                Minimize();
                break;

			case ButtonType.BRIGHTNESS_BUTTON:
				this.currentSelection = button;
                this.UpdateCopyOptions();
                break;

            case ButtonType.CONTRAST_BUTTON:
                this.currentSelection = button;
                this.UpdateCopyOptions();
                break;

            case ButtonType.RESIZE_BUTTON:
                this.currentSelection = button;
                this.UpdateCopyOptions();
                break;

            case ButtonType.CLOSE_BUTTON:
                this.currentSelection = button;
                this.UpdateCopyOptions();
                break;

            default:    // A copy option was clicked

                //if (this.currentCopies.Count > 0)
                //{
                //this.currentCopy.SendMessage("ReceiveSlider", this.slider);
                //  foreach (GameObject currentCopy in this.currentCopies)
                //{
                //  currentCopy.SendMessage("VRButtonClicked", button);
                //}
                // }
                break;

        }

    }

    private void UpdateCopyOptions()
    {
        if (this.currentCopies.Count > 0)
        {
            //this.currentCopy.SendMessage("ReceiveSlider", this.slider);
            foreach (GameObject currentCopy in this.currentCopies)
            {
                currentCopy.SendMessage("NewOptions", this.currentSelection);
            }
        }
    }

    public void CopySelected(GameObject copy)
    {
        if (copy.GetComponent<Copy>().isCurrentImage)
        {
            this.currentCopies.Add(copy);
            this.UpdateCopyOptions();
        }
        else
        {
            this.currentCopies.Remove(copy);
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
        if (this.minimized)
        {
			MaximizeButtons (true);
        }
        else
        {
			MaximizeButtons (false);
        }
    }

	/// <summary>
	/// Helper function for Minimize(). Sets menu and buttons active or not active based on the
	/// mode parameter.
	/// Pre:: nothing
	/// Post:: Buttons and menus set to either Active or not Active.
	/// Return:: nothing
	/// </summary>
	/// <param name="mode">If set to <c>true</c>, enable the menu and buttons; 
	/// otherwise disable them.</param>
	public void MaximizeButtons(bool mode){
		this.loadButton.gameObject.SetActive(mode);
		this.quitButton.gameObject.SetActive(mode);
		foreach (VRButton cButton in copyButtons)
		{
			cButton.gameObject.SetActive(mode);
		}
		this.minimized = !mode;
	}

    public void SetCopyButtons(List<VRButton> theList)
    {
        this.copyButtons = theList;
    }

}
