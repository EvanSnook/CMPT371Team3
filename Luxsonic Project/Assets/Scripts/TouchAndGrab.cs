using System.Collections;
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

    // the radius of the sphere for the raycast
    // Set in editor; below value is a default only
    [SerializeField]
    private float _sphereRadius = 0.1f;

	// GameObject for the user's other hand
	[SerializeField]
	private GameObject oppositeHandObject = null;

	// the object we are touching/grabbing
	public GameObject _grabbedObject = null;

	// This variable will be updated
	public LayerMask _someMask;

	// Indicates whether an object is being grabbed or not
	private bool _isHolding = false;

    // Reference to the user's other hand
    private TouchAndGrab oppositeHand = null;

    // whether buttons can be pressed again or not
    private bool onCooldown = false;

	[SerializeField]
	private float cooldownDuration = 0.5f;

    // previous parent of grabbed object
	private Transform objectsOldParent = null;


    /// <summary>
    /// 
    /// </summary>
	void Start()
	{
		oppositeHand = oppositeHandObject.GetComponent<TouchAndGrab>();
	}


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
	public bool IsHolding()
	{
		return _isHolding;
	}


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
	public GameObject GetHeldObject()
	{
		return _grabbedObject;
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
			if (oppositeHand.IsHolding())
			{
				if (oppositeHand.GetHeldObject() != _objectHits[_closestHit].transform.gameObject)
				{
					AssignGrabbedObject(_objectHits[_closestHit].transform.gameObject);
				}
			}
			else
			{
				AssignGrabbedObject(_objectHits[_closestHit].transform.gameObject);
			}
		}
	}


    /// <summary>
    /// 
    /// </summary>
    /// <param name="closest"></param>
	void AssignGrabbedObject(GameObject closest)
	{
		_isHolding = true;
		_grabbedObject = closest;
		_grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
		this.objectsOldParent = _grabbedObject.transform.parent;
		_grabbedObject.transform.position = transform.position;
		_grabbedObject.transform.parent = transform;
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
		if (_grabbedObject != null)
		{
			_grabbedObject.transform.parent = this.objectsOldParent;
			_grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
			_grabbedObject = null;
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
		if (((int)Input.GetAxis(grabTrigger) == 1) && (Input.GetAxis(indexTrigger) < 1))
		{
			if ((other.tag == "MenuButton"))
			{
                if (!onCooldown)
                {
                    other.gameObject.GetComponent<VRButton>().ButtonClicked(true);
				}
			}
			else if ((other.tag == "Copy"))
			{
				if (!(other.gameObject.GetComponent<Copy>().GetPressed()))
				{
					other.gameObject.GetComponent<Copy>().SetPressed(true);
				}
			}
			else if ((other.tag == "Thumbnail"))
			{
				if (!(other.gameObject.GetComponent<Thumbnail>().GetPressed()))
				{
					other.gameObject.GetComponent<Thumbnail>().SetPressed(true);
				}
			}
		}
	}


    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
	private void OnTriggerStay(Collider other)
	{
		if ((!_isHolding) && (Input.GetAxis(grabTrigger) == 1) && (Input.GetAxis(indexTrigger) == 1))
		{
			Grab();
		}
		else if ((_isHolding) && ((Input.GetAxis(grabTrigger) < 1) || (Input.GetAxis(indexTrigger) < 1)))
		{
			Drop();
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
            other.gameObject.GetComponent<VRButton>().ButtonClicked(false);
		}
		else if (other.tag == "Thumbnail")
		{
			other.gameObject.GetComponent<Thumbnail>().SetPressed(false);
		}
		else if (other.tag == "Copy")
		{
			other.gameObject.GetComponent<Copy>().SetPressed(false);
		}
	}


    private void ButtonPressCooldown()
    {
        onCooldown = true;
        Invoke("ResetButtonPressCooldown", cooldownDuration);
    }

    private void ResetButtonPressCooldown()
    {
        onCooldown = false;
    }
}