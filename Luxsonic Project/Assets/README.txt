README

Running the most recent approved build:
-Download Build #125 from the Unity Cloud Build interface for the project.
-Extract it to a directory on your local machine.
-Connect your Oculus Rift Headset and Touch Controllers to your computer
-Double click the application and select the "Play!" button.
-When running from an Oculus workstation, the application will pause if the
 headset is not placed on a user's head.
 
Running the project from the Unity Editor:
Note: The current version of the project requires Unity 5.5.1f1 to run
-The user must select the Scenes folder from Assets.
-Load the ID4_Demo scene. by double-clicking.
-Click the Play button on the top of the screen.
When the program runs, the user will enter the workspace. The workspace is set
up to the middle of the room so the user may have to adjust their position
relative to their computer.
If set up, the user will be able to see two panels in the workspace. The first
panel will contain the following buttons: Load, Quit and Minimize. The second
will contain: Rotate, Zoom, Resize, Invert, Brightness, Contrast, Close, Select All,
Deselect All, and Restore. The user must equip the Oculus headset and both
controllers in order to interact with the scene.
The user can interact with any of the buttons while using a pointing gesture and
running their finger through the button.  Simply move your fingers around the
controller until the virtual avatar makes a pointing gesture.  The minimize button
will minimize all the buttons in the scene, and the Load button will start the
file browser, where you can select a DICOM file to load into the Display, as
well as into the Tray in the lower right corner.  When three files have been loaded,
 a left scrolling button and a right scrolling button will appear to allow the
user to scroll through the images in the Display.
The user may select an image Thumbnail from the tray.  This will create a Copy of
that image in the workspace that the user can view. This Copy can be selected and
deselected using the pointing gesture. When the Copy is selected (a light green
outline appears on the boarder of the image) the user can use the point gesture
 to select buttons which manipulate the image. The resize button changes the size
 of the image, Brightness alters the brightness and Contrast changes the contrast
 of the image. The close button in the panel can be used by selecting an image
 and pressing close with the pointing gesture. At this point the close button
 will delete any Copy which is selected from the workspace.  The invert button
 will invert the colours of the image.  The user can use the 'select all' and
 'deselect all' buttons to select and deselect all copies in the workspace, respectively.
 The restore button will return the selected copies to the state they were when
 they were loaded in.
The scene can be exited at any time when the user removes the headset and clicks
the Play button in the Unity editor, or via the quit button if the application
is being run from the executable.