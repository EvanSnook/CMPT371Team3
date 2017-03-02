using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderBar : MonoBehaviour {

    public GameObject sliderBase;
    public GameObject cube;
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

	}

    /// <summary>
    /// Called by the manager
    /// </summary>
    /// <param name="currentVal"></param>
    void Setup(float currentVal)
    {
        convertedCurrentPosition = currentVal;
        cube.transform.position.Set(convertFromScale(currentVal), cube.transform.position.y, cube.transform.position.z);
        currentPosition = cube.transform.position;
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
	
	// Update is called once per frame
	void Update ()
    {
        if (update)
        {
            currentPosition = cube.transform.position;
            manager.SendMessage("SliderUpdate", convertToScale(currentPosition.x));
        }
	}
}
