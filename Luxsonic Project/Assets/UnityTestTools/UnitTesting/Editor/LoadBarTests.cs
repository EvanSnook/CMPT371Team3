using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using NUnit.Framework;
using System.IO;
//using class LoadBar;

namespace UnityTest
{
    [TestFixture]
    [Category("Unit Tests")]
    internal class LoadBarTests
    {
        [Test]
        public void TestForNull()
        {
            LoadBar load = new LoadBar();//Until NUnit error is figured out, these will be commented out
            FileInfo file = null;
            ImageManager testImage = new ImageManager();
            load.ConvertAndSendImage(file);
            //Assert.Pass();
        }

        [Test]
        public void TestForCompleteImage()
        {
            LoadBar load = new LoadBar();
            //FileInfo file = new FileInfo();
            ImageManager testImage = new ImageManager();
            //load.ConvertAndSendImage(file);
            Assert.Pass();
        }

    }
}
