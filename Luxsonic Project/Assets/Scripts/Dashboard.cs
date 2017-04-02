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
	public class ButtonAttributes
	{
		// The name and text to appear on the button
		public string buttonName;

		// The local position of the button, relative to its parent plane
		public Vector3 position;

		public ButtonAttributes(string n, Vector3 pos)
		{
			buttonName = n;
			position = pos;
		}


		public string getName()
		{
			return this.buttonName;
		}

		public Vector3 getPosition()
		{
			return this.position;
		}

	}



	// Holds the attributes for each button, indexed by the ButtonType
	public ButtonAttributes[] buttonAttributes = new ButtonAttributes[13];

	// The file system to load images with
	public FileBrowser1 fileBrowser;

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

	private bool deselectingAll = false;
	private Vector3 localScaleSetting;
	private Vector3 menuLocalScaleSetting;
	private VRButton pressedButton;


	// Built-in Unity method called at the beginning of the Scene
	public void Start()
	{
		display = GameObject.FindGameObjectWithTag("Display").GetComponent<Display>();
		this.copyButtons = new List<VRButton>();
		DisplayMenu();
		localScaleSetting = copyButtons [0].transform.localScale;
		this.menuLocalScaleSetting = this.loadButton.transform.localScale;
		this.pressedButton = null;

	}

	public void Update()
	{
		if (this.currentCopies.Count == 0)
		{
			this.currentSelection = ButtonType.NONE;
		}
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
	public VRButton InitializeButton(ButtonType index, bool copyPlane = false)
	{

		Assert.IsTrue((int)index < buttonAttributes.Length);
		Assert.IsTrue((int)index >= 0);

		Vector3 pos = buttonAttributes[(int)index].getPosition();
		string newName = buttonAttributes[(int)index].getName();
		VRButton newButton;

		if (copyPlane)
		{
			newButton = Instantiate(button, pos,
				Quaternion.Euler(copyPlaneRotation));
			newButton.transform.parent = this.copyButtonsPlane.transform;
		}
		else
		{
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
		copyButtons.Add(InitializeButton(ButtonType.INVERT_BUTTON, true));
		copyButtons.Add(InitializeButton(ButtonType.ZOOM_BUTTON, true));
		copyButtons.Add(InitializeButton(ButtonType.BRIGHTNESS_BUTTON, true));
		copyButtons.Add(InitializeButton(ButtonType.RESIZE_BUTTON, true));
		copyButtons.Add(InitializeButton(ButtonType.SATURATION_BUTTON, true));
		copyButtons.Add(InitializeButton(ButtonType.CLOSE_BUTTON, true));
		copyButtons.Add(InitializeButton(ButtonType.RESTORE_COPY_BUTTON, true));
		copyButtons.Add(InitializeButton(ButtonType.SELECT_ALL_COPIES_BUTTON, true));
		copyButtons.Add(InitializeButton(ButtonType.DESELECT_ALL_COPIES_BUTTON, true));


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
		if (this.pressedButton != null)
		{
			this.pressedButton.transform.localScale = this.localScaleSetting;
			this.pressedButton.transform.gameObject.SendMessage ("UnpressButton");
		}
		switch (button)
		{
		case ButtonType.LOAD_BUTTON:
			this.loadButton.transform.localScale = new Vector3 (this.menuLocalScaleSetting.x, this.menuLocalScaleSetting.y, 50f);
			this.pressedButton = this.loadButton;
			Load ();
			Invoke ("UnpressMenuButton", 1.0f);
			break;
		case ButtonType.QUIT_BUTTON:
			// If the quit button was clicked
			this.quitButton.transform.localScale = new Vector3 (this.menuLocalScaleSetting.x, this.menuLocalScaleSetting.y, 50f);
			this.pressedButton = this.quitButton;
			Quit();
			Invoke ("UnpressMenuButton", 1.0f);
			break;
		case ButtonType.MINIMIZE_BUTTON:
			// If the minimize button was clicked
			this.quitButton.transform.localScale = new Vector3 (this.menuLocalScaleSetting.x, this.menuLocalScaleSetting.y, 50f);
			this.pressedButton = this.minimizeButton;
			Minimize();
			Invoke ("UnpressMenuButton", 1.0f);
			break;

		case ButtonType.BRIGHTNESS_BUTTON:
			this.setButtonScale (button);
			this.currentSelection = button;
			if (this.currentCopies.Count != 0) {
				this.UpdateCopyOptions ();
			}
			else {
				Invoke ("UnpressButton", 1.0f);
			}

			break;

		case ButtonType.CONTRAST_BUTTON:
			this.setButtonScale (button);
			this.currentSelection = button;
			if (this.currentCopies.Count != 0) {
				this.UpdateCopyOptions ();
			}
			else {
				Invoke ("UnpressButton", 1.0f);
			}
			break;

		case ButtonType.INVERT_BUTTON:
			this.setButtonScale (button);
			this.currentSelection = button;
			if (this.currentCopies.Count != 0) {
				this.UpdateCopyOptions ();
			}
			Invoke ("UnpressButton", 1.0f);
			break;

		case ButtonType.SATURATION_BUTTON:
			this.setButtonScale(button);
			this.currentSelection = button;
			if (this.currentCopies.Count != 0) {
				this.UpdateCopyOptions ();
			}
			else {
				Invoke ("UnpressButton", 1.0f);
			}
			break;

		case ButtonType.RESIZE_BUTTON:
			this.currentSelection = button;
			if (this.currentCopies.Count != 0) {
				this.UpdateCopyOptions ();
			}
			else {
				Invoke ("UnpressButton", 1.0f);
			}
			break;

		case ButtonType.CLOSE_BUTTON:
			this.currentSelection = button;
			if (this.currentCopies.Count != 0) {
				this.UpdateCopyOptions ();
			}
			Invoke ("UnpressButton", 1.0f);
			break;

		case ButtonType.RESTORE_COPY_BUTTON:
			this.currentSelection = button;
			if (this.currentCopies.Count != 0) {
				this.UpdateCopyOptions ();
			}
			Invoke ("UnpressButton", 1.0f);
			break;

		case ButtonType.SELECT_ALL_COPIES_BUTTON:
			this.SelectAllCopies();
			if (this.currentCopies.Count != 0) {
				this.UpdateCopyOptions ();
			}
			Invoke ("UnpressButton", 1.0f);
			break;

		case ButtonType.DESELECT_ALL_COPIES_BUTTON:
			this.DeselectAllCopies();
			Invoke ("UnpressButton", 1.0f);
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
		List<GameObject> copiesToDelete = new List<GameObject>();

		if (this.currentCopies.Count > 0)
		{
			//this.currentCopy.SendMessage("ReceiveSlider", this.slider);
			foreach (GameObject currentCopy in this.currentCopies)
			{
				if (this.currentSelection == ButtonType.CLOSE_BUTTON)
				{
					copiesToDelete.Add(currentCopy);
				}
				else
				{
					currentCopy.SendMessage("NewOptions", this.currentSelection);

				}
			}
		}   // If there are no copies selected, deselect the current button
		else if (this.currentCopies.Count == 0)
		{
			this.currentSelection = ButtonType.NONE;
		}

		//if current selection was the close, set it to none after they have been closed.
		if (this.currentSelection == ButtonType.CLOSE_BUTTON)
		{
			foreach (GameObject copy in copiesToDelete)
			{
				this.currentCopies.Remove(copy);
                this.display.gameObject.SendMessage("RemoveCopy", copy);
				copy.SendMessage("NewOptions", this.currentSelection);
			}
			this.currentSelection = ButtonType.NONE;
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
			if (!this.deselectingAll)
			{
				this.currentCopies.Remove(copy);
			}
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
		fileBrowser.gameObject.SetActive(true);
		Minimize();
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
			MaximizeButtons(true);
			this.minimizeButton.textObject.text = "Minimize";
		}
		else
		{
			MaximizeButtons(false);
			this.minimizeButton.textObject.text = "Maximize";
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
	public void MaximizeButtons(bool mode)
	{
		this.loadButton.gameObject.SetActive(mode);
		this.quitButton.gameObject.SetActive(mode);
		this.copyButtonsPlane.gameObject.SetActive(mode);
		this.display.gameObject.SetActive(mode);
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

	/// <summary>
	/// Add all copies in the scene to the current copies list
	/// </summary>
	public void SelectAllCopies()
	{
		List<GameObject> tempList = this.display.GetCopies();

		foreach (GameObject copy in tempList)
		{
			if (!copy.GetComponent<Copy>().isCurrentImage)
			{
				copy.gameObject.SendMessage("Selected");
			}
		}
	}

	/// <summary>
	/// Remove all copies in the scene from the current copies list
	/// </summary>
	public void DeselectAllCopies()
	{
		this.deselectingAll = true;
		List<GameObject> tempList = this.currentCopies;
		foreach (GameObject copy in tempList)
		{
			if (copy.gameObject.GetComponent<Copy>().isCurrentImage)
			{
				copy.gameObject.SendMessage("Selected");
			}
		}
		this.currentCopies.Clear();
		this.deselectingAll = false;
	}

	//for testing purposes
	public bool GetMinimized()
	{
		return this.minimized;
	}

	//for testing purposes
	public ButtonType GetCurrentSelection()
	{
		return this.currentSelection;
	}

	//find the button of button mybutton and make it appear pressed by setting z value to 0
	//set the last button that was selected appear unselected by returning it to its orginal position
	private void setButtonScale (ButtonType myButton){
		foreach (VRButton cButton in copyButtons)
		{
			if (cButton.type == myButton) {
				cButton.transform.localScale = new Vector3 (this.localScaleSetting.x, this.localScaleSetting.y, 50f);
				this.pressedButton = cButton;
			}
		}
	}
	public void UnpressButton(){
		Debug.Log (this.pressedButton);
		if (this.pressedButton != null) {
			this.pressedButton.transform.localScale = this.localScaleSetting;
			this.pressedButton.gameObject.SendMessage ("UnpressButton");
		}

		this.pressedButton = null;
		this.currentSelection = ButtonType.NONE;

		UpdateCopyOptions ();
	}
	public void UnpressMenuButton(){
		if (this.pressedButton != null) {
			this.pressedButton.transform.localScale = this.menuLocalScaleSetting;
			this.pressedButton.gameObject.SendMessage ("UnpressButton");
		}

		this.pressedButton = null;
		this.currentSelection = ButtonType.NONE;
	}

}

