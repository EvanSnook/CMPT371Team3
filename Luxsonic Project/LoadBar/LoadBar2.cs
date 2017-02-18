using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.IO;

public class LoadBar2 : MonoBehaviour {

    Transform cameraPosition;
    string selectedFile;
    string currentDirectory;
    string[] listOfCurrentDirectories;
    string[] listOfCurrentFiles;


	// Use this for initialization
	void Start () {
        cameraPosition = GameObject.FindGameObjectWithTag("MainCamera").transform;
        selectedFile = "Nothing Selected";
        currentDirectory = Directory.GetCurrentDirectory().ToString();
        listOfCurrentDirectories = Directory.GetDirectories(currentDirectory);
        listOfCurrentFiles = Directory.GetFiles(currentDirectory);
	}

    //Function CreateButtons() will generate the list of all buttons and set up for the current
    //layout of the current file browsing directory
    //Preconditions: none
    //Postconditions: creation of all buttons involved
    //Return: noting
    void CreateButtons()
    {

        foreach(string i in listOfCurrentDirectories)
        {
            
        }
    }

    //Function DisableLoadBar() will disable the LoadBar so that it cannot be seen
    //Preconditions: User selected cancel or submit button
    //Postconditions: LoadBar disabled
    //Return: nothing
    void DisableLoadBar()
    {
        this.enabled = false;
    }

    //Function EnableLoadBar() will enable the LoadBar so that it can be seen
    //Preconditions: User selected the load button
    //Postconditions: LoadBar enabled
    //Return: nothing
    void EnableLoadBar()
    {
        this.enabled = true;
    }

    //Function EnterDirectory() will send the user to the specified directory and bring up the 
    //all the buttons withing that directory
    void EnterDirectory(string directory)
    {

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
        if (file == null)
        {
            return;
        }
        byte[] dicomImage = File.ReadAllBytes(file.ToString());
        //We also can't do anything with an empty file
        if (dicomImage.Length < 1)
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

    //Function CreateDirectoryButton() will create a new button representative of a directory that
    //the user can select to enter and view that directory's contents
    //Preconditions: none
    //Postconditions: creation of a new directory button
    //returns: nothing
    void CreateDirectoryButton()
    {

    }

    //Function create file Button will create a new button that will represent the path of
    //a file which the user can select to open
    //Preconditions: none
    //Postconditions: creation of file button
    //Return: nothing
    void CreateFileButton()
    {

    }

    void Update()
    {
        //We always want the LoadBar to be infront of the user.
        this.transform.position = new Vector3(cameraPosition.position.x + 10f, cameraPosition.position.y, cameraPosition.position.z + 10f);
    }
}
