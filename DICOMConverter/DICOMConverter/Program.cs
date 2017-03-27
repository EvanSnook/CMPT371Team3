using System;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dicom;
using Dicom.Imaging;

namespace DICOMConverter
{
    class Program
    {
        static void Main(string[] args)
        {

            if (args.Length == 3)
            {
                var mode = args[0];
                var target = args[1];
                var destinationPath = args[2] + @"\";

                var tempdir = Directory.CreateDirectory(destinationPath);
                tempdir.Attributes = FileAttributes.Directory | FileAttributes.Hidden;

                if (mode.CompareTo("d") == 0)
                {
                    DirectoryInfo d = new DirectoryInfo(target);

                    foreach (FileInfo file in d.EnumerateFiles())
                    {
                        try
                        {
                            Dicom.DicomFile obj = Dicom.DicomFile.Open(file.FullName);
                            var image = new DicomImage(obj.Dataset);
                            image.RenderImage().AsBitmap().Save(destinationPath + @"\" + file.Name + ".jpg");
                        }
                        catch (DicomDataException e)
                        {

                        }
                    }
                }
                else if (mode.CompareTo("f") == 0)
                {
                    try
                    {
                        FileInfo file = new FileInfo(target);
                        Dicom.DicomFile obj = Dicom.DicomFile.Open(file.FullName);
                        var image = new DicomImage(obj.Dataset);
                        image.RenderImage().AsBitmap().Save(destinationPath + @"\" + file.Name + ".jpg");
                    }
                    catch (DicomDataException e)
                    {

                    }
                }

            }
        }
    }
}
