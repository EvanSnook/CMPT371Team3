using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Security.Permissions;
using UnityEngine;
using UnityEditor;

public class LoadBar : MonoBehaviour
{
    //Is the file browser displayed?
    bool loading;

    //skins and textures
    public GUISkin[] skins;
    public Texture2D file, folder, back, drive;

    string[] layoutTypes = { "Type 0", "Type 1" };
    //initialize file browser
    FileBrowser fb = new FileBrowser();
    string output = "no file";
    // Use this for initialization
    void Start()
    {
        loading = true;
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

    void OnGUI() {
        GUILayout.BeginHorizontal();
		GUILayout.BeginVertical();
		GUILayout.Label("Layout Type");
		fb.setLayout(GUILayout.SelectionGrid(fb.layoutType,layoutTypes,1));
		GUILayout.Space(10);
		//select from available gui skins
		GUILayout.Label("GUISkin");
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
            //If the user selects an image, then the destination is saved and sent
            if (fb.outputFile == null)
            {
                output = "Cancel Hit";
                this.enabled = false;
            }
            else
            {
                fb.outputFile.ToString();

            }
		}
	}

}