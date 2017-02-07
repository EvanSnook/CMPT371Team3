using System;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Custom/Brightness Effect")]

public class ContrastSlider : ImageEffectBase
{
    
    [Range(0f, 1f)]
    public float Contrast = 0.5f;

    // Called by camera to apply image effect
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
       
        material.SetFloat("Contrast", Contrast);
        Graphics.Blit(source, destination, material);
    }
 
    public void SetContrast(float value)
    { 
       Contrast = GUI.HorizontalSlider(new Rect(25, 25, 100, 30), value, 0.0F, 1.0F);
    }
}