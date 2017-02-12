using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents a thumbnail object that is interactable in VR
/// </summary>
public class Thumbnail : MonoBehaviour {

    public Texture2D image;
    public GameObject manager;

    public void OnCollisionEnter(Collision collision)
    {
        manager.SendMessage("CreateDisplay", image);
    }
}
