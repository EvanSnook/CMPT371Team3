using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using System.IO;

public class MainMenu : MonoBehaviour
{

    bool displayGUI;
    SpriteRenderer testing1;//For testing DELETE
    // Use this for initialization
    void Start()
    {
        displayGUI = true;
        testing1 = GetComponentInChildren<SpriteRenderer>();//For testing DELETE
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
                //Working on searching all files for a universal DICOM file with pngs
                //string[] paths = Directory.GetDirectories(@"C:\", "D*");
                //print(paths.Length);
                //Assert.Equals(paths.Length, 1);

                string path = @"C:\Users\Mackenzie\Desktop\DICOMImages";
                
                DirectoryInfo directory = new DirectoryInfo(path);
                FileInfo[] info = directory.GetFiles();

                //The files should be present so we will assert this
                Assert.IsNotNull(info, "The array of files is null, but should contain files");
                Assert.AreNotEqual<int>(0, info.Length);

                byte[] dicomImages = File.ReadAllBytes(info[0].ToString());
                Texture2D testpic = new Texture2D(10, 10);
               // testpic.LoadImage(dicomImages);
                
                //print(info.Length);
                //testing1.sprite = Sprite.Create(testpic, new Rect(0, 0, testpic.width, testpic.height), new Vector2(0.5f, 0.5f));
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
