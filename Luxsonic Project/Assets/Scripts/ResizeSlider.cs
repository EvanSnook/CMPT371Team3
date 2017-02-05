using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeSlider : MonoBehaviour
{

    public float scaleSize = 1.0F;
    public GameObject image;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        image.transform.localScale = new Vector3(scaleSize, scaleSize, scaleSize);
    }

    void OnGUI()
    {
        scaleSize = GUI.HorizontalSlider(new Rect(25, 25, 100, 30), scaleSize, 0.1F, 3.0F);
    }

}
