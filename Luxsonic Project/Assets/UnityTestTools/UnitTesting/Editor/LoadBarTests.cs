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
            //Creating Mock
            GameObject mannyObj = new GameObject();
            mannyObj.AddComponent<ImageManager>();
            ImageManager manny = mannyObj.GetComponent<ImageManager>();
            GameObject thumbObject = new GameObject();
            thumbObject.AddComponent<Thumbnail>();
            thumbObject.AddComponent<SpriteRenderer>();
            manny.thumbnail = thumbObject;
            mannyObj.AddComponent<LoadBar>();

            //LoadBar load = new LoadBar();
            LoadBar load = mannyObj.GetComponent<LoadBar>();
            FileInfo file = null;
            load.ConvertAndSendImage(file);
        }

        [Test]
        public void TestForCompleteImage()
        {
            //Creating Mock
            GameObject mannyObj = new GameObject();
            mannyObj.AddComponent<ImageManager>();
            ImageManager manny = mannyObj.GetComponent<ImageManager>();
            GameObject thumbObject = new GameObject();
            thumbObject.AddComponent<Thumbnail>();
            thumbObject.AddComponent<SpriteRenderer>();
            manny.thumbnail = thumbObject;
            mannyObj.AddComponent<LoadBar>();
   
            //LoadBar load = new LoadBar();
            LoadBar load = mannyObj.GetComponent<LoadBar>();

            FileInfo file = new FileInfo (@"Assets/resources/Test.png");
            load.imageManager = manny;
            load.ConvertAndSendImage(file);
        }

    }
}
