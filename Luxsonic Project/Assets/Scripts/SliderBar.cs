using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderBar : MonoBehaviour {

	// The object prefab for the slider base and knob
    public GameObject sliderBasePrefab;
    public GameObject sliderKnobPrefab;
	// The object that the slider will be reporting too
    public GameObject manager;

	// The maximum coordinate of the slider in world space
    Vector3 maxCoord;
	// The minimum coordinate of the slider in world space
    Vector3 minCoord;
	// The coordinate of the knob in world space
    Vector3 knobCoord;

	// The maxCoord normalized (0 to maxScale)
    private float maxScale;
	// The knobCoord normalized (0 - knobScaleValue - maxScale)
	private float knobScaleValue;

	// The max represented as a percent
    float maxPercentScale = 1;
	// the min represented as a percent
	float minPercentScale = 0;

	// Indicates 
    private bool update = false;

    /// <summary>
    /// Called by the manager to set up the slider
	/// Pre:: currentVal is passed in from the Copy
	/// Post:: A Slider is created for the Copy
	/// Return:: nothing
    /// </summary>
    /// <param name="currentVal">Current Value of the image we are updating</param>
    public void Setup(float currentVal)
    {
		// Getting bounds of the slider in the world space
		Renderer rend = sliderBasePrefab.GetComponent<Renderer>();
		maxCoord = rend.bounds.max;
		minCoord = rend.bounds.min;
		Debug.Log ("max position: " + maxCoord);
		Debug.Log ("min position: " + minCoord);

		// Converting world space max to scaled max
		maxScale = maxCoord.x - minCoord.x;

		// Converting the value retrieved from image
		knobScaleValue = ConvertFromPercentOfSliderToScale(currentVal);

		// Moving slider knob to position of image value
		sliderKnobPrefab.transform.position = new Vector3(ConvertFromScaleToCoord(ConvertFromPercentOfSliderToScale(currentVal)),
			sliderKnobPrefab.transform.position.y, sliderKnobPrefab.transform.position.z);

		// Updating the current position to the value of the slider knob
        knobCoord = sliderKnobPrefab.transform.position;
		Debug.Log ("knob: " + knobCoord);

		// Setting the boundaries of the knob to the slider boundaries
		sliderKnobPrefab.GetComponent<SliderKnob>().SetLeftBoundary(this.minCoord);
		sliderKnobPrefab.GetComponent<SliderKnob>().SetRightBoundary(this.maxCoord);
    }

	/// <summary>
	/// Raises the collision enter event.
	/// </summary>
	/// <param name="collision">Collision.</param>
    void OnCollisionEnter(Collision collision)
    {
        this.update = true;
    }

	/// <summary>
	/// Raises the collision exit event.
	/// </summary>
	/// <param name="collision">Collision.</param>
    private void OnCollisionExit(Collision collision)
    {
        this.update = false;
    }

	/// <summary>
	/// Converts the coordinate to the scale of the slider by taking
	/// the value passed in a subtracting the minimum coordinate
	/// </summary>
	/// <returns>The scaled version of the coordinate.</returns>
	/// <param name="value">Value.</param>
    float ConvertFromCoordToScale(float value)
    {
        return value - minCoord.x;
    }

	/// <summary>
	/// Convert the scale of the slider to the coordinate by taking
	/// the value and adding the minimum coordinate
	/// </summary>
	/// <returns>The coordinate of the scale.</returns>
	/// <param name="value">Value.</param>
    float ConvertFromScaleToCoord(float value)
    {
        return value + minCoord.x;
    }

	/// <summary>
	/// Convert coordinate to percent scale by taking the value and 
	/// subtracting the minimum coordinate and then dividing by the 
	/// max scale number to get the percent
	/// </summary>
	/// <returns>The percent of slider.</returns>
	/// <param name="value">Value.</param>
	float ConvertFromCoordToPercentOfSlider(float value)
	{
		return (value - minCoord.x) / maxScale;
	}

	/// <summary>
	/// Convert the percent of slider to the coordinate by taking the value
	/// converting it to the scale, and then converting it to the coord
	/// </summary>
	/// <returns>The coordinate of the value.</returns>
	/// <param name="value">Value.</param>
	float ConvertFromPercentOfSliderToCoord(float value)
	{
		return ConvertFromScaleToCoord (ConvertFromPercentOfSliderToScale (value));
	}

	/// <summary>
	/// Converts percent scale to scale by taking the value
	/// and multiplying it by the maximum scale
	/// </summary>
	/// <returns>The scaled value.</returns>
	/// <param name="value">Value.</param>
	float ConvertFromPercentOfSliderToScale(float value)
	{
		return value * maxScale;
	}

	/// <summary>
	/// Converts the scale to the percent of slider by taking the value
	/// and dividing it by the maximum scale
	/// </summary>
	/// <returns>The percent of slider.</returns>
	/// <param name="value">Value.</param>
	float ConvertFromScaleToPercentOfSlider(float value)
	{
		return value / maxScale;
	}

	/// <summary>
	/// Sets the update.
	/// </summary>
	/// <param name="update">If set to <c>true</c> update.</param>
    public void setUpdate(bool update)
    {
        this.update = update;
    }
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update ()
    {
		// if update is true, then update the slider and slider knob
        if (update)
        {
			knobCoord = sliderKnobPrefab.transform.position;
			Debug.Log ("Converted: " + ConvertFromCoordToPercentOfSlider (knobCoord.x));
			manager.SendMessage("SliderUpdate", ConvertFromCoordToPercentOfSlider(knobCoord.x));
        }
	}
}
