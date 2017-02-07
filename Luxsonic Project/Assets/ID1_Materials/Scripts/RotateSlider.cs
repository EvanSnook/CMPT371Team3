using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSlider : MonoBehaviour
{


    public float x_value = 0.0F;
    public float y_value = 0.0F;
    public float z_value = 0.0F;
    public GameObject image;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        image.transform.localRotation = Quaternion.Euler(x_value, y_value, z_value);

    }

    void OnGUI()
    {
        x_value = GUI.HorizontalSlider(new Rect(25, 150, 100, 30), x_value, 0.0F, 360.0F);
        y_value = GUI.HorizontalSlider(new Rect(25, 200, 100, 30), y_value, 0.0F, 360.0F);
        z_value = GUI.HorizontalSlider(new Rect(25, 250, 100, 30), z_value, 0.0F, 360.0F);
        GUILayout.Label("X Value " + x_value + "Y value " + y_value + "Z Value" + z_value);
    }

}

