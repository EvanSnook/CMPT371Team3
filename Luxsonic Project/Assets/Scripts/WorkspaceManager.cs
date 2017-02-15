using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script to control the Workspace Manager menu system. 
/// This script creates and defines the funcionality for the menu buttons.
/// </summary>
public class WorkspaceManager : MonoBehaviour {

    [SerializeField]
    private float buttonWidth = 100;
    [SerializeField]
    private float buttonHeight = 50;

    public GameObject loadBar;  // The file system to load images with

    public Transform myTransform;   
    public Button button;       // The button object to use as a button

    // Define where to instantiate the Load button
    public float loadButtonPositionX;
    public float loadButtonPositionY;
    public float loadButtonPositionZ;
    public float loadButtonRotationX;
    public float loadButtonRotationY;
    public float loadButtonRotationZ;

    public Texture2D dummyImage;

    public void Start()
    {
        this.myTransform = this.GetComponent<Transform>();
        DisplayMenu();
    }

    

    /// <summary>
    /// Creates the buttons for the menu
    /// </summary>
    public void DisplayMenu(){

        // Create the load button to access the filesystem
        Button loadButton = Instantiate(button, new Vector3(loadButtonPositionX,loadButtonPositionY,loadButtonPositionZ), 
            Quaternion.Euler(new Vector3(loadButtonRotationX, loadButtonRotationY, loadButtonRotationZ)));

        loadButton.name = "Load";
        loadButton.manager = this.gameObject;
    }

    /// <summary>
    /// Called by a button object when it is interacted with.
    /// </summary>
    /// <param name="name">The name of the button</param>
    public void ButtonClicked(string name)
    {
        // If the load button was clicked...
        if(name == "Load")
        {
            Debug.Log("Load button pressed!");
            ImageManager imageMan = GameObject.FindGameObjectWithTag("ImageManager").GetComponent<ImageManager>();
            imageMan.AddImage(dummyImage);
            //Instantiate(loadBar, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
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
