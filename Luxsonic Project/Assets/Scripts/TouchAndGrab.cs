using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchAndGrab : MonoBehaviour
{

    public OVRInput.Controller controller;
    public string button;

    public GameObject _object;
    public float _sphereRadius;
    public LayerMask _someMask;

    private bool _isGrabbed;

    // Update is called once per frame
    // On Update() check to see if the 11th or 12th axis is pressed or not pressed
    // Call function Grab() on pressed
    // Call function Drop() on not pressed
    void Update()
    {

        if (!_isGrabbed && (Input.GetAxis(button) == 1))
            Grab();

        if (_isGrabbed && (Input.GetAxis(button) < 1))
            Drop();
    }



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

            for (int i = 0; i < _objectHits.Length; i++)
            {
                if (_objectHits[i].distance < _objectHits[_closestHit].distance)
                    _closestHit = i;
            }


            _object = _objectHits[_closestHit].transform.gameObject;
            _object.GetComponent<Rigidbody>().isKinematic = true;
            _object.transform.position = transform.position;
            _object.transform.parent = transform;

        }
    }

    void Drop()
    {
        //Change the boolean so the object is not grabbed anymore
        _isGrabbed = false;

        //If the object is attached change the transform to null so it keeps it's position
        if (_object != null)
        {
            _object.transform.parent = null;
            _object.GetComponent<Rigidbody>().isKinematic = false;

            _object = null;
        }


    }
}
