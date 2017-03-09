using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.IO;
using System;

public class FileBrowser1 : MonoBehaviour, IVRButton
{
    // Position of the Camera
    Transform cameraPosition;

    // Storage of the path name to the file we want to open
    string selectedFile;
    // Path name of the current directory
    string currentDirectory;
    // List of all directories within the current directory
    List<string> listOfCurrentDirectories = new List<string>();
    // List of all files in the current directory
    List<string> listOfCurrentFiles = new List<string>();

    // List of all Directory Buttons within the current directory
    List<VRButton> listOfCurrentDirectoryButtons = new List<VRButton>();
    // List of all File Buttons within the current directory
    List<VRButton> listOfCurrentFileButtons = new List<VRButton>();

    // VRButton prefab to create the Buttons
    public VRButton VRButtonPrefab;
    // Inital Position of the file Buttons
    public Vector3 filePosition;
    // Inital Rotation of the file Buttons
    public Vector3 fileRotation;
    // Inital Position of the file Buttons
    public Vector3 directoryPosition;
    // Inital Rotation of the file Buttons
    public Vector3 directoryRotation;
    // VRButton back to move back to the previous directory
    private VRButton back;


    // Use this for initialization
    void Start()
    {
        cameraPosition = GameObject.FindGameObjectWithTag("MainCamera").transform;
        selectedFile = "Nothing Selected";
        // Get the current Directory
        currentDirectory = Directory.GetCurrentDirectory().ToString();

        // Get all directories in the current directory and put them into a list
        string[] arrayOfCurrentDirectories = Directory.GetDirectories(currentDirectory);
        foreach (string i in arrayOfCurrentDirectories){
            listOfCurrentDirectories.Add(i);
        }

        // Get all files in the current directory and put them into a list
        string[] arrayOfCurrentFiles = Directory.GetFiles(currentDirectory);
        foreach (string i in arrayOfCurrentFiles) {
            listOfCurrentFiles.Add(i);
        }
        //Create all directory and file buttons
        CreateButtons();
    }

    //Function CreateButtons() will generate the list of all buttons and set up for the current
    //layout of the current file browsing directory
    //Preconditions: none
    //Postconditions: creation of all buttons involved
    //Return: noting
    void CreateButtons()
    {
        int count = 0;
        foreach (string i in listOfCurrentDirectories)
        {
            Vector3 newDirectoryPosition = directoryPosition;
            newDirectoryPosition.y = newDirectoryPosition.y - (count * 0.1f);
            CreateDirectoryButton(i, newDirectoryPosition);
            count++;
        }

        count = 0;
        foreach (string j in listOfCurrentFiles)
        {
            Vector3 newFilePosition = filePosition;
            newFilePosition.y = newFilePosition.y - (count * 0.1f);
            CreateFileButton(j, newFilePosition);
            count++;

        }
        
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

        listOfCurrentDirectories.Clear();
        listOfCurrentFiles.Clear();

        string[] arrayOfCurrentDirectories = Directory.GetDirectories(currentDirectory);
        foreach (string i in arrayOfCurrentDirectories)
        {
            listOfCurrentDirectories.Add(i);
        }
        string[] arrayOfCurrentFiles = Directory.GetFiles(currentDirectory);
        foreach (string i in arrayOfCurrentFiles)
        {
            listOfCurrentFiles.Add(i);
        }

        CreateButtons();
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
    void CreateDirectoryButton(string directoryPath, Vector3 position)
    {
        Assert.IsNotNull(VRButtonPrefab);
        Assert.IsNotNull(directoryPath);
        // Create a Directory button to access the directory
        VRButton newDirectory = Instantiate(VRButtonPrefab, position,
            Quaternion.Euler(fileRotation));
        newDirectory.transform.parent = gameObject.transform;

        newDirectory.name = "Directory";
        //newDirectory.manager = this.gameObject;
        //newDirectory.textObject.text = null; // GetLocalName(directoryPath);
        newDirectory.path = directoryPath;
        listOfCurrentDirectoryButtons.Add(newDirectory);
    }

    //Function create file Button will create a new button that will represent the path of
    //a file which the user can select to open
    //Preconditions: string filePath
    //Postconditions: creation of file button
    //Return: nothing
    void CreateFileButton(string filePath, Vector3 position)
    {
        // Create a File button to access the file
        VRButton newFile = Instantiate(VRButtonPrefab, position,
            Quaternion.Euler(directoryRotation));
        newFile.transform.parent = gameObject.transform;

        // Set the name to 
        newFile.name = "File";
        //newFile.manager = this.gameObject;
        //newFile.textObject.text = GetLocalName(filePath);
        newFile.path = filePath;

        listOfCurrentFileButtons.Add(newFile);
    }

    /// <summary>
    /// CreateBackButton will create a new back button for the interface.
    /// </summary>
    /// <param name="path"></param>
    void CreateBackButton(string path)
    {
        VRButton back = Instantiate(VRButtonPrefab, filePosition,
            Quaternion.Euler(fileRotation));
        back.transform.parent = gameObject.transform;

        back.name = "Back";
        //back.manager = this.gameObject;
        back.path = GetPreviousPath(path);
    }

    string GetLocalName(string path)
    {
        int index = path.LastIndexOf("/");
        if (index > 0)
        {
            path = path.Substring(index + 1);
            return path;
        }
        else
        {
            return path;
        }
    }

    // 
    string GetPreviousPath(string path)
    {
        int index = path.LastIndexOf("/");
        if(index > 0)
        {
            path = path.Substring(0, index);
            return path;
        }
        return path;
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
