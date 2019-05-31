using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO;
using Silverback.EditorTools;

public class AddNewProperty : EditorWindow
{
    private const int MAX_SPACE = 3;
    readonly string[] listProperties = { "Select Type", "String", "Float", "Integer",  
                                            "Material", "Light", "Boolean" };

    static readonly char[] invalidCharacters = {' ','.',',','/','\\','[',']','{','}',                                           // character array which hold the invalid character options for a file name.
                                       '(',')','*','%','?',';',':','=','+',
                                       '-','<','>','`','~','!','@','#','$',
                                       '^','&','"'};

    private const string PROPERTY_FILE_PATH = "Assets/Editor/BufferImportWindow/Buffers/Settings/PropertyFile.txt";             // Constant which holds the path to the Property File.

    string propertyName = "";
    string propertyDescription = "";
    int propertyType = 0;
    int minValue = 0;
    int maxValue = 1;

    bool slider = false;
    bool descriptionToggle = false;

    public void Init()
    {
        // Init method used to create the add property window as a popup.

        var window = ScriptableObject.CreateInstance<AddNewProperty>();                                                          // Create a window variable
        window.position = new Rect(Screen.width / 2, Screen.height / 2 - 140, 550, 150);
        window.ShowPopup();
    }

    private void OnGUI()
    {
        // Method called by Untiy to draw the windoe

        GUILayout.Space(MAX_SPACE);
        DrawTitle();                                                                                                        // Call the draw title method to draw the title label.
        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        var scrollControlID = GUIUtility.GetControlID(FocusType.Passive);                               // Add Scrolling to the window
        var scrollState = GetScrollState(scrollControlID);

        using (var scrollView = new GUILayout.ScrollViewScope(scrollState.scrollPosition))              // Scrolling section of the window.
        {
            scrollState.scrollPosition = scrollView.scrollPosition;

            EditorGUILayout.BeginHorizontal();
            propertyName = DrawTextField(propertyName, "Property Name", 0);                                                     // Draw a text field to hold the property name.
            propertyType = DrawPopup(propertyType, listProperties, "", 20);                                                     // Draw popup to hoild the property types.
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            descriptionToggle = EditorGUILayout.Toggle("Description", descriptionToggle);
            EditorGUILayout.EndHorizontal();
            if (descriptionToggle)
            {
                EditorGUILayout.BeginHorizontal();
                propertyDescription = EditorGUILayout.TextArea(propertyDescription, GUILayout.MinHeight(50));
                EditorGUILayout.EndHorizontal();
            }
            if (propertyType == (int)PROPERTY_TYPE.FloatField || propertyType == (int)PROPERTY_TYPE.IntegerField)
            {
                EditorGUILayout.BeginHorizontal();
                slider = EditorGUILayout.Toggle("Slider", slider);
                EditorGUILayout.EndHorizontal();
                // Check if the type is a float or int slider.
                if (slider)
                {
                    EditorGUILayout.BeginHorizontal();
                    minValue = DrawIntField(minValue, "Minimum Value", 0);                                                          // If the type is a slider, draw an int field for the min
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    maxValue = DrawIntField(maxValue, "Maximum Value", 0);                                                          // and draw an int field for the max
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();
        }
        GUILayout.FlexibleSpace();
        DrawButtons();                                                                                                              // Call method to draw the buttonsat the bottom.
    }                                                                                          


    private static ScrollState GetScrollState(int scrollControlID) =>
        (ScrollState)GUIUtility.GetStateObject(typeof(ScrollState), scrollControlID);

    public int DrawPopup(int integerValue, string[] stringArray, string label, int spacePixel)
    {
        //Method used to draw the popup next to the name text field.

        GUILayout.Space(spacePixel);
        integerValue = EditorGUILayout.Popup(label, integerValue, stringArray, GUILayout.MaxWidth(150));
        return integerValue;
    }

    void DrawButtons()
    {
        // Method used to draw the bottom buttons such as Cancel and Add.

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Cancel", GUILayout.MinWidth(100)))                                                                // If cancel is clicked, a pop up will appear confirming the users choice.
        {
            this.Close();
        }
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Add", GUILayout.MinWidth(100)))                                                                    // Draw the add button that when clicked, will write the data to the property file.
        {
            if (CheckInput())
            {
                this.Focus();
                return;
            }
            Debug.Log("Add the property");
            propertyName = FileNameSanitization(propertyName);
            WritePropertyFile(propertyName, propertyType, propertyDescription);
            this.Close();
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }

    private void WritePropertyFile(String propName, int propType, string propDescription)
    {
        // Method used to write to the property file.

        Debug.Log("Writting to file");
        string filePath = PROPERTY_FILE_PATH;
        string fileContent = propName + "," + propType.ToString() + "," + propDescription;
        if(slider)
        {
            fileContent += "," + minValue + "," + maxValue;
        }
        File.WriteAllText(filePath, fileContent);
    }

    private void DrawTitle()
    {
        // Method used to draw the title for the window.

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField("Add Property", FontStyle());
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(MAX_SPACE);

        GUIStyle FontStyle()
        {
            GUIStyle s = new GUIStyle()
            {
                fontSize = 20,
            };
            return s;
        }
    }

    public string DrawTextField(string stringValue, string label, int spacePixel)
    {
        // Method called to draw the text fields

        GUILayout.Space(spacePixel);
        stringValue = EditorGUILayout.TextField(label, stringValue, GUILayout.MinWidth(300));
        return stringValue;
    }

    private bool CheckInput()
    {
        // Method used to check the user input.
        if (propertyName.Equals(""))
        {
            EditorUtility.DisplayDialog("Add Property",                                // Display the amount of lights found.
            $"The Property Name cannot be empty.",
            "OK");
            return true;
        }
        if (propertyType.Equals(0))
        {
            EditorUtility.DisplayDialog("Add Property",                                // Display the amount of lights found.
            $"The component type was not selected.",
            "OK");
            return true;
        }
        if (char.IsNumber(propertyName[0]))
        {
            EditorUtility.DisplayDialog("Add Property",                                // Display the amount of lights found.
            $"The Property name can't start with a number. Please enter a valid name.",
            "OK");
            return true;
        }
        return false;
    }

    int DrawIntField(int intValue, string label, int spacePixel)
    {
        // MEthod used to draw an int filed.

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(spacePixel);
        intValue = EditorGUILayout.IntField(label, intValue);
        EditorGUILayout.EndHorizontal();
        return intValue;
    }

    private static string FileNameSanitization(string filename)
    {
        // Method used to sanitize the filename variable.

        char[] invalidPathChars = Path.GetInvalidPathChars();

        foreach (char invalidchar in invalidCharacters)
        {
            filename = filename.Replace(invalidchar, '_');
        }

        return filename;
    }
}
