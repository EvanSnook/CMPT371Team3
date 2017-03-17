using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.IO;

public class InvokeTest : MonoBehaviour {



	// Use this for initialization
	void Start () {
        string path = @"E:\Git Projects\CMPT371Team3\Luxsonic Project\Assets\Resources\DICOM";
        Process newProcess = new Process();
        newProcess.StartInfo.FileName = @"E:\Git Projects\CMPT371Team3\Converter\DICOMConverter.exe";
        newProcess.StartInfo.Arguments = path;
        newProcess.StartInfo.UseShellExecute = false;
        newProcess.StartInfo.CreateNoWindow = true;
        newProcess.EnableRaisingEvents = true;
        newProcess.Start();
        newProcess.WaitForExit();

        UnityEngine.Debug.Log(newProcess.ExitCode);

        var filePath = @"Assets\Resources\DICOM\temp\00000001";

        if (File.Exists(filePath))
        {
            var fileData = File.ReadAllBytes(filePath);
            var texture = new Texture2D(2, 2);
            texture.LoadImage(fileData);
            gameObject.GetComponent<GUITexture>().texture = texture;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
