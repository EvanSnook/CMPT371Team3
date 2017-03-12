// Copyright (c) 2012-2017 Anders Gustafsson, Cureos AB.
// All rights reserved. Any unauthorised reproduction of this 
// material will constitute an infringement of copyright.

using System.Text;

using UnityEngine;

using Dicom;
using Dicom.Imaging;
using Dicom.Log;

public class Test : MonoBehaviour
{
	private string _dump;
    private Texture2D _texture;

    // Use this for initialization
    private void Start()
    {
		var file = DicomFile.Open("Assets/US-RGB-8-epicard.asset");
		_dump = file.WriteToString();
        _texture = new DicomImage(file.Dataset).RenderImage().AsTexture2D();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnGUI()
    {
		GUI.DrawTexture (new Rect (0, 0, _texture.width, _texture.height), _texture);
		GUI.Label (new Rect (_texture.width, 0, Screen.width - _texture.width, Screen.height), _dump);				
	}
}
