README

Running the most recent approved build:

-Download Build #40 from the Unity Cloud Build interface for the project.
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

If set up, the user will be able to see an x-ray image and a green cube.  The user must equip the Oculus headset and both controllers in order to interact with the scene.  Using the back triggers on the Oculus Rift controllers, both the cube and the image can be grabbed at their center and manipulated.  The user can rotate their wrists to rotate the image and pull it towards themselves to zoom on the image.  Letting go of the triggers lets the image stay in the exact position the user had it in while holding the triggers.  
The scene can be exited at any time when the user removes the headset and clicks the Play button in the Unity editor.

The other scene that can be accessed is called the “Windows Scene”.  Clicking this scene with the mouse will bring the user to a simulated VR scene that allows the user to interact and make changes with mouse clicks. 
Initially there will be a single ‘Load’ button, which will import an image into the scene.  The image will appear as a thumbnail in the lower right corner.  The user can select load again to load more of the same image into the tray.  Once loaded, the thumbnail can be selected from the tray by clicking it.  This will bring up the enlarged image into the workspace.  The user can select this enlarge image to bring up a series of buttons: zoom, rotate, contrast, brightness, flip, resize and close.  The only button that is functional without being in VR is the close button, which will close the button display and collapse the image. 
 The scene can be exited through the Play button on the Unity browser.  
