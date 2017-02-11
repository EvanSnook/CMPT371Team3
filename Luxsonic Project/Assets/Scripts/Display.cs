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

    [SerializeField]
    private float buttonWidth = 100;
    [SerializeField]
    private float buttonHeight = 50;
    
    public Transform myTransform;     // The transform of the display object in world space
    public SpriteRenderer imageRenderer;   // The image to render on the display object 
    public bool isCurrentImage;     // Determines if this instance of a display object is currently selected
    public float imageBrightness;   // The brigtness of the display
    public float imageContrast;     // The contrast of the display
    public Vector3 imageRotation;   // The current rotation of the display
    public float currentSize;       // The current size of the display

    /// <summary>
    /// Constructor for the display class.
    /// Creates a new display object with the Texture2D converted to a sprite stored
    /// in the imageRenderer component.
    /// </summary>
    /// <param name="image"> A Texture2D to use as the image to display </param>
    public Display(Texture2D image)
    {
        this.imageRenderer.sprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f));
        this.myTransform = this.GetComponent<Transform>();
    } 

    // TODO: Add touch controller interactibitliy

     
    /// <summary>
    /// Used to show the image option buttons
    /// </summary>
    public void OnGUI()
    {
        if (isCurrentImage)// TODO: check to makesure this works
        {
            // Convert the world point of the display to a screen point
            Vector3 displayScreenPoint = Camera.main.WorldToScreenPoint(myTransform.position);

            // The positions of the buttons, relative to the calculated screen point of the display.
            Vector3 contrastButtonPosition = displayScreenPoint + new Vector3(0, 0, 0);
            Vector3 rotateButtonPosition = displayScreenPoint + new Vector3(100, 0, 0);
            Vector3 zoomButtonPosition = displayScreenPoint + new Vector3(200, 0, 0);
            Vector3 brightnessButtonPosition = displayScreenPoint + new Vector3(200, 50, 0);
            Vector3 resizeButtonPosition = displayScreenPoint + new Vector3(0, 50, 0);
            Vector3 filterButtonPosition = displayScreenPoint + new Vector3(100, 50, 0);

            // The contrast button
            if (GUI.Button(new Rect(contrastButtonPosition.x, Screen.height - contrastButtonPosition.y, buttonWidth, buttonHeight), "Contrast"))
            {
                // Instantiate(ContrastBar)
            }

            // The rotate button
            if (GUI.Button(new Rect(rotateButtonPosition.x, Screen.height - rotateButtonPosition.y, buttonWidth, buttonHeight), "Rotate"))
            {
                // Instantiate(RotateBar)
            }

            // The zoom button
            if (GUI.Button(new Rect(zoomButtonPosition.x, Screen.height - zoomButtonPosition.y, buttonWidth, buttonHeight), "Zoom"))
            {
                // Instantiate(ZoomBar)
            }

            // The brightness button
            if (GUI.Button(new Rect(brightnessButtonPosition.x, Screen.height - brightnessButtonPosition.y, buttonWidth, buttonHeight), "Brightness"))
            {
                // Instantiate(BrightnessBar)
            }

            // The resize button
            if (GUI.Button(new Rect(resizeButtonPosition.x, Screen.height - resizeButtonPosition.y, buttonWidth, buttonHeight), "Resize"))
            {
                // Instantiate(ResizeBar)
            }

            // The filter button
            if (GUI.Button(new Rect(filterButtonPosition.x, Screen.height - filterButtonPosition.y, buttonWidth, buttonHeight), "Filter"))
            {
                // Instantiate(FilterBar)
            }
        }
    }
}
