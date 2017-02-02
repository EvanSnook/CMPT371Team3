using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.IO;

public class MainMenu : MonoBehaviour
{

    bool displayGUI;

    // Use this for initialization
    void Start()
    {
        displayGUI = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        //Dimentions of the two buttons start and quit
        int buttonWidth = 70;
        int buttonLength = 20;
        int buttonPosY1 = 20;
        int buttonPosY2 = 50;
        if (displayGUI)
        {
            //GUI Box surrounding the buttons
            GUI.Box(new Rect(0, 0, 200, 200), "Main Menu");

            //Start button should download the DICOM files from the specified directory
            if (GUI.Button(new Rect(12.5f, buttonPosY1, buttonWidth, buttonLength), "Start"))
            {
                string path = @"C:\Users\Mackenzie\Desktop\pixelspacingtestimages\DISCIMG\IMAGES";
                DirectoryInfo directory = new DirectoryInfo(path);
                FileInfo[] info = directory.GetFiles();
                //The file folder containing the DICOM files must not be empty!
                Assert.IsNotNull(info);
                print(info.Length);
                displayGUI = false;
            }
            //Quit button should simply exit the application
            if (GUI.Button(new Rect(12.5f, buttonPosY2, buttonWidth, buttonLength), "Quit"))
            {
                Debug.Log("I'm quiting the app");
                print("Test?");
                Application.Quit();
            }
        }
    }
}
