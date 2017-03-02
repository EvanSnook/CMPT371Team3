using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderBar : MonoBehaviour {

    public GameObject sliderBase;
    public GameObject sliderKnob;
    public GameObject manager;

    Vector3 maxPosition;
    Vector3 minPosition;
    Vector3 currentPosition;

    private float convertedMaxPosition;
    private float convertedCurrentPosition;

    float maxVal = 1;
    float minVal = 0;

    private bool update = false;

	// Use this for initialization
	void Start () {
        Renderer rend = sliderBase.GetComponent<Renderer>();
        maxPosition = rend.bounds.max;
        minPosition = rend.bounds.min;

        convertedMaxPosition = maxPosition.x - minPosition.x;

        Setup(sliderKnob.transform.position.x);
	}

    /// <summary>
    /// Called by the manager
    /// </summary>
    /// <param name="currentVal"></param>
    public void Setup(float currentVal)
    {
        convertedCurrentPosition = currentVal;
        sliderKnob.transform.position.Set(convertFromScale(currentVal), sliderKnob.transform.position.y, sliderKnob.transform.position.z);
        currentPosition = sliderKnob.transform.position;
        sliderKnob.GetComponent<SliderKnob>().SetLeftBoundary(this.minPosition);
        sliderKnob.GetComponent<SliderKnob>().SetRightBoundary(this.maxPosition);
    }

    void OnCollisionEnter(Collision collision)
    {
        this.update = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        this.update = false;
    }

    float convertToScale(float point)
    {
        return currentPosition.x - minPosition.x / convertedMaxPosition;
    }

    float convertFromScale(float value)
    {
        return value * convertedMaxPosition + minPosition.x;
    }

    public void setUpdate(bool update)
    {
        this.update = update;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (update)
        {
            Debug.Log("Updating");
            currentPosition = sliderKnob.transform.position;
            manager.SendMessage("SliderUpdate", convertToScale(currentPosition.x));
        }
	}
}
