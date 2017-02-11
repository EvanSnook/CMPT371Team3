using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using System.IO;
//using class LoadBar;



    [TestFixture]
    [Category("LoadBar test")]
    internal class LoadBarTest
    {
        [Test]
        public void TestForNull()
        {
        LoadBar load = new LoadBar();
        FileInfo file = new FileInfo("Test");
        load.ConvertAndSendImage(file);

        }
    }
