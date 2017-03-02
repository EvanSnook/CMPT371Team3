using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderKnob : MonoBehaviour {

    private Vector3 leftBoundary;
    private Vector3 rightBoundary;

    public GameObject knob;
    public GameObject sliderBase;

    public void SetLeftBoundary(Vector3 bound)
    {
        this.leftBoundary = bound;
       
    }

    public void SetRightBoundary(Vector3 bound)
    {
        this.rightBoundary = bound;
    }

    public Vector3 getLeftBoundary()
    {
        return this.leftBoundary;
    }

    public Vector3 getRightBoundary()
    {
        return this.rightBoundary;
    }

    void OnMouseDrag()
    {
        if (Input.mousePosition.x <= this.rightBoundary.x && Input.mousePosition.x >= this.leftBoundary.x)
        {
            knob.transform.position = new Vector3(Input.mousePosition.x, knob.transform.position.y, knob.transform.position.z);
        }
        sliderBase.GetComponent<SliderBar>().setUpdate(true);
    }

    private void OnMouseUp()
    {
        sliderBase.GetComponent<SliderBar>().setUpdate(false);
    }

}
