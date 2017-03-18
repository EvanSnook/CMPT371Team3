﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TouchAndGrab : MonoBehaviour
{
    // the OVR controller
    public OVRInput.Controller controller;

    // name of the axis for the grab trigger
	// Set in editor; below value is a default only
    [SerializeField]
    private string grabTrigger = "";
    // name of the axis for the index trigger
	// Set in editor; below value is a default only
    [SerializeField]
    private string indexTrigger = "";
    


	// Reference to the user's other hand
	private TouchAndGrab oppositeHand = null;

	// GameObject for the user's other hand
	[SerializeField]
	private GameObject oppositeHandObject = null;

	// the object we are touching/grabbing
    public GameObject _object = null;

    // the radius of the sphere for the raycast
	// Set in editor; below value is a default only
	[SerializeField]
    private float _sphereRadius = 0.1f;

    // This variable will be updated
	//**UPDATED TO WHAT?**
	public LayerMask _someMask = (LayerMask)null;
    // Indicates whether an object is being grabbed or not
	private bool _isHolding = false;

    private Transform objectsOldParent = null;

	void Start(){
		oppositeHand = oppositeHandObject.GetComponent<TouchAndGrab> ();
	}

	public bool IsHolding(){
		return _isHolding;
	}

	public GameObject GetHeldObject(){
		return _object;
	}

    /// <summary>
    /// Update is called once per frame
    /// On Update() check to see if the 11th or 12th axis is pressed or not pressed
    /// Call function Grab() on pressed
    /// Call function Drop() on not pressed and object held
    /// Pre:: 
    /// Post:: 
    /// Return:: 
    /// </summary>
    void Update()
    {
        // If we are not holding something and the grab trigger is pressed, grab
		if (!_isHolding && ((int)Input.GetAxis(grabTrigger) == 1))
            Grab();
		// If we are holding something and release the trigger, drop it
		if (_isHolding && (Input.GetAxis(grabTrigger) < 1))
            Drop();
    }

    /// <summary>
    /// Make the object being grabbed a child of the controller (hand) whenever it is within the raycast
    /// of the controller
    /// Pre:: nothing
    /// Post:: The object is now a child of the controller and moves to the same position
    /// of the controller
    /// Return:: nothing
    /// </summary>
    void Grab()
    {
        // Create an array for the amount of objects grabbed
        RaycastHit[] _objectHits;
        
		// Store the amount of objects that are gathered from the Sphere Cast
        _objectHits = Physics.SphereCastAll(transform.position, _sphereRadius, transform.forward, 0F, _someMask);
        
		// Find the closes object from the SphereCast hits, if any exist
        if (_objectHits.Length > 0)
        {
            int _closestHit = 0;
            // Get the object that is closest to the center of the raycast
            for (int i = 0; i < _objectHits.Length; i++)
            {
                if (_objectHits[i].distance < _objectHits[_closestHit].distance)
                    _closestHit = i;
            }

			// Check to see if the other hand is holding something
			// If it is, and it's the same as the closest raycast object, don't grab
			if (oppositeHand.IsHolding ()) {
				if (oppositeHand.GetHeldObject () != _objectHits [_closestHit]) {
					AssignGrabbedObject (_objectHits [_closestHit].transform.gameObject);
				}
			} else {
				AssignGrabbedObject (_objectHits[_closestHit].transform.gameObject);
			}
        }
    }

	void AssignGrabbedObject(GameObject closest){
		_isHolding = true;
		_object = closest;
		_object.GetComponent<Rigidbody>().isKinematic = true;
		this.objectsOldParent = _object.transform.parent;
		_object.transform.position = transform.position;
		_object.transform.parent = transform;
	}

    /// <summary>
    /// Remove the object as a child of the controller 
    /// Pre:: an object is being grabbed
    /// Post:: The object is no longer a child of the controller
    /// Return:: nothing
    /// </summary>
    void Drop()
    {
        //Change the boolean so the object is not grabbed anymore
		_isHolding = false;

        //If the object is attached change the transform to null so it keeps it's position
        if (_object != null)
        {
            _object.transform.parent = this.objectsOldParent;
            _object.GetComponent<Rigidbody>().isKinematic = false;
            _object = null;
        }
    }
    /// <summary>
    /// OnTriggerEnter is called once an object enters the collision box
    /// If OnTriggerExit() on a menu button and corresponding input is 
    /// detected, set button's pressed flag to true
    /// Pre:: 
    /// Post:: button's pressed flag is set to true
    /// Return:: 
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
		if ((other.tag == "MenuButton") && ((int)Input.GetAxis(grabTrigger) == 1) && (Input.GetAxis(indexTrigger) < 1))
        {
            if (!(other.gameObject.GetComponent<VRButton>().GetPressed()))
            {
                other.gameObject.GetComponent<VRButton>().SetPressed(true);
            }
        }
		else if ((other.tag == "Copy") && ((int)Input.GetAxis(grabTrigger) == 1) && (Input.GetAxis(indexTrigger) < 1))
        {
            other.gameObject.SendMessage("Selected");
        }
        else if (other.tag == "Thumbnail")
        {
            other.gameObject.SendMessage("Selected");
        }
    }
    /// <summary>
    /// OnTriggerExit is called once an object exits the collision box
    /// If OnTriggerExit() on a menu button, reset button's pressed flag
    /// Pre:: 
    /// Post:: button's pressed flag is set to false
    /// Return:: 
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "MenuButton")
        {
            other.gameObject.GetComponent<VRButton>().SetPressed(false);
        }
    }
}