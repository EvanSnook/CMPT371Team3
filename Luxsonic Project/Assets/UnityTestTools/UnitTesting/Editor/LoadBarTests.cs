using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using NUnit.Framework;
using System.IO;
using UnityEditor;
using NSubstitute;
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
            GameObject mannyObj = new GameObject();
            mannyObj.AddComponent<ImageManager>();
            ImageManager manny = mannyObj.GetComponent<ImageManager>();
            Texture2D tex = new Texture2D(100, 100);

            GameObject thumbObject = new GameObject();
            Thumbnail thumb = Substitute.For<Thumbnail>();
            thumbObject.AddComponent<Thumbnail>();
            thumbObject.AddComponent<SpriteRenderer>();


            manny.thumbnail = thumbObject;

            LoadBar load = new LoadBar();
            
            FileInfo file = new FileInfo (@"Assets/resources/Test.png");
            load.imageManager = manny;
            load.ConvertAndSendImage(file);
        }

    }
}
