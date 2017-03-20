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

            if (args.Length > 0)
            {
                var path = args[0];

                DirectoryInfo d = new DirectoryInfo(path);

                var targetPath = path + @"\temp\";
                Directory.CreateDirectory(targetPath);

                foreach (FileInfo file in d.EnumerateFiles())
                {
                    try
                    {
                        Debug.Print(file.Name);
                        Dicom.DicomFile obj = Dicom.DicomFile.Open(file.FullName);
                        var PatientName = obj.Dataset.Get<string>(Dicom.DicomTag.PatientName, null);
                        var image = new DicomImage(obj.Dataset);
                        image.RenderImage().AsBitmap().Save(targetPath + file.Name + ".jpg");
                    }
                    catch (DicomDataException e)
                    {

                    }
                }
            }
        }
    }
}
