using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Security.Permissions;
using UnityEngine;
using UnityEditor;
using UnityEngine.Assertions;

public class SubmitButton : MonoBehaviour {

    FileInfo file;
    GameObject parent;
    

	// Use this for initialization
	void Start () {

        file = null;
        parent = GameObject.FindGameObjectWithTag("LoadBar");
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision hit)
    {
        parent.SendMessage("ConvertAndSendImage", file);
    }
}
