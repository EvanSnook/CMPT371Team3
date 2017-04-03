using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.IO;
using System;
using System.Threading;

using buttons;
using AssemblyCSharp;

using Dicom;
using Dicom.Imaging;
using Dicom.Log;
/// <summary>
/// The FileBrowser1 class represents the script for generating a virtual filebrowser that the user can
/// navigate to seach for image and dicom files for use in the program
/// </summary>
public class FileBrowser1 : MonoBehaviour, IVRButton
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
	LinkedList<VRButton> listOfCurrentDirectoryButtons = new LinkedList<VRButton>();
	// List of all File Buttons within the current directory
	LinkedList<VRButton> listOfCurrentFileButtons = new LinkedList<VRButton>();

	// VRButton prefab to create the Buttons
	[SerializeField]
	private VRButton VRFileButtonPrefab;
	[SerializeField]
	private VRButton VRDirectoryButtonPrefab;
	[SerializeField]
	private VRButton VROtherButtonPrefab;


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

	// VRButton cancel to exit out of the filebrowser
	private VRButton directoryLoadButton;
	// Cancel button Position
	public Vector3 directoryLoadPosition;
	// Cancel rotation
	public Vector3 directoryLoadRotation;

	// VRButtons up for files
	// up file button Position
	public Vector3 upFileButtonPosition;
	// up file rotation
	public Vector3 upFileButtonRotation;

	// down file button Position
	public Vector3 downFileButtonPosition;
	// down file rotation
	public Vector3 downFileButtonRotation;

	// VRButtons up for directories
	private VRButton topDirectoryButton;
	private VRButton bottomDirectoryButton;
	// up directory button Position
	public Vector3 upDirectoryButtonPosition;
	// up directory rotation
	public Vector3 upDirectoryButtonRotation;

	// down directory button Position
	public Vector3 downDirectoryButtonPosition;
	// down directory rotation
	public Vector3 downDirectoryButtonRotation;

	// The button limit on the filebrowser
	[SerializeField]
	private int buttonLimit;

	[SerializeField]
	private GameObject copy3DPrefab;


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
	public LinkedList<VRButton> GetListOfFileButtons()
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
	public LinkedList<VRButton> GetListOfDirectoryButtons()
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
	/// Function CreateFileAndDirectoryButtons() will generate the list of all directory and file buttons and set up for the current
	/// layout of the current file browsing directory
	/// Preconditions: none
	/// Postconditions: creation of all buttons involved
	/// Return: nothing
	/// </summary>
	void CreateFileAndDirectoryButtons()
	{
		// Create a directory button for each directory
		int count = 0;
		foreach (string directory in listOfCurrentDirectories)
		{
			Vector3 newDirectoryPosition = new Vector3(directoryPosition.x, 
				directoryPosition.y - (count * seperationBetweenButtonsY), directoryPosition.z);
			CreateVRButton(directory, "Directory", ButtonType.DIRECTORY_BUTTON, newDirectoryPosition, directoryRotation, this.VRDirectoryButtonPrefab);
			count++;
		}
		// Create a file button for each file
		count = 0;
		foreach (string file in listOfCurrentFiles)
		{
			Vector3 newFilePosition = new Vector3 (filePosition.x, 
				filePosition.y - (count * seperationBetweenButtonsY), filePosition.z);
			CreateVRButton(file, "File", ButtonType.FILE_BUTTON, newFilePosition, fileRotation, this.VRFileButtonPrefab);
			count++;
		}
		// Set the top and bottom directory/file buttons
		if (GetListOfDirectoryButtons().Count > 0)
		{
			//this.topDirectoryButton = GetListOfDirectoryButtons().First.Value;
			//this.bottomDirectoryButton = GetListOfDirectoryButtons().Last.Value;
			ShowLimitedButtons(listOfCurrentDirectoryButtons, this.directoryPosition.y);
		}
		if (GetListOfDirectoryButtons().Count > 0)
		{
			//this.topFileButton = GetListOfFileButtons().First.Value;
			//this.bottomFileButton = GetListOfFileButtons().First.Value;
			ShowLimitedButtons(listOfCurrentFileButtons, this.filePosition.y);
		}
	}


	/// <summary>
	/// Creates all buttons used in the file browser on start.
	/// Pre:: nothing
	/// Post:: creation of all buttons
	/// Return:: nothing
	/// </summary>
	private void CreateAllButtons(){
		CreateFileAndDirectoryButtons();
		CreateVRButton(this.currentDirectory, "Back", ButtonType.BACK_BUTTON, backPosition, backRotation, this.VROtherButtonPrefab);
		CreateVRButton(this.currentDirectory, "Load Directory", ButtonType.LOAD_DIRECTORY_BUTTON, directoryLoadPosition, directoryLoadRotation, this.VROtherButtonPrefab);
		CreateVRButton(this.currentDirectory, "Cancel", ButtonType.CANCEL_BUTTON, cancelPosition, cancelRotation, this.VROtherButtonPrefab);
		CreateVRButton(this.currentDirectory, "File Up", ButtonType.FILE_UP, upFileButtonPosition, upFileButtonRotation, this.VROtherButtonPrefab);
		CreateVRButton(this.currentDirectory, "File Down", ButtonType.FILE_DOWN, downFileButtonPosition, downFileButtonRotation, this.VROtherButtonPrefab);
		CreateVRButton(this.currentDirectory, "Directory Up", ButtonType.DIRECTORY_UP, upDirectoryButtonPosition, upDirectoryButtonRotation, this.VROtherButtonPrefab);
		CreateVRButton(this.currentDirectory, "Directory Down", ButtonType.DIRECTORY_DOWN, downDirectoryButtonPosition, downDirectoryButtonRotation, this.VROtherButtonPrefab);
	}


	/// <summary>
	/// 
	/// </summary>
	/// <param name="args"></param>
	void LoadFiles(string []args)
	{
		dashboard.SendMessage("Minimize");

		StartCoroutine(LoadFilesC(args));
	}

	public IEnumerator LoadFilesC(string[] args)
	{
		var type = args[0];
		var targetPath = args[1];
		var destinationPath = Application.persistentDataPath + @"\.temp_images";

		DeleteAllImagesInPath(destinationPath);

		if ((type.CompareTo ("f") == 0) && (Path.GetExtension (targetPath).CompareTo (".obj") == 0)) {
			ConvertObjFile (targetPath);
			yield return null;
		} else {
			StartConversion (type, targetPath, destinationPath);
		}

		DisableFileBrowser();

	}

	void ConvertObjFile(String targetPath){
		Debug.Log ("Reached converting obj file");
		ObjImporter importer = new ObjImporter ();
		Mesh importedMesh = importer.ImportFile (targetPath);
		GameObject new3DCopy = Instantiate (copy3DPrefab);
		new3DCopy.GetComponent<MeshFilter> ().mesh = importedMesh;
		new3DCopy.GetComponent<MeshCollider> ().sharedMesh = importedMesh;
		Debug.Log ("Instantiated 3d copy.");
	}

	// Change name later?
	IEnumerator StartConversion(String type, String targetPath, String destinationPath){
		yield return StartCoroutine(gameObject.GetComponent<DICOMConverter>().ExternalConverter(type, targetPath, destinationPath));

		//call ConvertAndSerndImages() on everything in (Application.persistentDataPath + \tempImages)
		string[] arrayOfFiles = Directory.GetFiles(destinationPath);

		Debug.Log(targetPath);
		Dictionary<string, string> patientInfo = GetPatientInfo(targetPath);

		foreach (string filePath in arrayOfFiles)
		{
			yield return StartCoroutine(ConvertAndSendImage(filePath, patientInfo));
			yield return null;
		}

		DeleteAllImagesInPath(destinationPath);
	}

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
	/// D
	/// </summary>
	/// <param name="path"></param>
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
	void EnterDirectory(string newDirectory)
	{
		Assert.IsNotNull(newDirectory);
		string storeCurrent = currentDirectory;
		currentDirectory = newDirectory;
		directoryLoadButton.path = this.currentDirectory;
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
			EnterDirectory(storeCurrent);
		}
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



	public byte[] convertToBytes(String file)
	{
		byte[] dicomImage = File.ReadAllBytes(file);
		return dicomImage;
	}


	/// <summary>
	/// Function CreateVRButton will instantiate a new VRButton.  It will create either a File, Directory,
	/// Back, or Cancel button based on the parameters given to it.
	/// Pre:: string or the button's path and name, Vector3 of the buttons position and rotation
	/// </summary>
	/// <param name="buttonPath">string of the path given to the button</param>
	/// <param name="buttonName">string of the new button's name</param>
	/// <param name="buttonType">enum describing the type of button</param> 
	/// <param name="position">Vector3 of the buttons position</param>
	/// <param name="rotation">Vector3 of the buttons Rotation</param>
	private void CreateVRButton(string buttonPath, string buttonName, ButtonType buttonType, Vector3 position, Vector3 rotation, VRButton buttonPrefab)
	{
		// we should contain a prefab and viable string
		Assert.IsNotNull(buttonPrefab);
		// Instantiate a new button and set it as a child of the FileBrowser
		VRButton newButton = Instantiate(buttonPrefab, position,
			Quaternion.Euler(rotation));
		newButton.type = buttonType;
		newButton.transform.parent = gameObject.transform;
		newButton.name = buttonName;
		newButton.manager = this.gameObject;
		newButton.path = buttonPath;
		newButton.textObject = newButton.GetComponentInChildren<TextMesh>();
		// File attributes are set
		if(buttonType == ButtonType.FILE_BUTTON)
		{
			newButton.textObject.text = GetLocalName(buttonPath);
			listOfCurrentFileButtons.AddLast(newButton);
			//listOfCurrentFileButtons.Add(newButton);
		}
		// Directory attributes are set
		else if(buttonType == ButtonType.DIRECTORY_BUTTON)
		{
			newButton.textObject.text = GetLocalName(buttonPath);
			listOfCurrentDirectoryButtons.AddLast(newButton);
			//listOfCurrentDirectoryButtons.Add(newButton);
		}
		// Back button attributes are set
		else if(buttonType == ButtonType.BACK_BUTTON)
		{
			newButton.textObject.text = "Back";
			newButton.path = GetPreviousPath(buttonPath);
			this.backButton = newButton;
		}
		// Cancel button attributes are set
		else if(buttonType == ButtonType.CANCEL_BUTTON)
		{
			this.cancelButton = newButton;
			this.cancelButton.textObject.text = "Cancel";
			this.cancelButton.path = null;
		}
		// Load Directory attributes set
		else if (buttonType == ButtonType.LOAD_DIRECTORY_BUTTON)
		{
			newButton.textObject.text = "Load Directory";
			newButton.path = this.currentDirectory;
			this.directoryLoadButton = newButton;
		}
		// File up attributes set
		else if(buttonType == ButtonType.FILE_UP)
		{
			newButton.textObject.text = "Up";
			newButton.path = null;
			//this.upFileButton = newButton;
		}
		// File down
		else if (buttonType == ButtonType.FILE_DOWN)
		{
			newButton.textObject.text = "Down";
			newButton.path = null;
			//this.downFileButton = newButton;
		}
		// Directory up attributes set
		else if (buttonType == ButtonType.DIRECTORY_UP)
		{
			newButton.textObject.text = "Up";
			newButton.path = null;
			//this.upDirectoryButton = newButton;
		}
		// Directory down attributes set
		else if (buttonType == ButtonType.DIRECTORY_DOWN)
		{
			newButton.textObject.text = "Down";
			newButton.path = null;
			//this.downDirectoryButton = newButton;
		}
		else
		{
			// If the buttonName is not one of the above four possibilities,
			// then something has gone wrong
			Assert.AreEqual<string>("File", buttonName, "The button was given an incorrect name");
		}
	}


	/// <summary>
	/// Function GoBack() will call EnterDirectory on the path above the current path.  This 
	/// will generate all directory and file buttons for that directory.
	/// Pre:: current directory is not null
	/// Post:: current directory is switched to that of the previous directory
	/// Return:: nothing
	/// </summary>
	void GoBack()
	{
		Assert.IsNotNull(currentDirectory);
		EnterDirectory(GetPreviousPath(currentDirectory));
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
		backButton.path = GetPreviousPath(path);
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
		// Get the index in the string where the last '\' is present
		int index = path.LastIndexOf("\\");
		// We can get the previous path if the index is greater than 0
		if(index > 0)
		{
			string newPath = path.Substring(0, index);
			// If we are already at the end of the file we cant get a previous path
			if (newPath == "C::")
			{
				return path;
			}
			else
			{
				return newPath;
			}
		}
		return path;
	}


	/// <summary>
	/// The function ShowLimitedButtons will diplay the file and or directory buttons within the user's view.
	/// Buttons outside of the user's view are hidden.
	/// Pre:: nothing
	/// Post:: sets which buttons are active
	/// Return:: nothing
	/// </summary>
	public void ShowLimitedButtons(LinkedList<VRButton> buttonList, float highestYPosition)
	{
		foreach(VRButton button in buttonList)
		{
			if(button.GetComponent<Transform>().position.y > (highestYPosition+(0.5*this.seperationBetweenButtonsY)))
			{
				button.gameObject.SetActive(false);
			}
			else if(button.GetComponent<Transform>().position.y <= (highestYPosition - (this.buttonLimit*this.seperationBetweenButtonsY)))
			{
				button.gameObject.SetActive(false);
			}
			else
			{
				button.gameObject.SetActive(true);
			}
		}
	}


	/// <summary>
	/// 
	/// </summary>
	/// <param name="list"></param>
	private void ScrollDown(LinkedList<VRButton> list, float highestButton)
	{
		// We should not be able to scroll if the amount of buttons present is less than the limit
		if (list.Count > this.buttonLimit)
		{
			//Transform lastValue = list.Last.Value.GetComponent<Transform>();
			// We can only scroll down if we are not at the bottom of the list
			if (list.Last.Value.gameObject.activeSelf == false)
			{
				foreach (VRButton button in list)
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
	private void ScrollUp(LinkedList<VRButton> list, float highestButton)
	{
		// We should not be able to scroll if the amount of buttons present is less than the limit
		if (list.Count > this.buttonLimit)
		{
			// If we are not at the 'top' of the list, then we can actually scroll up
			if (list.First.Value.gameObject.activeSelf == false)
			{
				foreach (VRButton button in list)
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


	private bool FloatLessThan(float a, float b)
	{
		UnityEngine.Debug.Log("Compare: " + ((double)b - (double)a));
		//double eps = 0.00001;
		return (((double)b - (double)a) < 0.00001d);
	}


	/// <summary>
	/// This function comes from the VRButton interface.  It thakes in a string reptrsenting
	/// the VRButton it recieves.  Based on the name, it will execute the specified function.
	/// Pre:: string button is not null
	/// Post:: execution of the specified function
	/// Return:: nothing.
	/// </summary>
	/// <param name="button">string button is the name of the button clicked</param>
	public void VRButtonClicked(ButtonType button)
	{
		Assert.IsFalse (button == ButtonType.NONE, "ButtonType of None passed into File Browser.");
		switch (button)
		{
		case ButtonType.BACK_BUTTON:
			GoBack();
			break;
		case ButtonType.CANCEL_BUTTON:
			DisableFileBrowser();
			break;
		case ButtonType.FILE_UP:
			ScrollUp(this.listOfCurrentFileButtons, this.filePosition.y);
			break;
		case ButtonType.FILE_DOWN:
			ScrollDown(this.listOfCurrentFileButtons, this.filePosition.y);
			break;
		case ButtonType.DIRECTORY_UP:
			ScrollUp(this.listOfCurrentDirectoryButtons, this.directoryPosition.y);
			break;
		case ButtonType.DIRECTORY_DOWN:
			ScrollDown(this.listOfCurrentDirectoryButtons, this.directoryPosition.y);
			break;


		}
	}
}
