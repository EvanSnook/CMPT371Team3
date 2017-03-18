using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.IO;

public class InvokeTest : MonoBehaviour {



	// Use this for initialization
	void Start () {
        string path = @"V:\cmpt\cswin\Desktop\GitHub\CMPT371Team3\Luxsonic Project\Assets\Resources\DICOM";
        Process newProcess = new Process();
        newProcess.StartInfo.FileName = @"V:\cmpt\cswin\Desktop\GitHub\CMPT371Team3\Converter\DICOMConverter.exe";
        newProcess.StartInfo.Arguments = "\"" + path + "\"";
        newProcess.StartInfo.UseShellExecute = false;
        newProcess.StartInfo.CreateNoWindow = true;
        newProcess.EnableRaisingEvents = true;
        newProcess.Start();
        newProcess.WaitForExit();

        var filePath = @"Assets\Resources\DICOM\temp\00000001.jpg";

        if (File.Exists(filePath))
        {
            FileInfo file = new FileInfo(filePath);
            byte[] dicomImage = File.ReadAllBytes(file.ToString());
            Texture2D image = new Texture2D(2, 2);
            image.LoadImage(dicomImage);
            gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f));
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
