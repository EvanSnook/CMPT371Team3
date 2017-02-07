using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomSlider : MonoBehaviour
{

    public float zoomSize;
    public Camera view;

    // Use this for initialization
    void Start()
    {

        zoomSize = view.fieldOfView;

    }

    // Update is called once per frame
    void Update()
    {

        view.fieldOfView = zoomSize;
    }

    void OnGUI()
    {

        zoomSize = GUI.HorizontalSlider(new Rect(25, 75, 100, 30), zoomSize, 1.0F, 100.0F);
    }
}
