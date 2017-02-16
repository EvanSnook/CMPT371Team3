using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//This is a simple script for ID2 purposes only.  The user will be able choose between
//selecting from a windowsscene with mouse click abilities and a VR scene where they can 
//Interact with and grab an x-ray in VR.

public class DefaultScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnGUI()
    {
        int buttonWidth = 100;
        int buttonLength = 50;
        int buttonPosY1 = 20;
        int buttonPosY2 = 70;
        GUI.Box(new Rect(0, 0, 200, 200), "Select Scene");

        if (GUI.Button(new Rect(12.5f, buttonPosY1, buttonWidth, buttonLength), "VR Scene"))
        {
           // SceneManager.LoadScene(7);
        }
        if(GUI.Button(new Rect(12.5f, buttonPosY2, buttonWidth, buttonLength), "Windows Scene"))
        {
            SceneManager.LoadScene(1);
        }
    }
}
