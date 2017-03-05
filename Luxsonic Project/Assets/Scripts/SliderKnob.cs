﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderKnob : MonoBehaviour {

    private Vector3 leftBoundary;
    private Vector3 rightBoundary;

	private float yCoord;
	private float zCoord;

    public GameObject knobPrefab;
    public GameObject sliderBasePrefab;

	void Start() {
		yCoord = this.transform.position.y;
		zCoord = this.transform.position.z;
	}

    /// <summary>
    /// Set the left boundary for the slider knob's position
    /// </summary>
    /// <param name="bound">The posiiton of the boundary</param>
    public void SetLeftBoundary(Vector3 bound)
    {
        this.leftBoundary = bound;
       
    }

    /// <summary>
    /// Set the right boundary for the slider knob's position
    /// </summary>
    /// <param name="bound">The position of the boundary</param>
    public void SetRightBoundary(Vector3 bound)
    {
        this.rightBoundary = bound;
    }

	/// <summary>
	/// Gets the left boundary.
	/// </summary>
	/// <returns>The left boundary.</returns>
    public Vector3 GetLeftBoundary()
    {
        return this.leftBoundary;
    }

	/// <summary>
	/// Gets the right boundary.
	/// </summary>
	/// <returns>The right boundary.</returns>
    public Vector3 GetRightBoundary()
    {
        return this.rightBoundary;
    }

    
	public void FixedUpdate() {
		knobPrefab.transform.position = new Vector3 (Mathf.Clamp(this.transform.position.x, leftBoundary.x, rightBoundary.x), yCoord, zCoord);
		sliderBasePrefab.GetComponent<SliderBar>().setUpdate(true);
	}

    void OnMouseDrag()
    {
		knobPrefab.transform.position = new Vector3(Mathf.Clamp(Input.mousePosition.x, leftBoundary.x, rightBoundary.x), knobPrefab.transform.position.y, knobPrefab.transform.position.z);
        sliderBasePrefab.GetComponent<SliderBar>().setUpdate(true);
    }

    private void OnMouseUp()
    {
        sliderBasePrefab.GetComponent<SliderBar>().setUpdate(false);
    }

   

}
