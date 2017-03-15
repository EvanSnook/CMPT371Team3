using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TouchAndGrab : MonoBehaviour
{
    // the OVR controller
    public OVRInput.Controller controller;
    // name of the axis for the grab trigger
    [SerializeField]
    private string grabTrigger;
    // name of the axis for the index trigger
    [SerializeField]
    private string indexTrigger;
    // the object we are touching/grabbing
    public GameObject _object;
    // the radius of the sphere for the raycast
    public float _sphereRadius;
    // This variable will be updated
    public LayerMask _someMask;
    // Indicates whether an object is being grabbed or not
    private bool _isGrabbed;

    private Transform objectsOldParent;

    /// <summary>
    /// Update is called once per frame
    /// On Update() check to see if the 11th or 12th axis is pressed or not pressed
    /// Call function Grab() on pressed
    /// Call function Drop() on not pressed
    /// Pre:: 
    /// Post:: 
    /// Return:: 
    /// </summary>
    void Update()
    {
        // If the object is not being grabbed, grab the object, else drop the object
        if (!_isGrabbed && (Input.GetAxis(grabTrigger) == 1))
            Grab();
        if (_isGrabbed && (Input.GetAxis(grabTrigger) < 1))
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
        // Check to see if the object has been grabbed
        _isGrabbed = true;
        // Create an array for the amount of objects grabbed
        RaycastHit[] _objectHits;
        // Store the amount of objects that are gathered from the Sphere Cast
        _objectHits = Physics.SphereCastAll(transform.position, _sphereRadius, transform.forward, 0F, _someMask);
        // As long as there is an object that is grabbed track the position of the object.
        if (_objectHits.Length > 0)
        {
            int _closestHit = 0;
            // Get the object that is closest to the center of the raycast
            for (int i = 0; i < _objectHits.Length; i++)
            {
                if (_objectHits[i].distance < _objectHits[_closestHit].distance)
                    _closestHit = i;
            }
            // make the object a child of the controller, updating the position of the object
            _object = _objectHits[_closestHit].transform.gameObject;
            _object.GetComponent<Rigidbody>().isKinematic = true;
            this.objectsOldParent = _object.transform.parent;
            _object.transform.position = transform.position;
            _object.transform.parent = transform;
        }
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
        _isGrabbed = false;
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
        if ((other.tag == "MenuButton") && (Input.GetAxis(grabTrigger) == 1) && (Input.GetAxis(indexTrigger) < 1))
        {
            if (!(other.gameObject.GetComponent<VRButton>().GetPressed()))
            {
                other.gameObject.GetComponent<VRButton>().SetPressed(true);
            }
        }
        else if ((other.tag == "Copy") && (Input.GetAxis(grabTrigger) == 1) && (Input.GetAxis(indexTrigger) < 1))
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