using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Security.Permissions;
using UnityEngine;
using UnityEditor;
using UnityEngine.Assertions;

public class LoadBar : MonoBehaviour
{
    //skins and textures
    public GUISkin[] skins;
    public Texture2D file, folder, back, drive;

    //initialize file browser
    FileBrowser fb = new FileBrowser();
    string output = "no file";

    // Use this for initialization
    void Start()
    {
        //set the various textures
        fb.fileTexture = file;
        fb.directoryTexture = folder;
        fb.backTexture = back;
        fb.driveTexture = drive;

        //show the search bar, flase since we will not have keyboard implementation
        fb.showSearch = false;

        //search recursively (setting recursive search may cause a long delay)
        fb.searchRecursively = false;
    }


    //Function OnGUI() is built into Unity and will display the desired Filebrowser for us
    void OnGUI() {
        GUILayout.BeginHorizontal();
		GUILayout.BeginVertical();
		GUILayout.Space(10);
		foreach(GUISkin s in skins){
			if(GUILayout.Button(s.name)){
				fb.guiSkin = s;
			}
		}
		GUILayout.Space(10);

		GUILayout.EndVertical();
		GUILayout.Space(10);
		GUILayout.Label("Selected File: "+output);
		GUILayout.EndHorizontal();
		//draw and display output
		if(fb.draw()){
            //If the user selects 'cancel', the browser should quit
            //If the user selects an image, then it will be sent to the ImageManager
            if (fb.outputFile == null)
            {
                output = "Cancel Hit";
                this.enabled = false;
            }
            else
            {
                Assert.IsNotNull(fb.outputFile, "The file slected by the user should not be null");
                ConvertAndSendImage(fb.outputFile);
                Assert.IsFalse(this.enabled);
            }
		}
	}

    //Function ConvertAndSendImage() will take in a file which it will convert to a Texture2D and send it
    //to the ImageManager.  This is done by converting the file into an array of bytes and creating a new Texture
    //from it.
    //Preconditions: FileInfo file, the file to be converted
    //Postconditions: updated ImageManager list and disabling the file-browser GUI
    //Return: Nothing
    public void ConvertAndSendImage(FileInfo file)
    {
        //We can't do anything with a null file
        if(file == null)
        {
            return;
        }
        byte[] dicomImage = File.ReadAllBytes(file.ToString());
        //We also can't do anything with an empty file
        if(dicomImage.Length < 1)
        {
            return;
        }
        Assert.AreNotEqual(0, dicomImage.Length, "The array of bytes from the File should not be empty");
        //From bytes, this is where we will call and write the code to decipher DICOMs
        Texture2D image = new Texture2D(10, 10);
        image.LoadImage(dicomImage);
        //SendMessage("AddImage", image);// Kyle and Heramb, this is the function to add an image to the List
        this.enabled = false;
    }

    

}