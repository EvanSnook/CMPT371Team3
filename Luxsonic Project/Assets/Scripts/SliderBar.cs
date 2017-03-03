using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderBar : MonoBehaviour {

    public GameObject sliderBase;
    public GameObject sliderKnob;
    public GameObject manager;

    Vector3 maxPosition;
    Vector3 minPosition;
    Vector3 knobPosition;

    private float convertedMaxPosition;
	private float convertedKnobPosition;

    float maxVal = 1;
    float minVal = 0;

    private bool update = false;

	// Use this for initialization
	void Start () {
//        Renderer rend = sliderBase.GetComponent<Renderer>();
//        maxPosition = rend.bounds.max;
//        minPosition = rend.bounds.min;
//		Debug.Log ("minPos: " + minPosition);
//
//        convertedMaxPosition = maxPosition.x - minPosition.x;

//        Setup(sliderKnob.transform.position.x);
	}

    /// <summary>
    /// Called by the manager to set up the slider
    /// </summary>
    /// <param name="currentVal">Current Value of the image we are updating</param>
    public void Setup(float currentVal)
    {
		// Getting bounds of the slider in the world space
		Renderer rend = sliderBase.GetComponent<Renderer>();
		maxPosition = rend.bounds.max;
		minPosition = rend.bounds.min;
		Debug.Log ("max position: " + maxPosition);
		Debug.Log ("min position: " + minPosition);

		// Converting world space max to scaled max
		convertedMaxPosition = maxPosition.x - minPosition.x;

		// Converting the value retrieved from image
		convertedKnobPosition = ConvertFromPercentOfSliderToScale(currentVal);

		// Moving slider knob to position of image value
		sliderKnob.transform.position = new Vector3(ConvertFromScaleToCoord(ConvertFromPercentOfSliderToScale(currentVal)), sliderKnob.transform.position.y, sliderKnob.transform.position.z);

		// Updating the current position to the value of the slider knob
        knobPosition = sliderKnob.transform.position;
		Debug.Log ("knob: " + knobPosition);

		// Setting the boundaries of the knob to the slider boundaries
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

	/// <summary>
	/// Converts the coordinate to the scale of the slider
	/// </summary>
	/// <returns>The scaled version of the coordinate.</returns>
	/// <param name="value">Value.</param>
    float ConvertFromCoordToScale(float value)
    {
        return value - minPosition.x;
    }

	/// <summary>
	/// Convert the scale of the slider to the coordinate
	/// </summary>
	/// <returns>The coordinate of the scale.</returns>
	/// <param name="value">Value.</param>
    float ConvertFromScaleToCoord(float value)
    {
        return value + minPosition.x;
    }

	/// <summary>
	/// Convert coordinate to percent scale
	/// </summary>
	/// <returns>The percent of slider.</returns>
	/// <param name="value">Value.</param>
	float ConvertFromCoordToPercentOfSlider(float value)
	{
		return (value - minPosition.x) / convertedMaxPosition;
	}

	/// <summary>
	/// Convert the percent of slider to the coordinate
	/// </summary>
	/// <returns>The coordinate of the value.</returns>
	/// <param name="value">Value.</param>
	float ConvertFromPercentOfSliderToCoord(float value)
	{
		return ConvertFromScaleToCoord (ConvertFromPercentOfSliderToScale (value));
	}

	/// <summary>
	/// Converts percent scale to scale
	/// </summary>
	/// <returns>The scaled value.</returns>
	/// <param name="value">Value.</param>
	float ConvertFromPercentOfSliderToScale(float value)
	{
		return value * convertedMaxPosition;
	}

	/// <summary>
	/// Converts the scale to the percent of slider
	/// </summary>
	/// <returns>The percent of slider.</returns>
	/// <param name="value">Value.</param>
	float ConvertFromScaleToPercentOfSlider(float value)
	{
		return value / convertedMaxPosition;
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
//            Debug.Log("Updating");
            knobPosition = sliderKnob.transform.position;
			Debug.Log ("Converted: " + ConvertFromCoordToPercentOfSlider (knobPosition.x));
			manager.SendMessage("SliderUpdate", ConvertFromCoordToPercentOfSlider(knobPosition.x));
        }
	}
}
