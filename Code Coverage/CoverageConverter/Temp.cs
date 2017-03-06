using System;
using Microsoft.VisualStudio.Coverage.Analysis;

namespace CodeCoverageConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            //arg[0] - path to .coverage
            //arg[1] - path to code (dll, .exe)
            //arg[2] - output file
           
            using (CoverageInfo info = CoverageInfo.CreateFromFile(
                args[0],
                new string[] { @args[1] },
                new string[] { }))
            {
                CoverageDS data = info.BuildDataSet();
                data.WriteXml(args[2]);
            }
        }
    }

}