using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkspaceManager : MonoBehaviour {

    [SerializeField]
    private float buttonWidth = 100;
    [SerializeField]
    private float buttonHeight = 50;

    public GameObject loadBar;

    public Transform myTransform;

    public void Start()
    {
        this.myTransform = this.GetComponent<Transform>();
    }

    public void OnGUI()
    {
        // Convert the world point of the display to a screen point
        Vector3 displayScreenPoint = Camera.main.WorldToScreenPoint(myTransform.position);

        // The positions of the buttons, relative to the calculated screen point of the display.
        Vector3 loadButtonPosition = displayScreenPoint - new Vector3(0, 0, 0);

        // The loadbutton
        if (GUI.Button(new Rect(loadButtonPosition.x, Screen.height - loadButtonPosition.y, buttonWidth, buttonHeight), "Load Image"))
        {
            Instantiate(loadBar, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
        }
    }
}
