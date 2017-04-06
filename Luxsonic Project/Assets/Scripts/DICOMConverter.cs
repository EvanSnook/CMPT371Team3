using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class DICOMConverter : MonoBehaviour {

	private bool waitingForConversion;

	public IEnumerator ExternalConverter(string type, string targetPath, string destinationPath)
	{
		Process newProcess = new Process(); // uncomment using system diagnostics to fix
		newProcess.StartInfo.FileName = Application.dataPath + @"\DICOMConverter\DICOMConverter.exe";
		newProcess.StartInfo.Arguments = type + " \"" + targetPath + "\" " + "\"" + destinationPath + "\"";
		newProcess.StartInfo.UseShellExecute = false;
		newProcess.StartInfo.CreateNoWindow = true;
		newProcess.EnableRaisingEvents = true;
		waitingForConversion = true;
		newProcess.Exited += new EventHandler(ProcessExited);

		newProcess.Start();

		while (waitingForConversion)
		{
			yield return null;
		}
	}

	internal void ProcessExited(object sender, System.EventArgs e)
	{
		waitingForConversion = false;
	}
}
