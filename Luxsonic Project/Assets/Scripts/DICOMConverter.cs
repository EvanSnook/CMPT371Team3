using System.Collections;
using UnityEngine;
using System.Diagnostics;
using System;


/// <summary>
/// DICOMConverter class to call the external converter program.
/// </summary>
public class DICOMConverter : MonoBehaviour
{

    private bool waitingForConversion;


    /// <summary>
    /// Function to call the external DICOM image converter program. The external program simply converts
    /// the files and deposits them in the destinationPath. Function is run as a Coroutine process. Coroutine
    /// funtions return an "IEnumerator" periodically, to allow for yielding to other Coroutines.
    /// </summary>
    /// <param name="mode">The type of conversion wanted, should be either "f" for single file, or "d" for entire directory</param>
    /// <param name="targetPath">Path of either file or folder, as a target for conversion</param>
    /// <param name="destinationPath"></param>
	public IEnumerator ExternalConverter(string mode, string targetPath, string destinationPath)
    {
        Process newProcess = new Process();
        newProcess.StartInfo.FileName = Application.dataPath + @"\DICOMConverter\DICOMConverter.exe";
        newProcess.StartInfo.Arguments = mode + " \"" + targetPath + "\" " + "\"" + destinationPath + "\""; // escape characters to keep ' " ' marks in the string, so that the program handles paths correctly
        newProcess.StartInfo.UseShellExecute = false;
        newProcess.StartInfo.CreateNoWindow = true;
        newProcess.EnableRaisingEvents = true;
        waitingForConversion = true;
        newProcess.Exited += new EventHandler(ProcessExited);

        // Start external program
        newProcess.Start();

        while (waitingForConversion)
        {
            yield return null;
        }
    }


    /// <summary>
    /// Event handler for when a proccess exited. Used to signal the ExternalConverter() funtion that the
    /// external converter program has finished.
    /// </summary>
    /// <param name="sender">Process object that sent the event</param>
    /// <param name="e">Event arguments passed in on the event</param>
	internal void ProcessExited(object sender, System.EventArgs e)
    {
        waitingForConversion = false;
    }
}