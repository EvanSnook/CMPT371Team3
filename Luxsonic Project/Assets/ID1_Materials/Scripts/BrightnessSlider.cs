using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrightnessSlider : MonoBehaviour
{

    public float lightValue = 0.0F;
    public GameObject image;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Light light = image.AddComponent<Light>();
        light.color = new Color(lightValue, lightValue, lightValue);

        
    }

    void OnGUI()
    {
        lightValue = GUI.HorizontalSlider(new Rect(25, 225, 100, 30), lightValue, 0.0F, 1.0F);
    }

}
