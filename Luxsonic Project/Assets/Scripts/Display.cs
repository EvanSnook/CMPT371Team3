using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script to be attached to objects that are to be displayed to the user.
/// This object will have an image attached to it that the user may choose from the loaded images, 
/// and will contain options to control properties of the image through related scripts, as well as being able to
/// control the position and size of the display.
/// </summary>
public class Display : MonoBehaviour {

    private const float BUTTON_WIDTH = 150;
    private const float BUTTON_HEIGHT = 100;
    
    private Transform position;     // The position of the display object in world space
    private SpriteRenderer imageRenderer;   // The image to render on the display object 

    public bool isCurrentImage;     // Determines if this instance of a display object is currently selected
    public float imageBrightness;   // The brigtness of the display
    public float imageContrast;     // The contrast of the display
    public Vector3 imageRotation;   // The current rotation of the display
    public float currentSize;       // The current size of the display


    /// <summary>
    /// Constructor for the display class
    /// </summary>
    /// <param name="image"> A Texture2D to use as the image to display </param>
    public Display(Texture2D image)
    {
        this.imageRenderer.sprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f));
    } 

    // TODO: Add touch controller interactibitliy

     
    /// <summary>
    /// Used to show the image option buttons
    /// </summary>
    public void OnGUI()
    {
        // The contrast button
        if(GUI.Button(new Rect(10, 10, BUTTON_WIDTH, BUTTON_HEIGHT), "Contrast"))
        {
            // Instantiate(ContrastBar)
        }

        // The rotate button
        if (GUI.Button(new Rect(10, 10, BUTTON_WIDTH, BUTTON_HEIGHT), "Rotate"))
        {
            // Instantiate(RotateBar)
        }

        // The zoom button
        if (GUI.Button(new Rect(10, 10, BUTTON_WIDTH, BUTTON_HEIGHT), "Zoom"))
        {
            // Instantiate(ZoomBar)
        }

        // The brightness button
        if (GUI.Button(new Rect(10, 10, BUTTON_WIDTH, BUTTON_HEIGHT), "Brightness"))
        {
            // Instantiate(BrightnessBar)
        }

        // The resize button
        if (GUI.Button(new Rect(10, 10, BUTTON_WIDTH, BUTTON_HEIGHT), "Resize"))
        {
            // Instantiate(ResizeBar)
        }

        // The filter button
        if (GUI.Button(new Rect(10, 10, BUTTON_WIDTH, BUTTON_HEIGHT), "Filter"))
        {
            // Instantiate(FilterBar)
        }
    }
}
