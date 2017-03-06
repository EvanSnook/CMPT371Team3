README

Running the most recent approved build:

-Download Build #59 from the Unity Cloud Build interface for the project.
-Extract it to a directory on your local machine.
-Double click the application and select the "Play!" button.
-If not running from an Oculus workstation, run the Windows Scene only.
-If running from an Oculus workstation, the application will pause if the headset is not placed on a user's head.

Running the code from the Unity Editor:

-The user must select the Scenes folder from Assets.  
-Load the ID2_Default scene. by double-clicking.
-Click the Play button on the top of the screen.   

When the program runs, a user interface will be displayed showing two different buttons that the user can select from.  

The top button says “VR Scene”, which when pressed will bring up a virtual reality scene that can be interacted with.(Note: the Oculus Rift hardware must be set up in order for this scene to function properly.)  

If set up, the user will be able to see three buttons in the workspace: Minimize, Quit and Load. The user must equip the Oculus headset and both controllers in order to interact with the scene.  The user can interact with any of the buttons.  The minimize button will minimize the other two buttons, the quit button will exit the application, and the Load button will load a random pre-loaded image into the Display, as well as into the Tray in the lower right corner.  When the load button has been pressed three times, a left scrolling button and a right scrolling button will appear to allow the user to scroll through the images in the Display.  The user may select an image Thumbnail from the tray.  This will create a Copy of that image in the workspace that the user can view.    

The scene can be exited at any time when the user removes the headset and clicks the Play button in the Unity editor, or via the quit button if the application is being run from the executable.

The other scene that can be accessed is called the “Windows Scene”.  Clicking this scene with the mouse will bring the user to a simulated VR scene that allows the user to interact and make changes with mouse clicks. 

This scene will be similar to the VR scene, but it will be more condensed to allow it to fit in the window.  The user will be presented with the Minimize, Quit, and Load buttons.  These buttons will have the same functionality as in the VR scene.  When the user selects a thumbnail, a Copy of it is presented in the workspace.   The user can select this enlarged image to bring up a series of buttons: zoom, rotate, contrast, brightness, flip, resize and close.  The only buttons that are functional without being in VR are the close button, which will close the button display and collapse the image, and the Brightness button, which will bring up a Slider Bar.  The slider bar can be used to adjust the brightness of the Copy image by dragging the handle left or right.  To close the Slider Bar, select the Brightness button again.

The scene can be exited through the Play button on the Unity browser, or via the Quit button if being run from the executable.  
