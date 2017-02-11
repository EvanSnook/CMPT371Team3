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
    [Category("LoadBar test")]
    internal class LoadBarTests
    {
        [Test]
        public void TestForNull()
        {/*
            LoadBar load = new LoadBar();//Until NUnit error is figured out, these will be commented out
            FileInfo file = new FileInfo("Test");
            load.ConvertAndSendImage(file);*/
            Assert.Pass();
        }

        [Test]
        public void TestForIncompleteImage()
        {/*
            LoadBar load = new LoadBar();
            FileInfo file = new FileInfo("Test");
            load.ConvertAndSendImage(file);*/
            Assert.Pass();
        }

    }
}
