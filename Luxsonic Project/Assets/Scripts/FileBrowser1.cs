using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.IO;
using System;

public class FileBrowser1 : MonoBehaviour, IVRButton
{

    Transform cameraPosition;
    string selectedFile;
    string currentDirectory;
    List<string> listOfCurrentDirectories;
    List<string> listOfCurrentFiles;

    //VRButton prefab to create the Buttons
    public VRButton VRButtonPrefab;

    public Vector3 filePosition;
    public Vector3 fileRotation;


    // Use this for initialization
    void Start()
    {
        cameraPosition = GameObject.FindGameObjectWithTag("MainCamera").transform;
        selectedFile = "Nothing Selected";
        currentDirectory = Directory.GetCurrentDirectory().ToString();
        string[] arrayOfCurrentDirectories = Directory.GetDirectories(currentDirectory);
        foreach (string i in arrayOfCurrentDirectories){
            listOfCurrentDirectories.Add(i);
        }
        string[] arrayOfCurrentFiles = Directory.GetFiles(currentDirectory);
        foreach (string i in arrayOfCurrentFiles)
        {
            listOfCurrentFiles.Add(i);
        }
    }

    //Function CreateButtons() will generate the list of all buttons and set up for the current
    //layout of the current file browsing directory
    //Preconditions: none
    //Postconditions: creation of all buttons involved
    //Return: noting
    void CreateButtons()
    {

        foreach (string i in listOfCurrentDirectories)
        {
            CreateDirectoryButton(i);
        }

        foreach (string j in listOfCurrentFiles)
        {
            CreateFileButton(j);
        }

        // Create a Submit button
        VRButton submit = Instantiate(VRButtonPrefab, filePosition,
            Quaternion.Euler(fileRotation));
        submit.transform.parent = gameObject.transform;

        submit.name = "File";
        submit.manager = this.gameObject;
        
    }

    //Function DisableLoadBar() will disable the LoadBar so that it cannot be seen
    //Preconditions: User selected cancel or submit button
    //Postconditions: LoadBar disabled
    //Return: nothing
    void DisableFileBrowser()
    {
        this.enabled = false;
    }

    //Function EnableLoadBar() will enable the LoadBar so that it can be seen
    //Preconditions: User selected the load button
    //Postconditions: LoadBar enabled
    //Return: nothing
    void EnableFileBrowser()
    {
        this.enabled = true;
    }

    //Function EnterDirectory() will send the user to the specified directory and bring up the 
    //all the buttons withing that directory
    void EnterDirectory(string newDirectory)
    {
        currentDirectory = newDirectory;


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
    void CreateDirectoryButton(string directoryPath)
    {
        // Create a Directory button to access the directory
        VRButton newDirectory = Instantiate(VRButtonPrefab, filePosition,
            Quaternion.Euler(fileRotation));
        newDirectory.transform.parent = gameObject.transform;

        newDirectory.name = "Directory";
        newDirectory.textObject.text = directoryPath;
        newDirectory.manager = this.gameObject;
    }

    //Function create file Button will create a new button that will represent the path of
    //a file which the user can select to open
    //Preconditions: none
    //Postconditions: creation of file button
    //Return: nothing
    void CreateFileButton(string filePath)
    {
        // Create a File button to access the file
        VRButton newFile = Instantiate(VRButtonPrefab, filePosition,
            Quaternion.Euler(fileRotation));
        newFile.transform.parent = gameObject.transform;

        newFile.name = "File";
        newFile.textObject.text = filePath;
        newFile.manager = this.gameObject;
    }

    void Update()
    {
        //We always want the LoadBar to be infront of the user.
        this.transform.position = new Vector3(cameraPosition.position.x + 10f, cameraPosition.position.y, cameraPosition.position.z + 10f);
    }

    public void VRButtonClicked(string button)
    {
        switch (button)
        {
            case "Back":
                //Submit()
                break;
            case "Cancel":
                //Cancel()
                break;
            case "File":
                //GetFile()
                break;
            case "Directory":
                //EnterDirectory()
                break;
        }
    }
}
