using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectoryButton : MonoBehaviour {

    GameObject parent;
    string currentDirectory;
    string[] listOfCurrentDirectories;
    string[] listOfCurrentFiles;

    DirectoryButton(string path)
    {
        currentDirectory = path;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision hit)
    {
        parent.SendMessage("EnterDirectory", currentDirectory);
    }
}
