using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadBar : MonoBehaviour
{

    //True while pictures are still being loaded
    bool loading;

    // Use this for initialization
    void Start()
    {
        loading = true;
    }

    void OnGUI()
    {
        GUI.Box(new Rect(0, 0, 200, 200), "Loading Images");
        while (loading)
        {
            string[] dicomDirectory = Directory.GetDirectories(@"C:\", "DICOMImages");
            Debug.Log(dicomDirectory.Length);

            loading = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

