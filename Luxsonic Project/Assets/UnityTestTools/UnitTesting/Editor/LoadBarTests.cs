using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using NUnit.Framework;
using System.IO;
using UnityEditor;
//using class LoadBar;

namespace LoadBarTests
{
    [TestFixture]
    [Category("Unit Tests")]
    internal class LoadBarTests
    {
        [Test]
        public void TestForNull()
        {
            LoadBar load = new LoadBar();
            FileInfo file = null;
            load.ConvertAndSendImage(file);
        }

        [Test]
        public void TestForCompleteImage()
        {
            LoadBar load = new LoadBar();
            
            FileInfo file = new FileInfo (@"Assets/esources/Test.png");
            load.imageManager = new GameObject();
            load.ConvertAndSendImage(file);
        }

    }
}
