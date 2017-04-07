using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using buttons;

[TestFixture]
[Category("Unit Tests")]
public class DashboardTests
{



    [Test]
    public void TestCreateButton()
    {
        GameObject dashObj = new GameObject();
        dashObj.AddComponent<Dashboard>();
        Dashboard dash = dashObj.GetComponent<Dashboard>();

        GameObject buttonObj = new GameObject();
        buttonObj.AddComponent<VRButton>();

        GameObject textObj = new GameObject();
        textObj.AddComponent<TextMesh>();
        textObj.transform.SetParent(buttonObj.transform);

        dash.planePrefab = new GameObject();
        dash.buttonPrefab = buttonObj;

        ButtonAttributes attributes = new ButtonAttributes();
        GameObject button = dash.CreateButton(attributes, dash.buttonPrefab);

        Assert.IsNotNull(button);

    }

    [Test]
    public void TestDisplayMenu()
    {
        GameObject dashObj = new GameObject();
        dashObj.AddComponent<Dashboard>();
        Dashboard dash = dashObj.GetComponent<Dashboard>();

        GameObject buttonObj = new GameObject();
        buttonObj.AddComponent<VRButton>();

        GameObject textObj = new GameObject();
        textObj.AddComponent<TextMesh>();
        textObj.transform.SetParent(buttonObj.transform);

        dash.planePrefab = new GameObject();
        dash.buttonPrefab = buttonObj;

        ButtonAttributes loadAttr = new ButtonAttributes();
        loadAttr.buttonName = "Load";
        loadAttr.position = new Vector3(0, 0.3f, 0);
        loadAttr.type = ButtonType.MENU_BUTTON;
        loadAttr.rotation = new Vector3(0, 0, 0);
        loadAttr.depressable = false;
        loadAttr.autoPushOut = false;
        loadAttr.buttonFunction = "Load";

        ButtonAttributes quitAttr = new ButtonAttributes();
        quitAttr.buttonName = "Quit";
        quitAttr.position = new Vector3(0, 0f, 0);
        quitAttr.type = ButtonType.MENU_BUTTON;
        quitAttr.rotation = new Vector3(0, 0, 0);
        quitAttr.depressable = false;
        quitAttr.autoPushOut = false;
        quitAttr.buttonFunction = "Quit";

        ButtonAttributes minimizeAttr = new ButtonAttributes();
        minimizeAttr.buttonName = "Minimize";
        minimizeAttr.position = new Vector3(0, -0.3f, 0);
        minimizeAttr.type = ButtonType.MENU_BUTTON;
        minimizeAttr.rotation = new Vector3(0, 0, 0);
        minimizeAttr.depressable = false;
        minimizeAttr.autoPushOut = false;
        minimizeAttr.buttonFunction = "Minimize";

        ButtonAttributes contrastAttr = new ButtonAttributes();
        contrastAttr.buttonName = "Contrast";
        contrastAttr.position = new Vector3(0, -0.3f, 0);
        contrastAttr.type = ButtonType.COPY_MODIFIER;
        contrastAttr.rotation = new Vector3(0, 0, 0);
        contrastAttr.depressable = false;
        contrastAttr.autoPushOut = false;
        contrastAttr.buttonFunction = "Contrast";

        ButtonAttributes zoomAttr = new ButtonAttributes();
        zoomAttr.buttonName = "Zoom";
        zoomAttr.position = new Vector3(0, -0.3f, 0);
        zoomAttr.type = ButtonType.COPY_MODIFIER;
        zoomAttr.rotation = new Vector3(0, 0, 0);
        zoomAttr.depressable = false;
        zoomAttr.autoPushOut = false;
        zoomAttr.buttonFunction = "Zoom";

        ButtonAttributes invertAttr = new ButtonAttributes();
        invertAttr.buttonName = "Invert";
        invertAttr.position = new Vector3(0, -0.3f, 0);
        invertAttr.type = ButtonType.COPY_MODIFIER;
        invertAttr.rotation = new Vector3(0, 0, 0);
        invertAttr.depressable = false;
        invertAttr.autoPushOut = false;
        invertAttr.buttonFunction = "Invert";

        ButtonAttributes brightnessAttr = new ButtonAttributes();
        brightnessAttr.buttonName = "Brightness";
        brightnessAttr.position = new Vector3(0, -0.3f, 0);
        brightnessAttr.type = ButtonType.COPY_MODIFIER;
        brightnessAttr.rotation = new Vector3(0, 0, 0);
        brightnessAttr.depressable = false;
        brightnessAttr.autoPushOut = false;
        brightnessAttr.buttonFunction = "Brightness";

        ButtonAttributes resizeAttr = new ButtonAttributes();
        resizeAttr.buttonName = "Resize";
        resizeAttr.position = new Vector3(0, -0.3f, 0);
        resizeAttr.type = ButtonType.COPY_MODIFIER;
        resizeAttr.rotation = new Vector3(0, 0, 0);
        resizeAttr.depressable = false;
        resizeAttr.autoPushOut = false;
        resizeAttr.buttonFunction = "Resize";

        ButtonAttributes saturationAttr = new ButtonAttributes();
        saturationAttr.buttonName = "Saturation";
        saturationAttr.position = new Vector3(0, -0.3f, 0);
        saturationAttr.type = ButtonType.COPY_MODIFIER;
        saturationAttr.rotation = new Vector3(0, 0, 0);
        saturationAttr.depressable = false;
        saturationAttr.autoPushOut = false;
        saturationAttr.buttonFunction = "Saturation";

        ButtonAttributes closeAttr = new ButtonAttributes();
        closeAttr.buttonName = "Close";
        closeAttr.position = new Vector3(0, -0.3f, 0);
        closeAttr.type = ButtonType.COPY_MODIFIER;
        closeAttr.rotation = new Vector3(0, 0, 0);
        closeAttr.depressable = false;
        closeAttr.autoPushOut = false;
        closeAttr.buttonFunction = "Close";

        ButtonAttributes restoreAttr = new ButtonAttributes();
        restoreAttr.buttonName = "Restore";
        restoreAttr.position = new Vector3(0, -0.3f, 0);
        restoreAttr.type = ButtonType.COPY_MODIFIER;
        restoreAttr.rotation = new Vector3(0, 0, 0);
        restoreAttr.depressable = false;
        restoreAttr.autoPushOut = false;
        restoreAttr.buttonFunction = "Restore";

        ButtonAttributes selectAllAttr = new ButtonAttributes();
        selectAllAttr.buttonName = "Select All";
        selectAllAttr.position = new Vector3(0, -0.3f, 0);
        selectAllAttr.type = ButtonType.COPY_MODIFIER;
        selectAllAttr.rotation = new Vector3(0, 0, 0);
        selectAllAttr.depressable = false;
        selectAllAttr.autoPushOut = false;
        selectAllAttr.buttonFunction = "SelectAll";

        ButtonAttributes deselectAllAttr = new ButtonAttributes();
        deselectAllAttr.buttonName = "Deselect All";
        deselectAllAttr.position = new Vector3(0, -0.3f, 0);
        deselectAllAttr.type = ButtonType.COPY_MODIFIER;
        deselectAllAttr.rotation = new Vector3(0, 0, 0);
        deselectAllAttr.depressable = false;
        deselectAllAttr.autoPushOut = false;
        deselectAllAttr.buttonFunction = "DeselectALL";

        List<ButtonAttributes> attributes = new List<ButtonAttributes>();
        attributes.Add(loadAttr);
        attributes.Add(quitAttr);
        attributes.Add(minimizeAttr);
        attributes.Add(contrastAttr);
        attributes.Add(zoomAttr);
        attributes.Add(invertAttr);
        attributes.Add(brightnessAttr);
        attributes.Add(resizeAttr);
        attributes.Add(saturationAttr);
        attributes.Add(closeAttr);
        attributes.Add(restoreAttr);
        attributes.Add(selectAllAttr);
        attributes.Add(deselectAllAttr);
        

        dash.SetButtonList(attributes);



        dash.DisplayMenu();
        VRButton[] myButtons = Transform.FindObjectsOfType<VRButton>();
        Assert.Greater(myButtons.Length, 0);
        bool loadIn = false;
        bool quitIn = false;
        bool minimizeIn = false;
        bool contrastIn = false;
        bool zoomIn = false;
        bool invertIn = false;
        bool brightnessIn = false;
        bool resizeIn = false;
        bool saturationIn = false;
        bool closeIn = false;
        bool restoreIn = false;
        bool selectAllIn = false;
        bool deselectAllIn = false;

        for (int i = 0; i < myButtons.Length; i = i + 1)
        {
            VRButton button = (VRButton)myButtons.GetValue(i);
            switch (button.name)
            {
                case "Load":
                    // If the load button was clicked
                    loadIn = true;
                    break;
                case "Quit":
                    // If the quit button was clicked
                    quitIn = true;
                    break;
                case "Minimize":
                    // If the minimize button was clicked
                    minimizeIn = true;
                    break;
                case "Contrast":
                    // If the contrast button was clicked
                    contrastIn = true;
                    break;
                case "Zoom":
                    // If the zoom button was clicked
                    zoomIn = true;
                    break;
                case "Invert":
                    // If the invert button was clicked
                    invertIn = true;
                    break;
                case "Brightness":
                    // If the brightness button was clicked
                    brightnessIn = true;
                    break;
                case "Resize":
                    // If the resize button was clicked
                    resizeIn = true;
                    break;
                case "Saturation":
                    // If the saturation button was clicked
                    saturationIn = true;
                    break;
                case "Close":
                    // If the close button was clicked
                    closeIn = true;
                    break;
                case "Restore":
                    // If the restore button was clicked
                    restoreIn = true;
                    break;
                case "Select All":
                    // If the select all button was clicked
                    selectAllIn = true;
                    break;
                case "Deselect All":
                    // Id the deselect all button was clicked
                    deselectAllIn = true;
                    break;
            }
        }
        Assert.IsTrue(loadIn);
        Assert.IsTrue(quitIn);
        Assert.IsTrue(minimizeIn);
        Assert.IsTrue(contrastIn);
        Assert.IsTrue(zoomIn);
        Assert.IsTrue(invertIn);
        Assert.IsTrue(brightnessIn);
        Assert.IsTrue(resizeIn);
        Assert.IsTrue(saturationIn);
        Assert.IsTrue(closeIn);
        Assert.IsTrue(restoreIn);
        Assert.IsTrue(selectAllIn);
        Assert.IsTrue(deselectAllIn);
    }

    [Test]
    public void TestUpdateCurrentSelection()
    {
        GameObject dashObj = new GameObject();
        dashObj.AddComponent<Dashboard>();
        Dashboard dash = dashObj.GetComponent<Dashboard>();

        GameObject buttonObj = new GameObject();
        buttonObj.AddComponent<VRButton>();

        GameObject textObj = new GameObject();
        textObj.AddComponent<TextMesh>();
        textObj.transform.SetParent(buttonObj.transform);

        dash.planePrefab = new GameObject();
        dash.buttonPrefab = buttonObj;

        ButtonAttributes loadAttr = new ButtonAttributes();
        loadAttr.buttonName = "Load";
        loadAttr.position = new Vector3(0, 0.3f, 0);
        loadAttr.type = ButtonType.MENU_BUTTON;
        loadAttr.rotation = new Vector3(0, 0, 0);
        loadAttr.depressable = false;
        loadAttr.autoPushOut = false;
        loadAttr.buttonFunction = "Load";

        List<ButtonAttributes> attributes = new List<ButtonAttributes>();
        attributes.Add(loadAttr);
        dash.SetButtonList(attributes);
        dash.DisplayMenu();

        bool test = dash.TestUpdateCurrentSelection("Load");
        Assert.IsTrue(test);
        
    }

    [Test]
    public void TestCopySelected()
    {
        GameObject dashObj = new GameObject();
        dashObj.AddComponent<Dashboard>();
        Dashboard dash = dashObj.GetComponent<Dashboard>();

        GameObject buttonObj = new GameObject();
        buttonObj.AddComponent<VRButton>();

        GameObject textObj = new GameObject();
        textObj.AddComponent<TextMesh>();
        textObj.transform.SetParent(buttonObj.transform);

        dash.planePrefab = new GameObject();
        dash.buttonPrefab = buttonObj;
        dash.currentCopies = new List<GameObject>();
        GameObject copyObj = new GameObject();
        copyObj.AddComponent<Copy>();
        Copy cop = copyObj.GetComponent<Copy>();

        cop.isCurrentImage = true;

        dash.CopySelected(copyObj);
        Assert.Contains(copyObj, dash.currentCopies);

        cop.isCurrentImage = false;
        dash.CopySelected(copyObj);
        Assert.IsFalse(dash.currentCopies.Contains(copyObj));
    }


    [Test]
   [Ignore("Send message does not work in editor mode")]
    public void TestSelectAllCopies()
    {
        GameObject dashObj = new GameObject();
        dashObj.AddComponent<Dashboard>();
        Dashboard dash = dashObj.GetComponent<Dashboard>();

        GameObject buttonObj = new GameObject();
        buttonObj.AddComponent<VRButton>();

        GameObject textObj = new GameObject();
        textObj.AddComponent<TextMesh>();
        textObj.transform.SetParent(buttonObj.transform);

        dash.planePrefab = new GameObject();
        dash.buttonPrefab = buttonObj;
        dash.currentCopies = new List<GameObject>();

        GameObject copyObj = new GameObject();
        copyObj.AddComponent<Copy>();
        Copy cop = copyObj.GetComponent<Copy>();
        cop.SetDashboard(dashObj);
        List<GameObject> copies = new List<GameObject>();
        copies.Add(copyObj);

        GameObject copyOutline = new GameObject();
        copyOutline.transform.SetParent(copyObj.transform);

        GameObject displayObj = new GameObject();
        displayObj.AddComponent<Display>();
        Display disp = displayObj.GetComponent<Display>();
        disp.SetCopies(copies);

        dash.display = disp;

        dash.SelectAllCopies();

        foreach(GameObject copy in disp.GetCopies())
        {
            Assert.IsTrue(copy.GetComponent<Copy>().isCurrentImage);
        }
    }

    [Test]
    [Ignore("Send message does not work in editor mode")]
    public void TestDeselectAllCopies()
    {
            GameObject dashObj = new GameObject();
            dashObj.AddComponent<Dashboard>();
            Dashboard dash = dashObj.GetComponent<Dashboard>();

            GameObject buttonObj = new GameObject();
            buttonObj.AddComponent<VRButton>();

            GameObject textObj = new GameObject();
            textObj.AddComponent<TextMesh>();
            textObj.transform.SetParent(buttonObj.transform);

            dash.planePrefab = new GameObject();
            dash.buttonPrefab = buttonObj;
            dash.currentCopies = new List<GameObject>();

            GameObject copyObj = new GameObject();
            copyObj.AddComponent<Copy>();
            Copy cop = copyObj.GetComponent<Copy>();
            cop.SetDashboard(dashObj);
            List<GameObject> copies = new List<GameObject>();
            copies.Add(copyObj);

            GameObject copyOutline = new GameObject();
            copyOutline.transform.SetParent(copyObj.transform);

            GameObject displayObj = new GameObject();
            displayObj.AddComponent<Display>();
            Display disp = displayObj.GetComponent<Display>();
            disp.SetCopies(copies);

            dash.display = disp;

            dash.SelectAllCopies();
            dash.DeselectAllCopies();
            Assert.AreEqual(0, dash.currentCopies.Count);
    }

    
}