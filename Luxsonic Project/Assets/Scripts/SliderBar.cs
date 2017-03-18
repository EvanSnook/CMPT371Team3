using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
public class SliderBar : MonoBehaviour
{
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
//    // The max represented as a percent
//    float maxPercentScale = 1;
//    // the min represented as a percent
//    float minPercentScale = 0;
    // Indicates if the slider needs to be updated
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
        // Converting world space max to scaled max
        maxScale = maxCoord.x - minCoord.x;
        // Converting the value retrieved from image
        knobScaleValue = ConvertFromPercentOfSliderToScale(currentVal);
        // Moving slider knob to position of image value
        sliderKnobPrefab.transform.position = new Vector3(ConvertFromScaleToCoord(ConvertFromPercentOfSliderToScale(currentVal)),
            sliderKnobPrefab.transform.position.y, sliderKnobPrefab.transform.position.z);
        // Updating the current position to the value of the slider knob
        knobCoord = sliderKnobPrefab.transform.position;

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
    public float ConvertFromCoordToScale(float value)
    {
        Assert.IsTrue(value >= this.minCoord.x && value <= this.maxCoord.x, "The coordinate given was outside of the range of the scale");
        return value - minCoord.x;
    }
    /// <summary>
    /// Convert the scale of the slider to the coordinate by taking
    /// the value and adding the minimum coordinate
    /// </summary>
    /// <returns>The coordinate of the scale.</returns>
    /// <param name="value">Value.</param>
    public float ConvertFromScaleToCoord(float value)
    {
        Assert.IsTrue(value <= this.maxScale && value >= 0, "The value given is not within the scale range");
        return value + minCoord.x;
    }
    /// <summary>
    /// Convert coordinate to percent scale by taking the value and 
    /// subtracting the minimum coordinate and then dividing by the 
    /// max scale number to get the percent
    /// </summary>
    /// <returns>The percent of slider.</returns>
    /// <param name="value">Value.</param>
    public float ConvertFromCoordToPercentOfSlider(float value)
    {
        Assert.IsTrue(value >= this.minCoord.x && value <= this.maxCoord.x, "The coordinate given was outside of the range of the scale");
        return (value - minCoord.x) / maxScale;
    }
    /// <summary>
    /// Convert the percent of slider to the coordinate by taking the value
    /// converting it to the scale, and then converting it to the coord
    /// </summary>
    /// <returns>The coordinate of the value.</returns>
    /// <param name="value">Value.</param>
    public float ConvertFromPercentOfSliderToCoord(float value)
    {
        Assert.IsTrue(value <= 1 && value >= 0, "The value is not a percentage of the slider.");
        return ConvertFromScaleToCoord(ConvertFromPercentOfSliderToScale(value));
    }
    /// <summary>
    /// Converts percent scale to scale by taking the value
    /// and multiplying it by the maximum scale
    /// </summary>
    /// <returns>The scaled value.</returns>
    /// <param name="value">Value.</param>
    public float ConvertFromPercentOfSliderToScale(float value)
    {
        Assert.IsTrue(value <= 1 && value >= 0, "The value is not a percentage of the slider.");
        return value * maxScale;
    }
    /// <summary>
    /// Converts the scale to the percent of slider by taking the value
    /// and dividing it by the maximum scale
    /// </summary>
    /// <returns>The percent of slider.</returns>
    /// <param name="value">Value.</param>
    public float ConvertFromScaleToPercentOfSlider(float value)
    {
        Assert.IsTrue(value >= 0 && value < this.maxScale);
        return value / maxScale;
    }
    /// <summary>
    /// Used to set update to true when the knob is moved.
    /// </summary>
    /// <param name="update">A boolean value representing if the slider should be updated</param>
    public void setUpdate(bool update)
    {
        this.update = update;
    }

    /// <summary>
    /// Update this instance.
    /// </summary>
    void Update()
    {
        // if update is true, then update the slider and slider knob
        if (update)
        {
            knobCoord = sliderKnobPrefab.transform.position;
            if (manager != null)
            {
                //              Debug.Log ("UPDATE");
                manager.SendMessage("SliderUpdate", ConvertFromCoordToPercentOfSlider(knobCoord.x));
            }
        }
    }
    /// <summary>
    /// Get the maximum value on the scale of the slider
    /// </summary>
    /// <returns>The maximum value on the scale of the slider</returns>
    public float GetMaxScale()
    {
        return this.maxScale;
    }
    /// <summary>
    /// Set the maximum value on the scale of the slider
    /// </summary>
    public void SetMaxScale()
    {
        this.maxScale = this.maxCoord.x - this.minCoord.x;
    }
    /// <summary>
    /// Get the knob position of the slider converted to the scale of the slider
    /// </summary>
    /// <returns>The knob position of the slider converted to the scale of the slider</returns>
    public float GetKnobScaleValue()
    {
        return this.knobScaleValue;
    }
    /// <summary>
    /// Get the max position of the slider
    /// </summary>
    /// <returns>The max position of the slider</returns>
    public Vector3 GetMaxCoord()
    {
        return this.maxCoord;
    }
    /// <summary>
    /// Fet the min position of the slider
    /// </summary>
    /// <returns>The min position of the slider</returns>
    public Vector3 GetMinCoord()
    {
        return this.minCoord;
    }
    /// <summary>
    /// Set the max position of the slider
    /// </summary>
    public void SetMaxCoord(Vector3 coord)
    {
        this.maxCoord = coord;
    }
    /// <summary>
    /// Set the min position of the slider
    /// </summary>
    public void SetMinCoord(Vector3 coord)
    {
        this.minCoord = coord;
    }
    /// <summary>
    /// Get the position of the knob
    /// </summary>
    /// <returns>The position of the knob</returns>
    public Vector3 GetKnobCoord()
    {
        return this.knobCoord;
    }
}