using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {

    public string name;
    public GameObject manager;
    public TextMesh textObject;

    public void OnCollisionEnter(Collision collision)
    {
        manager.SendMessage("buttonClicked", name);
        textObject.color = Color.black;
    }
    // Use this for initialization
    void Start () {
        textObject = gameObject.GetComponentInChildren<TextMesh>();
        textObject.text = name;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
