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

    // Reference to the Display
    GameObject display;

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
    [SerializeField]
    private VRButton VRButtonPrefab;
    // Inital Position of the file Buttons
    public Vector3 filePosition;
    // Inital Rotation of the file Buttons
    public Vector3 fileRotation;
    // Inital Position of the file Buttons
    public Vector3 directoryPosition;
    // Inital Rotation of the file Buttons
    public Vector3 directoryRotation;
    // Distance between each button
    public float seperationBetweenButtons;

    // VRButton back to move back to the previous directory
    private VRButton backButton;
    // Back button position
    public Vector3 backPosition;
    // Back rotation
    public Vector3 backRotation;

    // VRButton cancel to exit out of the filebrowser
    private VRButton cancelButton;
    // Cancel button Position
    public Vector3 cancelPosition;
    // Cancel rotation
    public Vector3 cancelRotation;


    // Use this for initialization
    void Start()
    {
        cameraPosition = GameObject.FindGameObjectWithTag("MainCamera").transform;
        display = GameObject.FindGameObjectWithTag("Display");
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
        backButton = CreateBackButton(currentDirectory);
        cancelButton = CreateCancelButton();
    }


    void Update()
    {
        // We always want the LoadBar to be infront of the user.
        // this.transform.position = new Vector3(cameraPosition.position.x + 10f, cameraPosition.position.y, cameraPosition.position.z + 500f);
    }


    /// <summary>
    /// Function CreateButtons() will generate the list of all buttons and set up for the current
    /// layout of the current file browsing directory
    /// Preconditions: none
    /// Postconditions: creation of all buttons involved
    /// Return: nothing
    /// </summary>
    void CreateButtons()
    {
        // Create a directory button for each directory
        int count = 0;
        foreach (string i in listOfCurrentDirectories)
        {
            Vector3 newDirectoryPosition = directoryPosition;
            newDirectoryPosition.y = newDirectoryPosition.y - (count * seperationBetweenButtons);
            CreateDirectoryButton(i, newDirectoryPosition);
            count++;
        }

        // Create a file button for each file
        count = 0;
        foreach (string j in listOfCurrentFiles)
        {
            Vector3 newFilePosition = filePosition;
            newFilePosition.y = newFilePosition.y - (count * seperationBetweenButtons);
            CreateFileButton(j, newFilePosition);
            count++;
        }
    }


    /// <summary>
    /// Function DisableLoadBar() will disable the LoadBar so that it cannot be seen
    /// Preconditions: User selected cancel or submit button
    /// Postconditions: LoadBar disabled
    /// Return: nothing
    /// </summary>
    void DisableFileBrowser()
    {
        this.enabled = false;
    }


    /// <summary>
    /// Function EnableLoadBar() will enable the LoadBar so that it can be seen
    /// Preconditions: User selected the load button
    /// Postconditions: LoadBar enabled
    /// Return: nothing
    /// </summary>
    void EnableFileBrowser()
    {
        this.enabled = true;
    }


    /// <summary>
    /// Function EnterDirectory() will send the user to the specified directory and bring up the 
    /// all the buttons withing that directory
    /// </summary>
    /// <param name="newDirectory"></param>
    void EnterDirectory(string newDirectory)
    {
        Assert.IsNotNull(newDirectory);
        currentDirectory = newDirectory;
        // Destroy all current directory buttons
        foreach (VRButton d in listOfCurrentDirectoryButtons)
        {
            Destroy(d.gameObject);
        }
        // Destroy all current File buttons
        foreach (VRButton f in listOfCurrentFileButtons)
        {
            Destroy(f.gameObject);
        }
        // Empty all lists
        listOfCurrentDirectoryButtons.Clear();
        listOfCurrentFileButtons.Clear();
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
        UpdateBackButton(newDirectory);
        CreateButtons();
    }

    /// <summary>
    /// Function ConvertAndSendImage() will take in a file which it will convert to a Texture2D and send it
    /// to the ImageManager.  This is done by converting the file into an array of bytes and creating a new Texture
    /// from it.
    /// </summary>
    /// <param name="filePath">string representation of the files path</param>
    public void ConvertAndSendImage(string filePath)
    {
        FileInfo file = new FileInfo(filePath);
        // Can't do anything with a null file
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
        display.SendMessage("AddImage", image);
        this.enabled = false;
    }


    /// <summary>
    /// Function CreateDirectoryButton() will create a new button representative of a directory that
    /// the user can select to enter and view that directory's contents
    /// </summary>
    /// <param name="directoryPath">string of the directory path</param>
    /// <param name="position">position of the directory</param>
    void CreateDirectoryButton(string directoryPath, Vector3 position)
    {
        //we should contain a prefab and viable string
        Assert.IsNotNull(VRButtonPrefab);
        Assert.IsNotNull(directoryPath);
        // Create a Directory button to access the directory
        VRButton newDirectory = Instantiate(VRButtonPrefab, position,
            Quaternion.Euler(fileRotation));
        newDirectory.transform.parent = gameObject.transform;

        //Setting all the atributes of the new Directory button
        newDirectory.name = "Directory";
        newDirectory.manager = this.gameObject;
        newDirectory.path = directoryPath;
        newDirectory.textObject = newDirectory.GetComponentInChildren<TextMesh>();
        newDirectory.textObject.text = GetLocalName(directoryPath);
        listOfCurrentDirectoryButtons.Add(newDirectory);
    }


    /// <summary>
    /// Function CreateFileButton will create a new button that will represent the path of
    /// a file which the user can select to open
    /// </summary>
    /// <param name="filePath">string of the files path</param>
    /// <param name="position">position of the new file button</param>
    void CreateFileButton(string filePath, Vector3 position)
    {
        //we should contain a prefab and viable string
        Assert.IsNotNull(VRButtonPrefab);
        Assert.IsNotNull(filePath);
        // Create a File button to access the file
        VRButton newFile = Instantiate(VRButtonPrefab, position,
            Quaternion.Euler(directoryRotation));
        newFile.transform.parent = gameObject.transform;

        // Set each attribute of the button
        newFile.name = "File";
        newFile.manager = this.gameObject;
        newFile.path = filePath;

        newFile.textObject = newFile.GetComponentInChildren<TextMesh>();
        newFile.textObject.text = GetLocalName(filePath);
        listOfCurrentFileButtons.Add(newFile);
    }


    /// <summary>
    /// CreateBackButton will create a new back button for the interface.
    /// </summary>
    /// <param name="path">The path leading to the directory one level above</param>
    VRButton CreateBackButton(string path)
    {
        //we should contain a prefab and viable string
        Assert.IsNotNull(VRButtonPrefab);
        Assert.IsNotNull(path);
        VRButton back = Instantiate(VRButtonPrefab, backPosition,
            Quaternion.Euler(backRotation));
        back.transform.parent = gameObject.transform;

        // Set attributes of the back button
        back.name = "Back";
        back.manager = this.gameObject;
        back.path = GetPreviousPath(path);
        back.textObject = back.GetComponentInChildren<TextMesh>();
        back.textObject.text = "Back";
        Debug.Log("Just created Back " + back);

        return back;
    }


    /// <summary>
    /// CreateFunctionButton will create a new cancel button which will close down
    /// the file browser by deactivating it.
    /// </summary>
    /// <returns>The newly created cancel buttob</returns>
    VRButton CreateCancelButton()
    {
        //we should contain a prefab
        Assert.IsNotNull(VRButtonPrefab);
        cancelButton = Instantiate(VRButtonPrefab, cancelPosition,
            Quaternion.Euler(cancelRotation));
        cancelButton.transform.parent = gameObject.transform;

        // Setting the attributes of thr cancel button
        cancelButton.name = "Cancel";
        cancelButton.manager = this.gameObject;
        cancelButton.path = null;
        cancelButton.textObject = cancelButton.GetComponentInChildren<TextMesh>();
        cancelButton.textObject.text = "Cancel";
    
        return cancelButton;
    }


    /// <summary>
    /// Function GoBack() will call EnterDirectory on the path above the current path.  This 
    /// will generate all directory and file buttons for that directory.
    /// </summary>
    void GoBack()
    {
        EnterDirectory(GetPreviousPath(currentDirectory));
    }


    /// <summary>
    /// UpdateBackButton will update the path contained in the back button
    /// </summary>
    /// <param name="path">new path to update the back button</param>
    void UpdateBackButton(string path)
    {
        backButton.path = GetPreviousPath(path);
        backButton.GetComponentInChildren<TextMesh>().text = "Back";
    }


    /// <summary>
    /// GetLocalName will get the name of the directory given it's path.
    /// The last name in the given path will be returned
    /// Pre:: string path
    /// Post:: nothing
    /// Return:: string name
    /// </summary>
    /// <param name="path">string of the path</param>
    /// <returns> string local name </returns>
   public string GetLocalName(string path)
    {
        Assert.IsNotNull(path);
        int index = path.LastIndexOf("\\");
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


    /// <summary>
    /// This function will take in a string representing a file or directory path and
    /// and return the directory path to that directory or file
    /// </summary>
    /// <param name="path">string of the path given to the function</param>
    /// <returns>string of the path</returns>
    string GetPreviousPath(string path)
    {
        int index = path.LastIndexOf("\\");
        if(index > 0)
        {
            path = path.Substring(0, index);
            return path;
        }
        return path;
    }


    /// <summary>
    /// This function comes from the VRButton interface.  It thakes in a string reptrsenting
    /// the VRButton it recieves.  Based on the name, it will execute the specified function.
    /// </summary>
    /// <param name="button"></param>
    public void VRButtonClicked(string button)
    {
        switch (button)
        {
            case "Back":
                GoBack();
                break;
            case "Cancel":
                DisableFileBrowser();
                break;
        }
    }
}
