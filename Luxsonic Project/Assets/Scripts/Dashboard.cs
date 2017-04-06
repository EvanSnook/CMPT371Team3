﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

using buttons;


/// <summary>
/// A script to control the Workspace Manager menu system. 
/// This script creates and defines the funcionality for the menu buttons.
/// </summary>
[ExecuteInEditMode]
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
        GameObject newButton;

        newButton = Instantiate(buttonPrefab, attributes.position,
        Quaternion.Euler(attributes.rotation));

        newButton.GetComponent<VRButton>().Initialise(attributes, this.gameObject);
        newButton.name = attributes.buttonName;

        return newButton;
    }


	/// <summary>
	/// Function DisplayMenu() Creates the load, quit and minimize buttons for the menu
	/// Pre:: nothing
	/// Post:: Creation of the load, quit and minimize buttons
	/// </summary>
    /// <pre>nothing</pre>
    /// <post>Creatopm of the load, quit, and minimize buttons for the menu</post>
	public void DisplayMenu()
	{

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
	}
	

    /// <summary>
    /// Tell each copy that is selected that a new modifier has been selected
    /// </summary>
    /// <param name="arguments">A string array containing the new option and the button name</param>
    private void UpdateCopyOptions(string[] arguments)
	{
        string newOption = arguments[0];
        string buttonName = arguments[1];

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
    private void CleanUpCopies()
    {
        foreach (GameObject copy in this.pendingDeletion)
        {
            this.currentCopies.Remove(copy);
        }
        pendingDeletion.Clear();
    }



    /// <summary>
    /// Delete the given copy
    /// </summary>
    private void DeleteCopy(GameObject target)
    {
        this.display.RemoveCopy(target);
        this.pendingDeletion.Add(target);
    }


    /// <summary>
    /// Unpress the current selected button and set the current selected button to the new button.
    /// </summary>
    /// <param name="newButton">The new button to set as the current</param>
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
    /// 
    /// </summary>
    /// <param name="copy"></param>
	public void CopySelected(GameObject copy)
	{
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
            ToggleButtons(true);
            this.buttonObjects.Find(button => button.name == "Minimize").transform.GetChild(0).GetComponent<TextMesh>().text = "Minimize";
        }
        else
        {
            ToggleButtons(false);
            this.buttonObjects.Find(button => button.name == "Minimize").transform.GetChild(0).GetComponent<TextMesh>().text = "Maximize";
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


    //========================================================
    // TEST HOOKS
    //=====================================================

	//for testing purposes
	public bool GetMinimized()
	{
		return this.minimized;
	}


	//for testing purposes
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

