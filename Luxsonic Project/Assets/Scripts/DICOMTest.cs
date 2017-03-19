using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Dicom;
using Dicom.Imaging;
using Dicom.Log;

public class DICOMTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log("Starting test.");
		Dicom.DicomFile obj = Dicom.DicomFile.Open("Assets/Resources/DICOM/00000001");
		var PatientName = obj.Dataset.Get< string>(Dicom.DicomTag.PatientName, null );
		Debug.Log("" + PatientName);
		Debug.Log("" + obj.Dataset.Get<string>(Dicom.DicomTag.PhotometricInterpretation));
		var image = new DicomImage(obj.Dataset);
		var texture = image.RenderImage().AsTexture2D();
		gameObject.GetComponent<GUITexture>().texture = texture;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
