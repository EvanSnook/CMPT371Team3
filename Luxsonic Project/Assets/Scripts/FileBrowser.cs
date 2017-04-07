using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.IO;
using System;
using System.Threading;

using buttons;

using Dicom;
using Dicom.Imaging;
using Dicom.Log;
/// <summary>
/// The FileBrowser1 class represents the script for generating a virtual filebrowser that the user can
/// navigate to seach for image and dicom files for use in the program
/// </summary>
public class FileBrowser : MonoBehaviour, IVRButton
{
	// Reference to the Dashboard
	Dashboard dashboard;

	// Reference the Display
	Display display;

	// Path name of the current directory
	string currentDirectory;
	// List of all directories within the current directory
	List<string> listOfCurrentDirectories = new List<string>();
	// List of all files in the current directory
	List<string> listOfCurrentFiles = new List<string>();

	// List of all Directory Buttons within the current directory
    LinkedList<GameObject> listOfCurrentDirectoryButtons = new LinkedList<GameObject>();
	// List of all File Buttons within the current directory
    LinkedList<GameObject> listOfCurrentFileButtons = new LinkedList<GameObject>();

    // Holds references to the instantiated buttons
    private List<GameObject> buttonObjects = new List<GameObject>();

    // Holds the attributes for each button to be instantiated
    [SerializeField]
    private List<ButtonAttributes> buttonList = new List<ButtonAttributes>();

    // VRButton prefab to create the Buttons
    [SerializeField]
    private GameObject VRFileButtonPrefab;
    [SerializeField]
    private GameObject VRDirectoryButtonPrefab;
    [SerializeField]
    private GameObject VROtherButtonPrefab;

	// Inital Position of the file Buttons
	public Vector3 filePosition;
	// Inital Rotation of the file Buttons
	public Vector3 fileRotation;
	// Inital Position of the file Buttons
	public Vector3 directoryPosition;
	// Inital Rotation of the file Buttons
	public Vector3 directoryRotation;
	// Distance between file and directory buttons in the Y position
	public float seperationBetweenButtonsY;

	// The button limit on the filebrowser
	[SerializeField]
	private int buttonLimit;


	// Use this for initialization
	void Start()
	{
		display = GameObject.FindGameObjectWithTag("Display").GetComponent<Display>();
		dashboard = GameObject.FindGameObjectWithTag("Dashboard").GetComponent<Dashboard>();
		// Get the current Directory
		currentDirectory = Directory.GetCurrentDirectory().ToString();
		// Get all directories in the current directory and put them into a list
		GetCurrentDirectories();
		// Get all files in the current directory and put them into a list
		GetCurrentFiles();
		// Create all directory and file buttons
		CreateAllButtons();
		// The filebrowser should not be visable initally
		DisableFileBrowser();
	}


	/// <summary>
	/// Function GetListOfFilePaths will return the list of file paths currently stored
	/// in the FileBrowser
	/// Pre:: nothing
	/// Post:: nothing
	/// Return:: List of file paths as strings
	/// </summary>
	/// <returns> list of file paths</returns>
	public List<string> GetListOfFilePaths()
	{
		return this.listOfCurrentFiles;
	}


	/// <summary>
	/// Function GetCurrentDirectory will return the current directory
	/// Pre:: nothing
	/// Post:: nothing
	/// Return:: string of directory
	/// </summary>
	/// <returns> string of current directory</returns>
	public string GetCurrentDirectory()
	{
		return this.currentDirectory;
	}


	/// <summary>
	/// Function GetListOfDirectory Paths will return the list of directory paths currently stored
	/// in the FileBrowser
	/// Pre:: nothing
	/// Post:: nothing
	/// Return:: List of directory paths as strings
	/// </summary>
	/// <returns> list of directory paths</returns>
	public List<string> GetListOfDirectoryPaths()
	{
		return this.listOfCurrentDirectories;
	}


	/// <summary>
	/// Function GetListOfFileButtons will return the list of File buttons currently stored
	/// in the FileBrowser
	/// Pre:: nothing
	/// Post:: nothing
	/// Return:: List of file buttons
	/// </summary>
	/// <returns> list of VRButtons paths</returns>
    public LinkedList<GameObject> GetListOfFileButtons()
	{
		return this.listOfCurrentFileButtons;
	}


	/// <summary>
	/// Function GetListOfDirectoryButtons will return the list of directory buttons currently stored
	/// in the FileBrowser
	/// Pre:: nothing
	/// Post:: nothing
	/// Return:: List of directory buttons
	/// </summary>
	/// <returns> list of VRButtons paths</returns>
    public LinkedList<GameObject> GetListOfDirectoryButtons()
	{
		return this.listOfCurrentDirectoryButtons;
	}


	/// <summary>
	/// This function sets the current directory to the path name given in the argument.
	/// Pre:: nothing
	/// Post:: sets current directory
	/// Return:: nothing
	/// </summary>
	/// <param name="path">string we want to set the directory to</param>
	public void SetCurrentDirectory(string path)
	{
		this.currentDirectory = path;
	}


    /// <summary>
    /// Creates all buttons used in the file browser on start.
    /// Pre:: nothing
    /// Post:: creation of all buttons
    /// Return:: nothing
    /// </summary>
    private void CreateAllButtons()
    {
        //Create menu buttons
        foreach (ButtonAttributes attributes in buttonList)
        {
            buttonObjects.Add(CreateButton(attributes, VROtherButtonPrefab));
        }

        // Create file and directory buttons
        CreateFileAndDirectoryButtons();

        // Set the top and bottom directory/file buttons
        if (GetListOfDirectoryButtons().Count > 0)
        {
            ShowLimitedButtons(listOfCurrentDirectoryButtons, this.directoryPosition.y);
        }
        if (GetListOfDirectoryButtons().Count > 0)
        {
            ShowLimitedButtons(listOfCurrentFileButtons, this.filePosition.y);
        }
    }


    /// <summary>
    /// Creates new button, and applies passed in attributes. 
    /// </summary>
    /// <param name="attributes">Attributes to be applied to the new button</param>
    /// <param name="buttonPrefab">Prefab to instantiate as a button</param>
    /// <returns>Newly created button GameObject</returns>
    public GameObject CreateButton(ButtonAttributes attributes, GameObject buttonPrefab)
    {
        GameObject newButton = Instantiate(buttonPrefab, attributes.position,
            Quaternion.Euler(attributes.rotation));

        newButton.transform.parent = gameObject.transform;

        newButton.GetComponent<VRButton>().Initialise(attributes, this.gameObject);
        newButton.name = attributes.buttonName;

        return newButton;
    }


	/// <summary>
	/// Function CreateFileAndDirectoryButtons() will generate the list of all directory and file buttons and set up for the current
	/// layout of the current file browsing directory
	/// Preconditions: none
	/// Postconditions: creation of all buttons involved
	/// Return: nothing
	/// </summary>
	void CreateFileAndDirectoryButtons()
	{
        int count = 0;
        foreach (string directory in listOfCurrentDirectories)
        {
            Vector3 newDirectoryPosition = new Vector3(directoryPosition.x, 
				directoryPosition.y - (count * seperationBetweenButtonsY), directoryPosition.z);

            string[] arguments = new string[1];
            arguments[0] = directory;

            ButtonAttributes currentButtonAttributes = new ButtonAttributes();
            currentButtonAttributes.buttonName = GetLocalName(directory);
            currentButtonAttributes.type = ButtonType.DIRECTORY_BUTTON;
            currentButtonAttributes.position = newDirectoryPosition;
            currentButtonAttributes.rotation = directoryRotation;
            currentButtonAttributes.buttonParameters = arguments;
            currentButtonAttributes.buttonFunction = "EnterDirectory";
            currentButtonAttributes.depressable = false;
            currentButtonAttributes.autoPushOut = false;

            listOfCurrentDirectoryButtons.AddLast(CreateButton(currentButtonAttributes, VRDirectoryButtonPrefab));

            count++;
        }
        // Create a file button for each file
        count = 0;
        foreach (string file in listOfCurrentFiles)
        {
            Vector3 newFilePosition = new Vector3(filePosition.x,
                filePosition.y - (count * seperationBetweenButtonsY), filePosition.z);

            string[] arguments = new string[2];
            arguments[0] = "f";
            arguments[1] = file;

            ButtonAttributes currentButtonAttributes = new ButtonAttributes();
            currentButtonAttributes.buttonName = GetLocalName(file);
            currentButtonAttributes.type = ButtonType.DIRECTORY_BUTTON;
            currentButtonAttributes.position = newFilePosition;
            currentButtonAttributes.rotation = fileRotation;
            currentButtonAttributes.buttonParameters = arguments;
            currentButtonAttributes.buttonFunction = "LoadFiles";
            currentButtonAttributes.depressable = false;
            currentButtonAttributes.autoPushOut = false;

            listOfCurrentFileButtons.AddLast(CreateButton(currentButtonAttributes, VRFileButtonPrefab));

            count++;
        }
	}
	

    /// <summary>
    /// 
    /// </summary>
    void LoadCurrentDirectory()
    {
        string[] args = new string[3];
        args[0] = "d";
        args[1] = this.currentDirectory;
        LoadFiles(args);
    }


	/// <summary>
	/// 
	/// </summary>
	/// <param name="args"></param>
	void LoadFiles(string []args)
	{
		dashboard.SendMessage("Minimize");

        StartCoroutine(LoadFilesCoroutine(args));
	}


    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
	public IEnumerator LoadFilesCoroutine(string[] args)
	{
		var type = args[0];
		var targetPath = args[1];
		var destinationPath = Application.persistentDataPath + @"\.temp_images";

		DeleteAllImagesInPath(destinationPath);

		yield return StartCoroutine(gameObject.GetComponent<DICOMConverter>().ExternalConverter(type, targetPath, destinationPath));

		//call ConvertAndSerndImages() on everything in (Application.persistentDataPath + \tempImages)
		string[] arrayOfFiles = Directory.GetFiles(destinationPath);

		Dictionary<string, string> patientInfo = GetPatientInfo(targetPath);

		foreach (string filePath in arrayOfFiles)
		{
			yield return StartCoroutine(ConvertAndSendImage(filePath, patientInfo));
			yield return null;
		}

		DeleteAllImagesInPath(destinationPath);
		DisableFileBrowser();

	}


    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
	public Dictionary<string, string> GetPatientInfo(string path)
	{

		//Debug.Log(path);
		Dicom.DicomFile obj = Dicom.DicomFile.Open(path);
		Dictionary<string, string> patientInfo = new Dictionary<string, string>();
		patientInfo.Add("PatientName", (obj.Dataset.Get<string>(Dicom.DicomTag.PatientName, null)));
		patientInfo.Add("PatientID", (obj.Dataset.Get< string>(Dicom.DicomTag.PatientID, null )));
		patientInfo.Add("PatientBirthDate", (obj.Dataset.Get<string>(Dicom.DicomTag.PatientBirthDate, null)));
		patientInfo.Add("PatientSex", (obj.Dataset.Get<string>(Dicom.DicomTag.PatientSex, null)));
		patientInfo.Add("StudyDescription", (obj.Dataset.Get<string>(Dicom.DicomTag.StudyDescription, null)));

		return patientInfo;

	}


	/// <summary>
	/// Deletes all files in the specified path
	/// </summary>
	/// <param name="path">Target path for deletion</param>
	void DeleteAllImagesInPath (String path)
	{
		if (Directory.Exists(path))
		{
			string[] arrayOfFiles = Directory.GetFiles(path);
			foreach (string filePath in arrayOfFiles)
			{
				File.Delete(filePath);
			}
		}
	}


	/// <summary>
	/// Function DisableFileBrowser() will disable the FileBrowser so that it cannot be seen
	/// Preconditions: none
	/// Postconditions: FileBrowser is disabled if it is not currently
	/// Return: nothing
	/// </summary>
	///</summary>
	void DisableFileBrowser()
	{
		this.gameObject.SetActive(false);
	}


	/// <summary>
	/// Function EnableFileBrowser() will enable the FileBrowser so that it can be seen
	/// Preconditions: none
	/// Postconditions: FileBrowser enabled
	/// Return: nothing
	/// </summary>
	void EnableFileBrowser()
	{
		this.gameObject.SetActive(true);
	}


	/// <summary>
	/// Function EnterDirectory() will send the user to the specified directory and bring up the 
	/// all the buttons withing that directory
	/// Pre:: string of the directory's path
	/// Post:: current directory is set to the new directory, and the list of directory
	/// buttons and files are reset and given the values of the directory we are entering
	/// </summary>
	/// <param name="newDirectory">string of the path representing the directory
	/// we are entering</param>
	void EnterDirectory(string[] arguments)
	{
        string newDirectory = arguments[0];

        Assert.IsNotNull(newDirectory);
		string storeCurrent = currentDirectory;
		currentDirectory = newDirectory;
		// Destroy all current directory buttons
        foreach (GameObject d in listOfCurrentDirectoryButtons)
		{
			Destroy(d);
		}
		// Destroy all current File buttons
        foreach (GameObject f in listOfCurrentFileButtons)
		{
			Destroy(f);
		}
		// Empty all lists
		listOfCurrentDirectoryButtons.Clear();
		listOfCurrentFileButtons.Clear();
		listOfCurrentDirectories.Clear();
		listOfCurrentFiles.Clear();
		// Get the new list of directories and files
		try {
			GetCurrentDirectories();
			GetCurrentFiles();
			// Update the path of the Back button
			UpdateBackButton(newDirectory);
			// Create file and directory buttons
			CreateFileAndDirectoryButtons();
		}
		catch(UnauthorizedAccessException)
		{
            string[] args = new string[1];
            args[0] = storeCurrent;
            EnterDirectory(args);
		}
        ShowLimitedButtons(this.listOfCurrentDirectoryButtons, this.directoryPosition.y);
        ShowLimitedButtons(this.listOfCurrentFileButtons, this.filePosition.y);
	}


	/// <summary>
	/// Function GetCurrentFiles() will store the paths of each file in the 
	/// listOfCurrentFiles attribute.
	/// Pre:: The listOfCurrentFiles must be empty
	/// Post:: list populated with new files.
	/// </summary>
	public void GetCurrentFiles()
	{
		// List should be empty
		Assert.AreEqual(0, listOfCurrentFiles.Count);
		// The function GetFiles returns an array, so we want to place them in a list
		// for easier use.
		string[] arrayOfCurrentFiles = Directory.GetFiles(currentDirectory);
		foreach (string file in arrayOfCurrentFiles)
		{
			listOfCurrentFiles.Add(file);
		}
	}


	/// <summary>
	/// Function GetCurrentDirectories() will store the paths of each directory in the 
	/// listOfCurrentDirectories attribute.
	/// Pre:: The listOfCurrentDirectories must be empty
	/// Post:: list populated with new directories.
	/// </summary>
	public void GetCurrentDirectories()
	{
		// List should be empty
		Assert.AreEqual(0, listOfCurrentDirectories.Count);
		// The function GetDirectories returns an array, so we want to place them in a list
		// for easier use.
		string[] arrayOfCurrentDirectories = Directory.GetDirectories(this.currentDirectory);
		foreach (string directory in arrayOfCurrentDirectories)
		{
			this.listOfCurrentDirectories.Add(directory);
		}
	}


	/// <summary>
	/// Function ConvertAndSendImage() will take in a file path which it will convert to a Texture2D and send it
	/// to the Display. This is done by converting the file into an array of bytes and creating a new Texture2D
	/// from it.
	/// Pre:: file path is not null
	/// Post:: send created Texture2D to Display
	/// Return:: nothing
	/// </summary>
	/// <param name="filePath">string representation of the files path</param>
	public IEnumerator ConvertAndSendImage(string filePath, Dictionary<string,string> patientInfo)
	{
		FileInfo file = new FileInfo(filePath);
		// Can't do anything with a null file
		Assert.AreNotEqual(null, file, "The file should not be null");
		byte[] dicomImage = File.ReadAllBytes(file.ToString());

		// We need to supress an unused variable warning. Unity views this as unused
		// because it's not used here but in a new thread.
		#pragma warning disable 0219
		byte[] result = null;
		#pragma warning restore 0219
		Thread newThread = new Thread(() => { result = convertToBytes(file.ToString()); });
		newThread.Start();

		while (newThread.IsAlive)
		{
			yield return null;
		}

		//We also can't do anything with an empty file
		Assert.AreNotEqual(0, dicomImage.Length, "The array of bytes from the File should not be empty");
		//From bytes, this is where we will call and write the code to decipher DICOMs
		Texture2D image = new Texture2D(10, 10);
		image.LoadImage(dicomImage);
		image.name = filePath;

		display.AddImage(image, patientInfo);
	}


    /// <summary>
    /// 
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
	public byte[] convertToBytes(String file)
	{
		byte[] dicomImage = File.ReadAllBytes(file);
		return dicomImage;
	}


	/// <summary>
	/// Function GoBack() will call EnterDirectory on the path above the current path.  This 
	/// will generate all directory and file buttons for that directory.
	/// Pre:: current directory is not null
	/// Post:: current directory is switched to that of the previous directory
	/// Return:: nothing
	/// </summary>
	public void GoBack()
	{
		Assert.IsNotNull(currentDirectory);
        string[] arguments = new string[1];
        arguments[0] = GetPreviousPath(currentDirectory);
        EnterDirectory(arguments);
	}


	/// <summary>
	/// UpdateBackButton will update the path contained in the back button
	/// Pre:: string of thr path is not null
	/// Post:: back button's path is now updated
	/// Return:: nothing
	/// </summary>
	/// <param name="path">new path to update the back button</param>
	void UpdateBackButton(string path)
	{
        GameObject backButton = this.buttonObjects.Find(button => button.name == "Back");
        backButton.GetComponent<VRButton>().SetPath(GetPreviousPath(path));
		backButton.GetComponentInChildren<TextMesh>().text = "Back";
	}



	/// <summary>
	/// GetLocalName will get the name of the directory given it's path.
	/// The last name in the given path will be returned
	/// Pre:: string path to get the local name from
	/// Post:: nothing
	/// Return:: string of the last word at the end of the path
	/// </summary>
	/// <param name="path">string of the path</param>
	/// <returns> string local name </returns>
	public string GetLocalName(string path)
	{
		Assert.IsNotNull(path);
		// Get the index in the string where the last '\' is present
		int index = path.LastIndexOf("\\");
		// if the index is bigger than 0, then we can get the local name
		if (index > 0)
		{
			// we don't want the '\' in the name, so we add 1.
			path = path.Substring(index + 1);
			return path;
		}
		// the string given to us is already local
		else
		{
			return path;
		}
	}


	/// <summary>
	/// This function will take in a string representing a file or directory path and
	/// and return the directory path to that directory or file
	/// Pre:: string path to get the previous path from
	/// Post:: nothing
	/// Return:: string of path one directory above 
	/// </summary>
	/// <param name="path">string of the path given to the function</param>
	/// <returns>string of the path</returns>
	string GetPreviousPath(string path)
	{
		Assert.IsNotNull(path, "The path must not be null");
		DirectoryInfo parent = Directory.GetParent(path);
		if (parent == null)
		{
			return path;
		}
		else
		{
			return parent.ToString();
		}

	}


	/// <summary>
	/// The function ShowLimitedButtons will diplay the file and or directory buttons within the user's view.
	/// Buttons outside of the user's view are hidden.
	/// Pre:: nothing
	/// Post:: sets which buttons are active
	/// Return:: nothing
	/// </summary>
    public void ShowLimitedButtons(LinkedList<GameObject> buttonList, float highestYPosition)
	{
        foreach (GameObject button in buttonList)
		{
            if (button.transform.position.y > (highestYPosition+(0.5*this.seperationBetweenButtonsY)))
			{
                button.SetActive(false);
			}
			else if(button.transform.position.y <= (highestYPosition - (this.buttonLimit*this.seperationBetweenButtonsY)))
			{
                button.SetActive(false);
			}
			else
			{
				button.SetActive(true);
			}
		}
	}


	/// <summary>
	/// 
	/// </summary>
	/// <param name="list"></param>
    private void ScrollDown(string[] target)
	{
        LinkedList<GameObject> list = null;
        float highestButton = 0f;
        if (target[0] == "files")
        {
            list = this.listOfCurrentFileButtons;
            highestButton = filePosition.y;
        }
        else if (target[0] == "directories")
        {
            list = this.listOfCurrentDirectoryButtons;
            highestButton = directoryPosition.y;
        }

		// We should not be able to scroll if the amount of buttons present is less than the limit
		if (list.Count > this.buttonLimit)
		{
			//Transform lastValue = list.Last.Value.GetComponent<Transform>();
			// We can only scroll down if we are not at the bottom of the list
			if (list.Last.Value.gameObject.activeSelf == false)
			{
                foreach (GameObject button in list)
				{
					// All buttons are shifted up one position
					Transform oldPosition = button.GetComponent<Transform>();
					oldPosition.position = new Vector3(oldPosition.position.x,
						oldPosition.position.y + this.seperationBetweenButtonsY, oldPosition.position.z);
				}
				ShowLimitedButtons(list, highestButton);
			}
		}
	}


	/// <summary>
	/// Scroll up will scroll up the list of file or directory buttons.  The position
	/// of each button in the list will shift down one position, giving the impression of
	/// scrolling down.
	/// Pre:: The list of buttons are either files or directories
	/// Post:: position of buttons shift down
	/// Return:: nothing
	/// </summary>
	/// <param name="list">The list of buttons to be shifted</param>
	/// <param name="highestButton"></param>
    private void ScrollUp(string[] target)
	{
        LinkedList<GameObject> list = null;
        float highestButton = 0f;
        if (target[0] == "files")
        {
            list = this.listOfCurrentFileButtons;
            highestButton = filePosition.y;
        }
        else if (target[0] == "directories")
        {
            list = this.listOfCurrentDirectoryButtons;
            highestButton = directoryPosition.y;
        }

        // We should not be able to scroll if the amount of buttons present is less than the limit
        if (list.Count > this.buttonLimit)
		{
			// If we are not at the 'top' of the list, then we can actually scroll up
			if (list.First.Value.gameObject.activeSelf == false)
			{
                foreach (GameObject button in list)
				{
					// All buttons are shifted down one position
					Transform oldPosition = button.GetComponent<Transform>();
					oldPosition.position = new Vector3(oldPosition.position.x,
						oldPosition.position.y - this.seperationBetweenButtonsY, oldPosition.position.z);
				}
				ShowLimitedButtons(list, highestButton);
			}
		}
	}

}
