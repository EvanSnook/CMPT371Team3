using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


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

    //[SerializeField]
    //private float buttonWidth = 100; //commented out due to not being used
    //[SerializeField]
    //private float buttonHeight = 50;

	public enum ButtonType {LOAD_BUTTON, QUIT_BUTTON, MINIMIZE_BUTTON, CONTRAST_BUTTON,
		ROTATE_BUTTON, ZOOM_BUTTON, BRIGHTNESS_BUTTON,
		RESIZE_BUTTON, FILTER_BUTTON, CLOSE_BUTTON};





	public ButtonAttributes[] buttonAttributes = new ButtonAttributes[10];

    public GameObject loadBar;  // The file system to load images with
    public Display display; // Creates a display object in dashboard
    public GameObject planePrefab;


    public Transform myTransform;
    public VRButton button;       // The button object to use as a button



    //Images used to test load functionality
    public Texture2D[] dummyImages;

    // Reference to the buttons
    private VRButton loadButton;
    private VRButton quitButton;
    private VRButton minimizeButton;

    //Are the Buttons visable to the user?
    private bool minimized = false;

    public List<GameObject> currentCopies;

    private List<VRButton> copyButtons;

    private GameObject menuPlane;
    public Vector3 menuPlanePosition;
    public Vector3 menuPlaneRotation;

    private GameObject copyButtonsPlane;
    public Vector3 copyPlanePosition;
    public Vector3 copyPlaneRotation;

    public Vector3 copyButtonsPanelScale;
    public Vector3 menuButtonsPanelScale;

    // The current selection, defaults to none
    private string currentSelection = "none";

    public void Start()
    {
        this.myTransform = this.GetComponent<Transform>();
        display = GameObject.FindGameObjectWithTag("Display").GetComponent<Display>();
        this.copyButtons = new List<VRButton>();
        DisplayMenu();
    }


	public VRButton InitializeButton(int index, bool copyPlane=false){
		Vector3 pos = buttonAttributes [index].getPosition ();
		string newName = buttonAttributes [index].getName ();
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
		newButton.transform.localPosition = new Vector3(pos.x, pos.y, 0.0f);

		newButton.name = newName;
		newButton.manager = this.gameObject;
		newButton.textObject = button.GetComponentInChildren<TextMesh>();
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
        this.menuPlane.transform.localScale = this.menuButtonsPanelScale;

        this.copyButtonsPlane = Instantiate(planePrefab, this.copyPlanePosition, Quaternion.Euler(this.copyPlaneRotation));
        this.copyButtonsPlane.transform.parent = this.gameObject.transform;
        this.copyButtonsPlane.transform.localScale = this.copyButtonsPanelScale;

        // Create the load button to access the filesystem
		this.loadButton = InitializeButton((int)ButtonType.LOAD_BUTTON);

        // Create the Quit button 
		this.quitButton = InitializeButton((int)ButtonType.QUIT_BUTTON);

        // Create the Minimize button 
		this.minimizeButton = InitializeButton((int)ButtonType.MINIMIZE_BUTTON);

        // Create the buttons
		copyButtons.Add(InitializeButton((int)ButtonType.CONTRAST_BUTTON, true));
		copyButtons.Add(InitializeButton((int)ButtonType.ROTATE_BUTTON, true));
		copyButtons.Add(InitializeButton((int)ButtonType.ZOOM_BUTTON, true));
		copyButtons.Add(InitializeButton((int)ButtonType.BRIGHTNESS_BUTTON, true));
		copyButtons.Add(InitializeButton((int)ButtonType.RESIZE_BUTTON, true));
		copyButtons.Add(InitializeButton((int)ButtonType.FILTER_BUTTON, true));
		copyButtons.Add(InitializeButton((int)ButtonType.CLOSE_BUTTON, true));

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

            case "Brightness":
                this.currentSelection = "Brightness";
                this.UpdateCopyOptions();
                break;

            case "Contrast":
                this.currentSelection = "Contrast";
                this.UpdateCopyOptions();
                break;

            case "Resize":
                this.currentSelection = "Resize";
                this.UpdateCopyOptions();
                break;

            case "Close":
                this.currentSelection = "Close";
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
