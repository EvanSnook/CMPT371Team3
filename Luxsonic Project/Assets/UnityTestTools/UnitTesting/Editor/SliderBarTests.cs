using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using NSubstitute;

[TestFixture]
[Category("Unit Tests")]
public class SliderBarTests {

	[Test]
    public void TestSetup()
    {
        GameObject sliderObject = new GameObject();
        sliderObject.AddComponent<MeshCollider>();
        sliderObject.AddComponent<MeshRenderer>();
        sliderObject.AddComponent<SliderBar>();

        SliderBar sliderScript = sliderObject.GetComponent<SliderBar>();

        GameObject sliderKnobObject = new GameObject();
        sliderKnobObject.AddComponent<SliderKnob>();

        sliderScript.sliderKnobPrefab = sliderKnobObject;

        // Make sure the knob values are set properly
        Assert.AreEqual(sliderKnobObject.GetComponent<SliderKnob>().GetLeftBoundary(), sliderScript.GetMaxCoord());
        Assert.AreEqual(sliderKnobObject.GetComponent<SliderKnob>().GetRightBoundary(), sliderScript.GetMinCoord());
        Assert.AreEqual(sliderKnobObject.GetComponent<SliderKnob>().transform.position, sliderScript.GetKnobCoord());     
    }

    [Test]
    public void TestConvertFromCoordToScale()
    {
        GameObject sliderObject = new GameObject();
        sliderObject.AddComponent<MeshCollider>();
        sliderObject.AddComponent<MeshRenderer>();
        sliderObject.AddComponent<SliderBar>();

        SliderBar sliderScript = sliderObject.GetComponent<SliderBar>();

        sliderScript.SetMaxCoord(new Vector3(600, 100, 200));
        sliderScript.SetMinCoord(new Vector3(300, 100, 200));
        float val = sliderScript.ConvertFromCoordToScale(450);

        // The coordinates of the slider are minX = 300, maxX = 600
        // 300       450        600
        // ==========Knob==========

        // The scale of the slider is min = 0, max = 300
        // 0         150       300
        // ==========Knob=========

        // The percentage is a value between 0 and 1
        // 0         0.5       1
        // ==========Knob========
        
        Assert.AreEqual(val, 150);
    }

    [Test]
    public void TestConvertScaleToCoord()
    {
        GameObject sliderObject = new GameObject();
        sliderObject.AddComponent<MeshCollider>();
        sliderObject.AddComponent<MeshRenderer>();
        sliderObject.AddComponent<SliderBar>();

        SliderBar sliderScript = sliderObject.GetComponent<SliderBar>();

        sliderScript.SetMaxCoord(new Vector3(600, 100, 200));
        sliderScript.SetMinCoord(new Vector3(300, 100, 200));
        sliderScript.SetMaxScale();
       
        float val = sliderScript.ConvertFromScaleToCoord(150);

        // The coordinates of the slider are minX = 300, maxX = 600
        // 300       450        600
        // ==========Knob==========

        // The scale of the slider is min = 0, max = 300
        // 0         150       300
        // ==========Knob=========

        // The percentage is a value between 0 and 1
        // 0         0.5       1
        // ==========Knob========

        Assert.AreEqual(val, 450);
    }

    [Test]
    public void TestConvertFromCoordToPercentOfSlider()
    {
        GameObject sliderObject = new GameObject();
        sliderObject.AddComponent<MeshCollider>();
        sliderObject.AddComponent<MeshRenderer>();
        sliderObject.AddComponent<SliderBar>();

        SliderBar sliderScript = sliderObject.GetComponent<SliderBar>();

        sliderScript.SetMaxCoord(new Vector3(600, 100, 200));
        sliderScript.SetMinCoord(new Vector3(300, 100, 200));
        sliderScript.SetMaxScale();
        

        float val = sliderScript.ConvertFromCoordToPercentOfSlider(450);

        // The coordinates of the slider are minX = 300, maxX = 600
        // 300       450        600
        // ==========Knob==========

        // The scale of the slider is min = 0, max = 300
        // 0         150       300
        // ==========Knob=========

        // The percentage is a value between 0 and 1
        // 0         0.5       1
        // ==========Knob========

        Assert.AreEqual(val, 0.5);
    }

    [Test]
    public void TestConvertFromPercentOfSliderToCoord()
    {
        GameObject sliderObject = new GameObject();
        sliderObject.AddComponent<MeshCollider>();
        sliderObject.AddComponent<MeshRenderer>();
        sliderObject.AddComponent<SliderBar>();

        SliderBar sliderScript = sliderObject.GetComponent<SliderBar>();

        sliderScript.SetMaxCoord(new Vector3(600, 100, 200));
        sliderScript.SetMinCoord(new Vector3(300, 100, 200));
        sliderScript.SetMaxScale();


        float val = sliderScript.ConvertFromPercentOfSliderToCoord(0.5f);

        // The coordinates of the slider are minX = 300, maxX = 600
        // 300       450        600
        // ==========Knob==========

        // The scale of the slider is min = 0, max = 300
        // 0         150       300
        // ==========Knob=========

        // The percentage is a value between 0 and 1
        // 0         0.5       1
        // ==========Knob========

        Assert.AreEqual(val, 450);
    }

    [Test]
    public void TestConvertFromPercentOfSliderToScale()
    {
        GameObject sliderObject = new GameObject();
        sliderObject.AddComponent<MeshCollider>();
        sliderObject.AddComponent<MeshRenderer>();
        sliderObject.AddComponent<SliderBar>();

        SliderBar sliderScript = sliderObject.GetComponent<SliderBar>();

        sliderScript.SetMaxCoord(new Vector3(600, 100, 200));
        sliderScript.SetMinCoord(new Vector3(300, 100, 200));
        sliderScript.SetMaxScale();


        float val = sliderScript.ConvertFromPercentOfSliderToScale(0.5f);

        // The coordinates of the slider are minX = 300, maxX = 600
        // 300       450        600
        // ==========Knob==========

        // The scale of the slider is min = 0, max = 300
        // 0         150       300
        // ==========Knob=========

        // The percentage is a value between 0 and 1
        // 0         0.5       1
        // ==========Knob========

        Assert.AreEqual(val, 150);
    }

    [Test]
    public void TestConvertFromScaleToPercentOfSlider()
    {
        GameObject sliderObject = new GameObject();
        sliderObject.AddComponent<MeshCollider>();
        sliderObject.AddComponent<MeshRenderer>();
        sliderObject.AddComponent<SliderBar>();

        SliderBar sliderScript = sliderObject.GetComponent<SliderBar>();

        sliderScript.SetMaxCoord(new Vector3(600, 100, 200));
        sliderScript.SetMinCoord(new Vector3(300, 100, 200));
        sliderScript.SetMaxScale();


        float val = sliderScript.ConvertFromScaleToPercentOfSlider(150);

        // The coordinates of the slider are minX = 300, maxX = 600
        // 300       450        600
        // ==========Knob==========

        // The scale of the slider is min = 0, max = 300
        // 0         150       300
        // ==========Knob=========

        // The percentage is a value between 0 and 1
        // 0         0.5       1
        // ==========Knob========

        Assert.AreEqual(val, 0.5);
    }


}
