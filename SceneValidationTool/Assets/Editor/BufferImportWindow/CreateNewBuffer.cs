using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using Silverback.EditorTools;

public class CreateNewBuffer : EditorWindow
{
    AddNewProperty m_addProperty;

    private const string PROPERTY_FILE_PATH = "Assets/Editor/BufferImportWindow/Buffers/Settings/PropertyFile.txt";             // Constant which holds the path to the Property File.
    private const int MAX_BOTTOM_SPACE = 3;
    private const string EXTENSION = ".cs";
    private const string CLOSE_ICON = "closeIcon";

    static readonly char[] invalidCharacters = {' ','.',',','/','\\','[',']','{','}',                                           // character array which hold the invalid character options for a file name.
                                       '(',')','*','%','?',';',':','=','+',
                                       '-','<','>','`','~','!','@','#','$',
                                       '^','&','"'};
    string bufferName = "";
    string summaryTextArea = "";

    List<PropertyClass> list = new List<PropertyClass>();                                                                       // List of properties that get added by reading the Property File.

    bool foldout = true;                                                                                                        // Fold out for the Default section
    bool foldout2 = true;                                                                                                       // Fold out for the custom section.

    public void Init()
    {
        // Method used to create the window as a POPUP
        var window = ScriptableObject.CreateInstance<CreateNewBuffer>();                                                        
        window.position = new Rect(Screen.width / 2, Screen.height / 2 - 500, 500, 500);
        window.ShowPopup();
    }

    public void OnGUI()
    {
        // Method called by Unity to constantly draw the window.
        ReadPropertyFile();                                                                                                     // Call method to read the Property File.
        GUILayout.Space(MAX_BOTTOM_SPACE);
        DrawTitle();                                                                                                            // Call the method to draw the title for the window.
        GUILayout.Space(MAX_BOTTOM_SPACE);
        DrawAddButton();                                                                                                        // Call the method to draw the add button.
        GUILayout.Space(MAX_BOTTOM_SPACE);
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);                                                                    // Begin the vertical section
        foldout = DrawFoldout(foldout, "Default");                                                                              // Draw the first foldout for the default section, if opened call Draw()
        if (foldout)
        {
            DrawDefault();
        }
        EditorGUILayout.EndVertical();
        if (list.Count > 0)                                                                                                     // Check if the list has any custom properties in it, if not, then it's not drawn.
        {
            DrawCustomFoldout();
        }                                                                                                                       // Call the method to draw the custom foldout section.
        GUILayout.FlexibleSpace();
        DrawButtons();                                                                                                          // Call method to draw the cancel or accept buttons.
        GUILayout.Space(MAX_BOTTOM_SPACE);
    }

    private void DrawCustomFoldout()
    {
        // Method that draws the custom foldout section using the list and a switch using each properties type.
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);                                                                    
        foldout2 = DrawFoldout(foldout2, "Custom");                                                                             // Draw the foldout for custom.
        if (foldout2)
        {
            var scrollControlID = GUIUtility.GetControlID(FocusType.Passive);                               // Add Scrolling to the window
            var scrollState = GetScrollState(scrollControlID);

            using (var scrollView = new GUILayout.ScrollViewScope(scrollState.scrollPosition))              // Scrolling section of the window.
            {
                scrollState.scrollPosition = scrollView.scrollPosition;

                for (int i = 0; i < list.Count; i++)                                                                                 // Loop througbht the list of Property Objects
                {
                    EditorGUILayout.BeginHorizontal();
                    switch (list[i].type)                                                                                          // Switch on the type for each Property. Depending on the type, different methods are called to draw the sections.
                    {
                        case (int)PROPERTY_TYPE.FloatField:
                            list[i].value = DrawFloatField((float)list[i].value, list[i].name, 20);
                            break;
                        case (int)PROPERTY_TYPE.IntegerField:
                            list[i].value = DrawIntField((int)list[i].value, list[i].name, 20);
                            break;
                        case (int)PROPERTY_TYPE.StringField:
                            list[i].value = DrawTextField((string)list[i].value, list[i].name, 20);
                            break;
                        case (int)PROPERTY_TYPE.FloatSlider:
                            list[i].value = DrawFloatSlider((float)list[i].value, list[i].minValue, list[i].maxValue, list[i].name, 20);
                            break;
                        case (int)PROPERTY_TYPE.IntegerSlider:
                            list[i].value = DrawIntSlider((int)list[i].value, list[i].minValue, list[i].maxValue, list[i].name, 20);
                            break;
                        case (int)PROPERTY_TYPE.MaterialField:
                            DrawMaterialField(null, list[i].name, 20);
                            break;
                        case (int)PROPERTY_TYPE.LightField:
                            DrawLightField(null, list[i].name, 20);
                            break;
                        case (int)PROPERTY_TYPE.BooleanField:
                            list[i].value = DrawToggle((bool)list[i].value, list[i].name, 20);
                            break;
                    }
                    DrawCloseButton(i);
                    EditorGUILayout.EndHorizontal();
                }
            }

        }
        EditorGUILayout.EndVertical();
    }

    private static ScrollState GetScrollState(int scrollControlID) =>
        (ScrollState)GUIUtility.GetStateObject(typeof(ScrollState), scrollControlID);

    private void DrawAddButton()
    {
        // Method to draw the add property button.
        if (m_addProperty == null)
        {
            m_addProperty = new AddNewProperty();                                                                               // if addProperty is null, then instantiate it.
        }
        if (GUILayout.Button("Add Property", GUILayout.MinWidth(100)))
        {
            m_addProperty.Init();                                                                                               // If the button is clicked, then the Init method of addProperty is called.
        }
    }

    private void DrawTitle()
    {
        // Method used to draw the title. with a custom font size create by the custom GUIStyle.
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField("Create a new Buffer", FontStyle());                                                         // Create the label and use the custom GUIStyle. Also added two flexible spaces to center the title.
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(MAX_BOTTOM_SPACE);

        GUIStyle FontStyle()
        {
            GUIStyle s = new GUIStyle()
            {
                fontSize = 20,
            };
            return s;
        }
    }

    void DrawDefault()
    {
        // Method to draw the default section of the window.
        bufferName = DrawTextField(bufferName, "Buffer Name", 20);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Summary:");
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal(); 
        summaryTextArea = EditorGUILayout.TextArea(summaryTextArea, GUILayout.MinHeight(50));
        EditorGUILayout.EndHorizontal();
    }

    void DrawButtons()
    {
        // Method called to Draw the bottom buttons such as Cancel or Create.

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Cancel", GUILayout.MinWidth(100)))                                                                // If cancel is clicked, then a popup will appear asking if the user is sure of the choice.
        {                                                                                                                       // If yes is selected, then the window will be closed.
            if (EditorUtility.DisplayDialog("Cancel",
                            $"Are you sure you want to Cancel?",
                            "Yes", "No"))
            {
                this.Close();
            }
        }
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Create", GUILayout.MinWidth(100)))                                                                // Add a button at the bottom of the window that displays Create, and will be used to Create a new BUFFER
        {
            if (CheckInput())                                                                                                   // Call CheckInput to validate the filename
            {
                return;
            }
            WriteNewScript();                                                                                                   // Call the method to write a new script.
            AssetDatabase.Refresh();                                                                                            // Refresh the project folders inside Unity.
            this.Close();                                                                                                       // Close the window.
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }

    private void ReadPropertyFile()
    {
        //Method which is used to read tyhe property file.
        string fileContent = System.IO.File.ReadAllText(PROPERTY_FILE_PATH);                                                    // Reads the content of the file.
        if (!fileContent.Contains(","))                                                                                         // Checks if the file contains a period ",". If not, then leaves the method.
        {
            return;
        }
        string[] values = fileContent.Split(',');                                                                               // If it does have one, than the file contains data. Split on the periods "," creating a string array.

        foreach(PropertyClass property in list)                                                                                 // Check for same name property.
        {
            if (values[0].Equals(property.name))
            {
                return;
            }
        }
        PropertyClass newProperty = new PropertyClass();                                                                        // Create a new Property class and set the values to the contents of the file.
        newProperty.name = values[0];
        newProperty.type = System.Convert.ToInt32(values[1]);
        newProperty.value = SetValue(newProperty.type);
        newProperty.description = values[2];
        if (values.Length > 3)
        {
            newProperty.type += 5;
            newProperty.minValue = System.Convert.ToInt32(values[3]);
            newProperty.maxValue = System.Convert.ToInt32(values[4]);
        }
        list.Add(newProperty);                                                                                                  // Add the new property to the list.
        File.Create(PROPERTY_FILE_PATH);                                                                                        // Used to overwrite the property file so the data is not copied twice.
    }

    private object SetValue(int type)
    {
        object value = null;
        switch (type)                                                                                          // Switch on the type for each Property. Depending on the type, different methods are called to draw the sections.
        {
            case (int)PROPERTY_TYPE.FloatField:
                value = 0f;
                break;
            case (int)PROPERTY_TYPE.IntegerField:
                value = 0;
                break;
            case (int)PROPERTY_TYPE.StringField:
                value = "";
                break;
            case (int)PROPERTY_TYPE.FloatSlider:
                value = 0f;
                break;
            case (int)PROPERTY_TYPE.IntegerSlider:
                value = 0;
                break;
            case (int)PROPERTY_TYPE.BooleanField:
                value = false;
                break;
        }
        return value;
    }

    private void WriteNewScript()
    {
        // Method used to write the new script when the buffer is created.

        string filename = bufferName;                                                                               // Set the file name to the bufer name.

        filename = FileNameSanitization(filename);                                                                  // Sanitize the file name.

        string path = EditorUtility.OpenFolderPanel("Create New Buffer", "Assets", filename);                       // Open the file explorer to select a folder.

        string fileContent = System.IO.File.ReadAllText("Assets/Editor/BufferImportWindow/BufferScript.txt");       // Read the content of the placeholder text file.

        fileContent = fileContent.Replace("#SCRIPTNAME#", filename);                                                // Replace some text from the placeholder text file to specified variables.
        fileContent = fileContent.Replace("#PROPERTIES#", CreateFileContent());
        fileContent = fileContent.Replace("#SUMMARY#", CreateSummaryContent());

        if (path.Length != 0)                                                                                       // Check if poath is valid.
        {
            File.WriteAllText($"{path}/{filename}{EXTENSION}", fileContent);                                        // If so, write the new script file with the file content.
        }
    }

    private string CreateSummaryContent()
    {
        string summary = "";

        if (summaryTextArea.Equals(""))
        {
            return summary;
        }

        summaryTextArea = summaryTextArea.Replace("\n", "\n\t");

        summary += "/* <summary>\n\t" + summaryTextArea +"\n\t</summary> */";

        return summary;
    }

    private string CreateFileContent()
    {
        // Method that is used to create the file content by using the list and based on the property type.

        string fileContent = "";
        string description = "";
        fileContent += "#region Default Properties\n";
        fileContent += "\tstring bufferName = \"" + bufferName + "\";\n";
        fileContent += "\t#endregion\n";

        if (list.Count > 0)
        {
            list = list.OrderBy(property => property.type).ToList();
            fileContent += "\n\t#region Custom Properties";
            foreach (var property in list)
            {
                description = property.description;
                if(description != "")
                {
                    description = description.Replace("\n", "\n\t");
                    fileContent += "\n\t/* <summary>\n\t" + description + "\n\t</summary> */";
                }
                fileContent += "\n\t[SerializeField]\n";
                switch (property.type)
                {
                    case (int)PROPERTY_TYPE.FloatField:
                        fileContent += "\tfloat " + property.name + " = " + property.value + "f;\n";
                        break;
                    case (int)PROPERTY_TYPE.IntegerField:
                        fileContent += "\tint " + property.name + " = " + property.value + ";\n";
                        break;
                    case (int)PROPERTY_TYPE.StringField:
                        fileContent += "\tstring " + property.name + " = \"" + property.value + "\";\n";
                        break;
                    case (int)PROPERTY_TYPE.FloatSlider:
                        fileContent += "\tfloat " + property.name + " = " + property.value + "f;\n";
                        break;
                    case (int)PROPERTY_TYPE.IntegerSlider:
                        fileContent += "\tint " + property.name + " = " + property.value + ";\n";
                        break;
                    case (int)PROPERTY_TYPE.MaterialField:
                        fileContent += "\tMaterial " + property.name + ";\n";
                        break;
                    case (int)PROPERTY_TYPE.LightField:
                        fileContent += "\tLight " + property.name + ";\n";
                        break;
                    case (int)PROPERTY_TYPE.BooleanField:
                        fileContent += "\tbool " + property.name + " = " + (property.value.ToString().ToLower()) + ";\n";
                        break;
                }
            }
            fileContent += "\t#endregion\n";

            fileContent += "\n\t#region Methods";
            foreach(PropertyClass property in list)
            {
                string setter = "value";
                if (property.name.Equals(setter))
                {
                    setter = "val";
                }
                switch (property.type)
                {
                    case (int)PROPERTY_TYPE.FloatField:
                        fileContent += "\n\tpublic float Get" + property.name + "()\n\t{\n\t\treturn " + property.name + ";\n\t}";
                        fileContent += "\n\tpublic void Set" + property.name + "(float " + setter + ")\n\t{\n\t\t" + property.name + "= " + setter + ";\n\t}\n";
                        break;
                    case (int)PROPERTY_TYPE.IntegerField:
                        fileContent += "\n\tpublic int Get" + property.name + "()\n\t{\n\t\treturn " + property.name + ";\n\t}\n";
                        fileContent += "\n\tpublic void Set" + property.name + "(int " + setter + ")\n\t{\n\t\t" + property.name + "= " + setter + ";\n\t}\n";
                        break;
                    case (int)PROPERTY_TYPE.StringField:
                        fileContent += "\n\tpublic string Get" + property.name + "()\n\t{\n\t\treturn " + property.name + ";\n\t}\n";
                        fileContent += "\n\tpublic void Set" + property.name + "(string " + setter + ")\n\t{\n\t\t" + property.name + "= " + setter + ";\n\t}\n";
                        break;
                    case (int)PROPERTY_TYPE.FloatSlider:
                        fileContent += "\n\tpublic float Get" + property.name + "()\n\t{\n\t\treturn " + property.name + ";\n\t}\n";
                        fileContent += "\n\tpublic void Set" + property.name + "(float " + setter + ")\n\t{\n\t\t" + property.name + "= " + setter + ";\n\t}\n";
                        break;
                    case (int)PROPERTY_TYPE.IntegerSlider:
                        fileContent += "\n\tpublic int Get" + property.name + "()\n\t{\n\t\treturn " + property.name + ";\n\t}\n";
                        fileContent += "\n\tpublic void Set" + property.name + "(int " + setter + ")\n\t{\n\t\t" + property.name + "= " + setter + ";\n\t}\n";
                        break;
                    case (int)PROPERTY_TYPE.MaterialField:
                        fileContent += "\n\tpublic Material Get" + property.name + "()\n\t{\n\t\treturn " + property.name + ";\n\t}\n";
                        fileContent += "\n\tpublic void Set" + property.name + "(Material " + setter + ")\n\t{\n\t\t" + property.name + "= " + setter + ";\n\t}\n";
                        break;
                    case (int)PROPERTY_TYPE.LightField:
                        fileContent += "\n\tpublic Light Get" + property.name + "()\n\t{\n\t\treturn " + property.name + ";\n\t}\n";
                        fileContent += "\n\tpublic void Set" + property.name + "(Light " + setter + ")\n\t{\n\t\t" + property.name + "= " + setter + ";\n\t}\n";
                        break;
                    case (int)PROPERTY_TYPE.BooleanField:
                        fileContent += "\n\tpublic bool Get" + property.name + "()\n\t{\n\t\treturn " + property.name + ";\n\t}\n";
                        fileContent += "\n\tpublic void Set" + property.name + "(bool " + setter + ")\n\t{\n\t\t" + property.name + "= " + setter + ";\n\t}\n";
                        break;
                }
            }
            fileContent += "\t#endregion";
        }
        return fileContent;
    }

    private bool CheckInput()
    {
        // Method used to check the user input.

        if (bufferName.Equals(""))
        {
            EditorUtility.DisplayDialog("Create Buffer",                                // Display the amount of lights found.
            $"There is required field that is empty. Please fill out all of the needed information.",
            "OK");
            return true;
        }
        if (bufferName.StartsWith("_", System.StringComparison.Ordinal))
        {
            EditorUtility.DisplayDialog("Create Buffer",                                // Display the amount of lights found.
            $"The buffer name can not start with an underscore \"_\". Please enter a valid name.",
            "OK");
            return true;
        }
        if (char.IsNumber(bufferName[0]))
        {
            EditorUtility.DisplayDialog("Create Buffer",                                // Display the amount of lights found.
            $"The buffer name can not start with a number. Please enter a valid name.",
            "OK");
            return true;
        }
        return false;
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

    void DrawCloseButton(int listIndex)
    {
        var rectIcon = GUILayoutUtility.GetRect(10f, 10f, GetBtnStyleSelected());                                       // Rectangle for the check or cross icon

        var tex = Resources.Load(CLOSE_ICON, typeof(Texture2D)) as Texture2D;                                           // Add a texture of the Icon

        if (GUI.Button(rectIcon, tex, GetBtnStyleSelected()))
        {
            list.RemoveAt(listIndex);
            return;                                                                               // If the gear icon is clicked, set the window data member to 1 to go to the settings window.
        }

        GUIStyle GetBtnStyleSelected()                                                                  // Style for sections not selected
        {
            GUIStyle s = new GUIStyle
            {
                margin = new RectOffset(0, 5, 7, 0),
                fixedWidth = 20
            };

            return s;
        }
    }

    // Method used to draw the different UI sections.
    bool DrawFoldout(bool boolValue, string label)
    {
        EditorGUILayout.BeginHorizontal();
        boolValue = EditorGUILayout.Foldout(boolValue, label);
        EditorGUILayout.EndHorizontal();
        return boolValue;
    }

    string DrawTextField(String stringValue, string label, int spacePixel)
    {
        GUILayout.Space(spacePixel);
        stringValue = EditorGUILayout.TextField(label, stringValue);
        return stringValue;
    }

    float DrawFloatField(float floatValue, string label, int spacePixel)
    {
        GUILayout.Space(spacePixel);
        floatValue = EditorGUILayout.FloatField(label, floatValue);
        return floatValue;
    }

    int DrawIntField(int intValue, string label, int spacePixel)
    {
        GUILayout.Space(spacePixel);
        intValue = EditorGUILayout.IntField(label, intValue);
        return intValue;
    }

    float DrawFloatSlider(float floatValue, float floatMinValue, float floatMaxValue, string label, int spacePixel)
    {
        GUILayout.Space(spacePixel);
        floatValue = EditorGUILayout.Slider(label, floatValue, floatMinValue, floatMaxValue);
        return floatValue;
    }

    int DrawIntSlider(int intValue, int minValue, int maxValue, string label, int spacePixel)
    {
        GUILayout.Space(spacePixel);
        intValue = EditorGUILayout.IntSlider(label, intValue, minValue, maxValue);
        return intValue;
    }

    Material DrawMaterialField(Material mat, string label, int spacePixel)
    {
        GUILayout.Space(spacePixel);
        mat = (Material)EditorGUILayout.ObjectField(label, mat, typeof(Material), true);
        return mat;
    }

    Light DrawLightField(Light light, string label, int spacePixel)
    {
        GUILayout.Space(spacePixel);
        light = (Light)EditorGUILayout.ObjectField(label, light, typeof(Light), true);
        return light;
    }

    bool DrawToggle(bool boolValue, string label, int spacePixel)
    {
        GUILayout.Space(spacePixel);
        boolValue = EditorGUILayout.Toggle(label, boolValue);
        return boolValue;
    }
}
