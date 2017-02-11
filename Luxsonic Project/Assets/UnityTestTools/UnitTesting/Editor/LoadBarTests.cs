using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using System.IO;
//using class LoadBar;


namespace UnityTest{

    [TestFixture]
    [Category("LoadBar test")]
    internal class LoadBarTest
    {
        [Test]
        public void TestForNull()
        {
            FileInfo file = new FileInfo("Test");
           // ConvertAndSendImage(file);
        }
    }
}
