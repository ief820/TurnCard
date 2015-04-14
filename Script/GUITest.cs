

using UnityEngine;
using System.Collections;

public class GUITest : MonoBehaviour {

	public Texture2D icon;

	public Texture2D controlTexture;

    private string textFieldString = "text field";

    private bool toggleBool = true;

    private int toolbarInt = 0;
    private string[] toolbarStrings = { "Toolbar1", "Toolbar2", "Toolbar3" };

    private int selectionGridInt = 0;
    private string[] selectionStrings = { "Grid 1", "Grid 2", "Grid 3", "Grid 4", "Grid5"};

    private float hSliderValue = 0.0f;

    private float hScrollbarValue;

    private Vector2 scrollViewVector = Vector2.zero;
    private string innerText = "I am inside the ScrollView";

    private Rect windowRect = new Rect(300, 180, 120, 50);

	void OnGUI () {

        GUI.Box(new Rect(0, 0, 100, 50), "Top-left");
        GUI.Box(new Rect(Screen.width - 100, 0, 100, 50), "Top-right");
        GUI.Box(new Rect(0, Screen.height - 50, 100, 50), "Bottom-left");
        GUI.Box(new Rect(Screen.width - 100, Screen.height - 50, 100, 50), "Bottom-right");

		//Make a background box
        GUI.Box(new Rect(130,10,100,90), "Loader Menu");
		
        // Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
        if(GUI.Button(new Rect(140,40,80,20), "Level 1")) {
            Application.LoadLevel(1);
        }
		
        // Make the second button.
        if(GUI.Button(new Rect(140,70,80,20), "Level 2")) {
            Application.LoadLevel(2);
        }

        if (Time.time % 2 < 1) {
            if (GUI.Button (new Rect (100,110,160,20), "Meet the flashing button")) 
            {
                print ("You clicked me!");
            }
        }

        GUI.Label(new Rect(0, 200, 100, 50), "This is the text string for a Label Control");


        GUI.Label(new Rect(0, 0, 100, 50), controlTexture);


        if (GUI.Button(new Rect(130, 140, 100, 50), icon))
        {
            print("you clicked the icon");
        }

        if (GUI.Button(new Rect(0, 70, 100, 20), "This is text"))
        {
            print("you clicked the text button");
        }


        // This line feeds "This is the tooltip" into GUI.tooltip
        GUI.Button(new Rect(0, 100, 100, 20), new GUIContent("Click me", "This is the tooltip"));

        // This line reads and displays the contents of GUI.tooltip
        GUI.Label(new Rect(0, 120, 100, 20), GUI.tooltip);


        GUI.Box(new Rect(130, 200, 100, 50), new GUIContent("This is text", icon));

        if (GUI.RepeatButton(new Rect(0, 135, 100, 30), "RepeatButton"))
        {
            print(" holding " + Time.time);
        }



        textFieldString = GUI.TextField(new Rect(0, 260, 100, 30), textFieldString);
        {
            //print(" string " + textFieldString);
        }

        toggleBool = GUI.Toggle(new Rect(0, 300, 100, 30), toggleBool, "Toggle");


        toolbarInt = GUI.Toolbar(new Rect(0, 350, 250, 30), toolbarInt, toolbarStrings);

        if (GUI.changed)
        {
            print(" GUI changed" + toolbarInt);
            print("textFieldString " + textFieldString);
        }

        selectionGridInt = GUI.SelectionGrid(new Rect(0, 400, 150, 60), selectionGridInt, selectionStrings, 2);

        hSliderValue = GUI.HorizontalSlider(new Rect(300, 10, 100, 30), hSliderValue, 0.0f, 10.0f);

        hSliderValue = GUI.VerticalSlider(new Rect(270, 10, 30, 100), hSliderValue, 0.0f, 10.0f);

        hScrollbarValue = GUI.HorizontalScrollbar(new Rect(300, 35, 100, 30), hScrollbarValue, 1.0f, 0.0f, 10.0f);


        // Begin the ScrollView
        scrollViewVector = GUI.BeginScrollView(new Rect(300, 60, 100, 100), scrollViewVector, new Rect(0, 0, 400, 400));

        // Put something inside the ScrollView
        innerText = GUI.TextArea(new Rect(0, 0, 400, 400), innerText);

        // End the ScrollView
        GUI.EndScrollView();


        windowRect = GUI.Window (0, windowRect, WindowFunction, "My Window");
    
    
    
    }

    void WindowFunction(int windowID)
    {
        // Draw any Controls inside the window here
    }
	
}





