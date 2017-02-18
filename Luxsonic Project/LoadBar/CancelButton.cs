using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelButton : MonoBehaviour {

    GameObject parent;

    // Use this for initialization
    void Start () {
        parent = GameObject.FindGameObjectWithTag("LoadBar");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision hit)
    {
        parent.SendMessage("DisableLoadBar");
    }
}
