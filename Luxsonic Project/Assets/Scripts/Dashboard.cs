using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Display display;

    public Transform myTransform;   
    public VRButton button;       // The button object to use as a button

    // Define where to instantiate the buttons
    public Vector3 loadButtonPosition;
    public Vector3 loadButtonRotation;

    public Vector3 quitButtonPosition;
    public Vector3 quitButtonRotation;

    public Vector3 minimizeButtonPosition;
    public Vector3 minimizeButtonRotation;


    public Texture2D[] dummyImages;

    // Reference to the buttons
    private VRButton loadButton;
    private VRButton quitButton;
    private VRButton minimizeButton;

    //Are the Buttons visable to the user?
    private bool minimized = false;

    public void Start()
    {
        this.myTransform = this.GetComponent<Transform>();
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

        this.loadButton.name = "Load";
        this.loadButton.manager = this.gameObject;


        // Create the Quit button 
        this.quitButton = Instantiate(button, quitButtonPosition,
            Quaternion.Euler(quitButtonRotation));

        this.quitButton.name = "Quit";
        this.quitButton.manager = this.gameObject;

        // Create the Minimize button 
        this.minimizeButton = Instantiate(button, minimizeButtonPosition,
            Quaternion.Euler(minimizeButtonRotation));

        this.minimizeButton.name = "Minimize";
        this.minimizeButton.manager = this.gameObject;
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
        Debug.Log("Load button pressed!");
        Display imageMan = GameObject.FindGameObjectWithTag("Display").GetComponent<Display>();
        imageMan.AddImage(dummyImages[Random.Range(0, dummyImages.Length)]);
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
            this.display.gameObject.SetActive(true);

            this.minimized = false;
            

        } else {

            this.loadButton.gameObject.SetActive(false);
            this.quitButton.gameObject.SetActive(false);
            this.display.gameObject.SetActive(false);

            this.minimized = true;
        }
    }

    //public void OnGUI()
    //{
    //    // Convert the world point of the display to a screen point
    //    Vector3 displayScreenPoint = Camera.main.WorldToScreenPoint(myTransform.position);

    //    // The positions of the buttons, relative to the calculated screen point of the display.
    //    Vector3 loadButtonPosition = displayScreenPoint - new Vector3(0, 0, 0);

    //    // The loadbutton
    //    if (GUI.Button(new Rect(loadButtonPosition.x, Screen.height - loadButtonPosition.y, buttonWidth, buttonHeight), "Load Image"))
    //    {
    //    }
    //}
}
