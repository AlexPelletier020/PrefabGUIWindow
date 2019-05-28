using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;
using System;

public class CreateNewBuffer : EditorWindow
{

    private const int MAX_BOTTOM_SPACE = 3;
    private const string EXTENSION = ".cs";
    static char[] invalidCharacters = {' ','.',',','/','\\','[',']','{','}',
                                       '(',')','*','%','?',';',':','=','+',
                                       '-','<','>','`','~','!','@','#','$',
                                       '^','&','"'};

    string bufferName = "";
    bool foldout = false;

    public void Init()
    {
        var window = ScriptableObject.CreateInstance<CreateNewBuffer>();                                                          // Create a window variable
        window.position = new Rect(Screen.width / 2 - 300, Screen.height / 2 - 500, 500, 500);
        window.ShowPopup();
    }

    public void OnGUI()
    {
        DrawTitle();
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);                                                                // Begin the vertical section
        foldout = DrawFoldout(foldout, "Default");
        if (foldout)
        {
            Draw();
        }
        EditorGUILayout.EndVertical();                                                                  // End the verticle section
        GUILayout.FlexibleSpace();
        DrawButtons();
        GUILayout.Space(MAX_BOTTOM_SPACE);
    }

    private void DrawTitle()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField("Create a new Buffer", FontStyle());
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

    void Draw()
    {
        DrawTextField();
    }

    void DrawTextField()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(20);
        bufferName = EditorGUILayout.TextField("Buffer Name", bufferName);
        EditorGUILayout.EndHorizontal();
    }

    void DrawButtons()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Cancel", GUILayout.MinWidth(100)))
        {
            if (EditorUtility.DisplayDialog("Cancel",
                            $"Are you sure you want to Cancel?",
                            "Yes", "No"))
            {
                this.Close();
            }
        }
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Create", GUILayout.MinWidth(100)))                                                                         // Add a button at the bottom of the window that displays Update All, and will be used to Update every Prefab
        {
            if (CheckInput())
            {
                return;
            }
            WriteNewScript();
            AssetDatabase.Refresh();
            this.Close();
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }

    private void WriteNewScript()
    {
        string filename = bufferName;

        filename = FilenameSanitization(filename);

        string path = EditorUtility.OpenFolderPanel("Create New Buffer", "Assets", filename);

        string fileContent = System.IO.File.ReadAllText("Assets/Editor/BufferImportWindow/BufferScript.txt");

        fileContent = fileContent.Replace("#SCRIPTNAME#", filename);
        fileContent = fileContent.Replace("#PROPERTIES#", CreateFileContent());

        if (path.Length != 0)
        {
            File.WriteAllText($"{path}/{filename}{EXTENSION}", fileContent);
        }
    }

    private string CreateFileContent()
    {
        string fileContent = "";
        fileContent += "string bufferName = \"" + bufferName + "\";\n";

        return fileContent;
    }

    private bool CheckInput()
    {
        if (bufferName.StartsWith("_", System.StringComparison.Ordinal))
        {
            EditorUtility.DisplayDialog("Create Buffer",                                // Display the amount of lights found.
            $"The buffer name cannoty start with an underscore \"_\". Please enter a valid name.",
            "OK");
            return true;
        }
        if (bufferName.Equals(""))
        {
            EditorUtility.DisplayDialog("Create Buffer",                                // Display the amount of lights found.
            $"There is required field that is empty. Please fill out all of the needed information.",
            "OK");
            return true;
        }
        return false;
    }

    private static string FilenameSanitization(string filename)
    {
        char[] invalidPathChars = Path.GetInvalidPathChars();

        foreach (char invalidchar in invalidCharacters)
        {
            filename = filename.Replace(invalidchar, '_');
        }

        return filename;
    }

    public bool DrawFoldout(bool boolValue, string label)
    {
        EditorGUILayout.BeginHorizontal();
        boolValue = EditorGUILayout.Foldout(boolValue, label);
        EditorGUILayout.EndHorizontal();
        return boolValue;
    }


}
