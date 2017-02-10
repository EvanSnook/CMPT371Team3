using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class LoadBar : MonoBehaviour
{

    //True while pictures are still being loaded
    bool loading;

    //List of file paths
    List<string> files;

    // Use this for initialization
    void Start()
    {
        loading = true;
        files = new List<string>();
    }

    void OnGUI()
    {
        if (loading)
        {
            GUI.Box(new Rect(0, 0, 200, 200), "Loading Images");
            loadImages();
        }
    }

    void DirSearch(string sDir)
    {
        //try
        //{
        string[] direct = Directory.GetDirectories(sDir);
        for (int d = 0; d < direct.Length; d++)
        {
            try
            {
                foreach (string f in Directory.GetFiles(direct[d], "*.dcm"))
                {
                    files.Add(f);
                }
            }
            catch (UnauthorizedAccessException)
            {
                Debug.Log("caught the exception");
            }
            try
            {
                DirSearch(direct[d]);
            }
            catch (UnauthorizedAccessException)
            {
                Debug.Log("Caught another exception");
            }
            // }
            //catch (UnauthorizedAccessException)
            //{
            //  Debug.Log("Restricted file found, must ignore");
            //  Debug.Log(sDir);
            //  Debug.Log("Restricted file above, must ignore");
            //}
        }
    }

    void loadImages()
    {
        while (loading)
        {
            Debug.Log("About to load images");
            DirSearch(@"c:\");
            Debug.Log(files.Count);
            loading = false;

            //string dicomDirectory = Path.GetFullPath(@"DICOMImages");
            /*
            try
            {
                string[] dicomDirectory = Directory.GetDirectories(@"c:\", "DICOMImages", SearchOption.AllDirectories);
                Debug.Log(dicomDirectory.Length);
            }
            catch(UnauthorizedAccessException)
            {
                Debug.Log("Restricted File found, will not search it.");
            }
            finally { }
           // DirectoryInfo directory = new DirectoryInfo(dicomDirectory);
            //FileInfo[] info = directory.GetFiles();
            //Debug.Log(info.Length);
            loading = false;
                }
    }*/
        }
    }

}

