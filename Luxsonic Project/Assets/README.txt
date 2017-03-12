fo-dicom for Unity
==================

Copyright (c) 2012-2017 fo-dicom contributors; UnityImage and UnityImageManager implementations (c) 2016-2017 Anders Gustafsson, Cureos AB
Licensed under Microsoft Public License, MS-PL, except UnityImage and UnityImageManager implementations.
All rights reserved.


Installation
------------
fo-dicom for Unity is provided as a single assembly DLL. It is built against Unity version 4.5.0, and the DLL is dependent upon UnityEngine.dll.

To install fo-dicom for Unity, open the Unity Asset Store and search for fo-dicom. Click on the Buy button. After completing the purchase the Import Unity Package is displayed. Click Import to install the DLL and associated license and README documentation.


Usage
-----
fo-dicom for Unity is a scaled-down release of fo-dicom, excluding asynchronous method calls, networking and JPEGx compressed codec support.
Unity specific image rendering is available. DICOM dictionary is synchronized with DICOM version 2016e.

Code examples:

    // Load DICOM object from file (loading from Stream is also supported)
    var file = DicomFile.Open(@"test.dcm");

    // Get data from file
    var patientid = file.Dataset.Get<string>(DicomTag.PatientID);
    var beamSequence = file.Dataset.Get<DicomSequence>(DicomTag.BeamSequence);
    foreach (var item in beamSequence.Items) {
        var beamNumber = item.Get<int>(DicomTag.BeamNumber);
    }

    // Add elements to existing DICOM object
    file.Dataset.Add(DicomTag.PatientsName, "DOE^JOHN");
    beamSequence.Items[2].Add(DicomTag.CompensatorTransmissionData, 0.1, 0.2, 0.3, 0.2, 0.3, 0.4, 0.3, 0.4, 0.5);

    // Create a new instance of DicomFile with different transfer syntax
    var newFile = file.ChangeTransferSyntax(DicomTransferSyntax.RLELossless);

    // Save updated file
    newFile.Save(@"output.dcm");

    // Render Image to Texture2D
    var image = new DicomImage(@"test.dcm");
    var texture = image.RenderImage().AsTexture2D();

    // Forward logging messages to Console:
    LogManager.SetImplementation(ConsoleLogManager.Instance);
	
    // Switch off logging messages
    LogManager.SetImplementation(null);
	
Additional usage information is available from the fo-dicom Github site, here: https://github.com/fo-dicom/fo-dicom


Support
-------
E-mail to: support@cureos.com
