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
    // Holds the attributes for each button to be instantiated
    [SerializeField]
    private List<ButtonAttributes> buttonList = new List<ButtonAttributes>();

    // Holds references to the instantiated buttons
    private List<GameObject> buttonObjects = new List<GameObject>();

    private List<GameObject> pendingDeletion = new List<GameObject>();

	// The file system to load images with
	public FileBrowser fileBrowser;

	// Creates a display object in dashboard
	public Display display;

	// Prefab for the menu planes, which hold menu buttons
	public GameObject planePrefab;

	// The button object to use for instantiating buttons
	public GameObject buttonPrefab;

	//Images used to test load functionality
	public Texture2D[] dummyImages;

	//Are the Buttons visable to the user?
	private bool minimized = false;

	// A list of the Copy objects instantiated in the workspace
	public List<GameObject> currentCopies;

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

	// The current selection, defaults to null
	private GameObject currentButtonSelection = null;

    // True if deselect all is selected
	private bool deselectingAll = false;


	// Built-in Unity method called at the beginning of the Scene
	public void Start()
	{
		display = GameObject.FindGameObjectWithTag("Display").GetComponent<Display>();
        this.currentCopies = new List<GameObject>();
		DisplayMenu();
	}


	public void Update()
	{
		if ((this.currentCopies.Count == 0) && (this.currentButtonSelection != null))
		{
            this.currentButtonSelection.GetComponent<VRButton>().TimedUnpress();
            this.currentButtonSelection = null;
		}
	}


    /// <summary>
    /// Creates new button, and applies passed in attributes. 
    /// </summary>
    /// <param name="attributes">Attributes to be applied to the new button</param>
    /// <param name="buttonPrefab">Prefab to instantiate as a button</param>
    /// <returns>Newly created button GameObject</returns>
    public GameObject CreateButton(ButtonAttributes attributes, GameObject buttonPrefab)
    {
        Assert.IsNotNull(attributes, "The attributes passed to the button are null.");
        Assert.IsNotNull(buttonPrefab, "The button prefab is null.");

        GameObject newButton;

        newButton = Instantiate(buttonPrefab, attributes.position,
        Quaternion.Euler(attributes.rotation));

        newButton.GetComponent<VRButton>().Initialise(attributes, this.gameObject);
        newButton.name = attributes.buttonName;

        Assert.IsNotNull(newButton, "The new button was not created successfully.");

        return newButton;
    }


	/// <summary>
	/// Function DisplayMenu() Creates the load, quit and minimize buttons for the menu
	/// </summary>
    /// <pre>nothing</pre>
    /// <post>Creation of the load, quit, and minimize buttons for the menu</post>
	public void DisplayMenu()
	{

        Assert.IsNotNull(planePrefab, "The plane prefab is null.");

		this.menuPlane = Instantiate(planePrefab, this.menuPlanePosition, Quaternion.Euler(this.menuPlaneRotation));
		this.menuPlane.transform.parent = this.gameObject.transform;
		this.menuPlane.transform.localScale = this.menuPlaneScale;

		this.copyButtonsPlane = Instantiate(planePrefab, this.copyPlanePosition, Quaternion.Euler(this.copyPlaneRotation));
		this.copyButtonsPlane.transform.parent = this.gameObject.transform;
		this.copyButtonsPlane.transform.localScale = this.copyPlaneScale;

        // loops through each button to be instantiated
        foreach (ButtonAttributes attributes in buttonList)
        {
            GameObject newButton;
            // if button is tagged as a menu button, tie it to the menu plane
            if (attributes.type == ButtonType.MENU_BUTTON)
            {
                attributes.rotation = menuPlaneRotation;
                newButton = CreateButton(attributes, buttonPrefab);
                newButton.transform.parent = this.menuPlane.transform;
                newButton.GetComponent<VRButton>().ResetPosition();
            }
            else // else tie it to the copy plane
            {
                attributes.rotation = copyPlaneRotation;
                newButton = CreateButton(attributes, buttonPrefab);
                newButton.transform.parent = this.copyButtonsPlane.transform;
                newButton.GetComponent<VRButton>().ResetPosition();
            }
            // add to the list of all buttons
            buttonObjects.Add(newButton);
        }

        Assert.IsNotNull(this.buttonObjects, "The button objects are null.");
        Assert.IsTrue(this.buttonObjects.Count > 0, "The button objects list is empty.");
	}
	

    /// <summary>
    /// Tell each copy that is selected that a new modifier has been selected
    /// </summary>
    /// <param name="arguments">A string array containing the new option and the button name</param>
    private void UpdateCopyOptions(string[] arguments)
	{
        Assert.IsTrue(arguments.Length >= 2, "There are not enough arguments to update the copy options.");

        string newOption = arguments[0];
        string buttonName = arguments[1];

        Debug.Log("User pressed " + buttonName + " button.");

        UpdateCurrentSelection(buttonName);
        if (this.currentCopies.Count > 0 && this.currentButtonSelection != null)
        {
            foreach (GameObject currentCopy in this.currentCopies)
            {
                currentCopy.SendMessage("ChangeSelection", newOption);
            }
        }   // If there are no copies selected, deselect the current button
        else if (this.currentCopies.Count == 0)
        {
            this.currentButtonSelection.GetComponent<VRButton>().TimedUnpress();
            this.currentButtonSelection = null;
        }
        CleanUpCopies();
	}


    /// <summary>
    /// Remove all copies in the pendingDeletion list
    /// </summary>
    /// <post>All copies in the pending deletion list have been deleted</post>
    private void CleanUpCopies()
    {
        foreach (GameObject copy in this.pendingDeletion)
        {
            this.currentCopies.Remove(copy);
        }
        pendingDeletion.Clear();
        Assert.IsTrue(this.pendingDeletion.Count == 0, "The pending deletion list was not properly cleared.");
    }



    /// <summary>
    /// Delete the given copy
    /// </summary>
    /// <post>The given object has been deleted</post>
    private void DeleteCopy(GameObject target)
    {
        Assert.IsNotNull(target, "The object to delete was null.");
        this.display.RemoveCopy(target);
        this.pendingDeletion.Add(target);
        Assert.IsTrue(this.pendingDeletion.Contains(target), "The object to delete was not added to the pending deletion list.");
    }


    /// <summary>
    /// Unpress the current selected button and set the current selected button to the new button.
    /// </summary>
    /// <param name="newButton">The new button to set as the current</param>
    /// <post>The current selection is set to the option corresponding to the given button</post>
    private void UpdateCurrentSelection(string newButton)
    {   
        // make sure not null
        if (this.currentButtonSelection != null)
        {
            // unpress current button
            this.currentButtonSelection.GetComponent<VRButton>().UnpressButton();
        }
        
        // update current button to new button
        this.currentButtonSelection = this.buttonObjects.Find(button => button.name == newButton);
    }


    /// <summary>
    /// Detemines whether to add or remove the given copy in the list 
    /// of current copies.
    /// </summary>
    /// <param name="copy"></param>
	public void CopySelected(GameObject copy)
	{
        Assert.IsNotNull(copy, "The copy selected was null.");
        
        if (copy.GetComponent<Copy>().isCurrentImage)
        {
            this.currentCopies.Add(copy);
            if (currentButtonSelection != null)
            {
                this.UpdateCopyOptions(currentButtonSelection.GetComponent<VRButton>().attributes.buttonParameters);
            }
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
	/// </summary>
    /// <post>Program Terminiation</post>
	private void Quit()
	{
		Debug.Log("Quit Button clicked");
		Application.Quit();
	}


	/// <summary>
	/// Function Load() adds functionality for when the load button is clicked.  Currently
	/// it will select a random image stored in the Assets and add it to the Display
	/// </summary>
    /// <post>New Texture2D given and added to the Display class</post>
	private void Load()
	{
        Debug.Log("Load Button clicked");
		fileBrowser.gameObject.SetActive(true);
		Minimize();
	}


	/// <summary>
	/// Function Minimize() adds functionality for the minimize button.  It will minimize disable
	/// the other buttons if they are currently visable or make them enabled if they are currently 
	/// disabled
	/// </summary>
    /// <post>Buttons set to either Active or Not Active, minimized attribut changed</post>
	private void Minimize()
	{
        if (this.minimized)
        {
            Debug.Log("User has maximized the dashboard.");
            ToggleButtons(true);
            this.buttonObjects.Find(button => button.name == "Minimize").transform.GetChild(0).GetComponent<TextMesh>().text = "Minimize";
        }
        else
        {
            Debug.Log("User has minimized the dashboard.");
            ToggleButtons(false);
            this.buttonObjects.Find(button => button.name == "Minimize").transform.GetChild(0).GetComponent<TextMesh>().text = "Maximize";
        }
	}


    /// <summary>
    /// Helper function for Minimize(). Sets menu and buttons active or not active based on the
    /// mode parameter.
    /// </summary>
    /// <param name="mode">If set to <c>true</c>, enable the menu and buttons; 
    /// otherwise disable them.</param>
    /// <post>Buttons and menus set to either Active or not Active</post>
    public void ToggleButtons(bool mode)
    {
        foreach (GameObject button in this.buttonObjects)
        {
            if (button.name != "Minimize")
            {
                button.SetActive(mode);
            }
        }
        this.minimized = !mode;
    }


	/// <summary>
	/// Add all copies in the scene to the current copies list
	/// </summary>
    /// <post>All copies in the scene are in the current copies list</post>
	public void SelectAllCopies()
	{
        List<GameObject> tempList = this.display.GetCopies();
        Debug.Log("User has selected all copies.");
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
    /// <post>All copies in the scene have been removed from the current copies list</post>
	public void DeselectAllCopies()
	{
        Debug.Log("User has deselected all copies.");
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


    //========================================================
    // TEST HOOKS
    //=====================================================

	/// <summary>
    /// Return the value of the minimized variable.
    /// </summary>
    /// <returns>Boolean representing whether the dashboard is minimized</returns>
	public bool GetMinimized()
	{
		return this.minimized;
	}


	/// <summary>
    /// Returns the current button selection of the Dashboard
    /// </summary>
    /// <returns>The current selected button in the Dashboard</returns>
	public GameObject GetCurrentButtonSelection()
	{
		return this.currentButtonSelection;
	}

    /// <summary>
    /// Sets the button list to the list provided
    /// </summary>
    /// <param name="attributes">The list of attributes to create</param>
    public void SetButtonList(List<ButtonAttributes> attributes) {
        this.buttonList = attributes;
    }

    /// <summary>
    /// Returns the button list
    /// </summary>
    /// <returns>The button list</returns>
    public List<ButtonAttributes> GetButtonList()
    {
        return this.buttonList;
    }

    /// <summary>
    /// Test the update copy options function with the given arguments
    /// </summary>
    /// <param name="args">The arguments to pass to UpdateCopyOptions()</param>
    public void TestUpdateCopyOptions(string[] args)
    {
        this.UpdateCopyOptions(args);
    }

    /// <summary>
    /// Test the update current selcetion button with the new button
    /// </summary>
    /// <param name="button">The button to test with</param>
    /// <returns>True if the current selection is set to button, false otherwise</returns>
    public bool TestUpdateCurrentSelection(string button)
    {

        this.UpdateCurrentSelection(button);
        return this.currentButtonSelection.gameObject.name == button;
    }
}

