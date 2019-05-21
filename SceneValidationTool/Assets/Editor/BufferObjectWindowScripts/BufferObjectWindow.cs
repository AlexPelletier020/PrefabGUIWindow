using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.SceneManagement;
using Silverback.EditorTools;
using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector;
using UnityEditor.IMGUI.Controls;

public class BufferObjectWindow : EditorWindow
{
    private const string CrossIconPath = "cross";
    private const string CheckIconPath = "check";
    private const string QuestionIconPath = "warning";
    private const string BlueSquare = "blueSquare";
    private const string PrefabIcon = "PrefabIcon";

    private int choice = 0;                                                                             // Drop down menu integer user choice for sorting.
    private string[] choices = new string[] { "Alphabetical", "Unalphabetical" };                       // List of items in drop down menu.

    private static GameObject selectedGameObject = null;                                                       // Game Object selected by user.

    List<GameObject> listOfUpdateObjects = new List<GameObject>();                                      // List of prefabs that are being updated when the button update all is pressed.

    static String searchInput = "";                                                                            // Search input string variable used to define the search.

    [MenuItem("Buffered/Update Buffer Objects")]
    static void Init()                                                                                  //Initialize the window creation for Editor Window
    {
        var window = GetWindow<BufferObjectWindow>();                                                   // Create a window variable

        window.Show();                                                                                  // Show the window
    }

    [SerializeField]
    static AutocompleteSearchField.AutocompleteSearchField searchField;                                        // Create a searchField bar.

    void OnEnable()
    {
        if (searchField == null) searchField = new AutocompleteSearchField.AutocompleteSearchField();
        searchField.onInputChanged = OnInputChanged;
    }

    void OnInputChanged(string searchString)
    {
        searchField.ClearResults();
        if (!string.IsNullOrEmpty(searchString))
        {
            searchInput = searchString.ToUpper();                                                       // Set the string search input equal to the user input case insensitive by making all upper case.
        }
        else
        {
            searchInput = "";                                                                           // Set string search input to blank if nothing is in the bar.
        }
    }

    void YourCallback()
    {
        searchField.searchString = selectedGameObject.name; 
        searchInput = selectedGameObject.name.ToUpper();
    }

    [MenuItem("Assets/Buffer/Check For Updates", priority = 1)]
    private static void SetupPrefab()
    {
        GameObject obj = (UnityEngine.GameObject)Selection.activeObject;
        string objPath = AssetDatabase.GetAssetPath(obj);
        if (objPath.Contains(".prefab"))
        {
            Init();
            searchField.searchString = obj.name;
            searchInput = obj.name.ToUpper();
        }
    }

    void OnGUI()                                                                                        // This is where all the GUI elements for the window is located for the window
    {

        var e = Event.current;                                                                          // Get Key events from the user including mouse keys

        CheckForRightClick(e);                                                                          // Check right click

        listOfUpdateObjects.Clear();                                                                    // Clear the list being drawn so it's not repeated.

        List<GameObject> listOfProjectPrefabs = CreateList();                                           // Repopulate the list by calling CreateList method.

        if (choice == 0)                                                                                // Sort the list alphabetically
        {
            listOfProjectPrefabs = listOfProjectPrefabs.OrderBy(obj => obj.name).ToList();
        }
        else                                                                                            // Or sort the list Unalphabetically
        {
            listOfProjectPrefabs = listOfProjectPrefabs.OrderByDescending(obj => obj.name).ToList();
        }

        EditorGUILayout.BeginVertical();

        using (new GUILayout.HorizontalScope(EditorStyles.helpBox))                                     // Put each Prefab in the list inside of it's own horizontal help box
        {
            searchField.OnGUI();
            choice = EditorGUILayout.Popup(choice, choices, GUILayout.MaxWidth(200));
        }

        var scrollControlID = GUIUtility.GetControlID(FocusType.Passive);                               // Add Scrolling to the window
        var scrollState = (ScrollState)GUIUtility.GetStateObject(typeof(ScrollState), scrollControlID);

        using (var scrollView = new GUILayout.ScrollViewScope(scrollState.scrollPosition))              // Scrolling section of the window.
        {
            scrollState.scrollPosition = scrollView.scrollPosition;

            DrawList(listOfProjectPrefabs);
        }


        GUILayout.FlexibleSpace();                                                                      // Add some white space until the bottom of the window

        using (new GUILayout.HorizontalScope(EditorStyles.helpBox))                                     // Put each Prefab in the list inside of it's own horizontal help box
        {
            if (selectedGameObject != null)
            {
                DrawText("Enter Problem Here For: " + selectedGameObject.name);
            }
        }

        using (new GUILayout.HorizontalScope(EditorStyles.helpBox))                                     // Put each Prefab in the list inside of it's own horizontal help box
        {
            if (selectedGameObject != null)
            {
                if (GUILayout.Button("Update"))                                                         // Add a button at the bottom of the window that displays Update All, and will be used to Update every Prefab
                {
                    Debug.Log(AssetDatabase.GetAssetPath(selectedGameObject));
                }
            }
            if (GUILayout.Button("Update All"))                                                         // Add a button at the bottom of the window that displays Update All, and will be used to Update every Prefab
            {
                foreach (GameObject obj in listOfUpdateObjects)
                {
                    Debug.Log(AssetDatabase.GetAssetPath(obj) + "\n");
                }
            }
        }

        EditorGUILayout.EndVertical();

        if (e.type == EventType.MouseDown && e.button == 0)                                             // Check for left mouse click if off the screen, or trying to unselect.
        {
            selectedGameObject = null;
        }
    }

    private List<GameObject> CreateList()
    {
        List<GameObject> listOfProjectPrefabs = new List<GameObject>();                                 // ArrayList which will hold only the prfabs in the project and not the ones in the scene                                         

        ArrayList allObjects = new ArrayList();                                                         // GameObject array which contains all of the Game Objects, both in the scene and in the project

        String[] allPaths = System.IO.Directory.GetFiles("Assets/", "*.prefab", System.IO.SearchOption.AllDirectories);

        foreach (String path in allPaths)
        {
            allObjects.Add(AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)));
        }

        foreach (GameObject obj in allObjects)                                                          // Loop through the array of all objects and check if they have the component required, 
        {                                                                                               // and check that the gameObjects are not located in the current scene

            if (obj.scene.name != SceneManager.GetActiveScene().name && obj.name.ToUpper().Contains(searchInput))                                   // Check if the gameObject is not located in the current scene.
            {
                listOfProjectPrefabs.Add(obj);                                                          // If both are true, then add the gameObject to the ArrayList of project prefabs
            }
        }

        return listOfProjectPrefabs;
    }

    private void DrawList(List<GameObject> list)
    {
        foreach (GameObject obj in list)                                                                // Loop through the list of project Prefabs to create the GUI portion of the window.
        {
            bool selected = false || obj.Equals(selectedGameObject);

            using (new GUILayout.HorizontalScope(EditorStyles.helpBox))                                 // Put each Prefab in the list inside of it's own horizontal help box
            {
                if (selected)
                {
                    EditorGUILayout.BeginHorizontal(GetBtnStyleSelected());                             // If button selected, use style to set background blue
                }
                else
                {
                    EditorGUILayout.BeginHorizontal();                                                  // If not selected, use default style
                }

                GUIStyle GetBtnStyleSelected()                                                          // Method for styling selected buttons to have a blue background
                {
                    GUIStyle s = new GUIStyle();
                    s.normal.background = Resources.Load(BlueSquare, typeof(Texture2D)) as Texture2D;

                    return s;
                }

                var rectPrefab = GUILayoutUtility.GetRect(15f, 15f, GUILayout.ExpandWidth(false));      // Rectangle for the prefab icon.

                DrawIcon(rectPrefab, PrefabIcon);                                                       // Draw the prefab icon
                DrawButton(obj, selected);                                                              // Draw the button with the obj name as a label.

                var rectIcon = GUILayoutUtility.GetRect(15f, 15f, GUILayout.ExpandWidth(false));        // Rectangle for the check or cross icon

                switch (CheckIfValid(obj))                                                              // Check if the obj is valid or not, then draws the coresponding icon
                {
                    case (int)ValidationResult.Success:
                        DrawIcon(rectIcon, CheckIconPath);                                              // Draw check mark icon
                        break;
                    case (int)ValidationResult.Fail:
                        DrawIcon(rectIcon, CrossIconPath);                                              // Draw cross Icon
                        break;
                }

                EditorGUILayout.EndHorizontal();
            }
        }
    }

    private void CheckForRightClick(Event e)                                                            // Method which checks if the right mouse key is hit. If so, then make a menu for the right click menu
    {
        if (e.type == EventType.MouseDown && e.button == 1)
        {
            GenericMenu menu = new GenericMenu();

            menu.AddItem(new GUIContent("Search For:"), false, YourCallback);                            // Add menu item to right click menu, which calls the method YourCallBack.
            menu.ShowAsContext();

            e.Use();
        }
    }

    private int CheckIfValid(GameObject obj)                                                            // Currently checks if the object has a Sprite Renderer
    {
        if (obj.GetComponent<SpriteRenderer>() != null)
        {
            return (int)ValidationResult.Success;                                                       // If has a Sprite renderer, return the success integer value of success using enum
        }
        listOfUpdateObjects.Add(obj);
        return (int)ValidationResult.Fail;                                                              // Or else, return the false integer value of the fail enum
    }


    private static void DrawIcon(Rect position, string iconPath)                                        // Draw the Icons in the created rectangle
    {
        var tex = Resources.Load(iconPath, typeof(Texture2D)) as Texture2D;                             // Add a texture of the Icon

        var style = new GUIStyle(EditorStyles.whiteLargeLabel)                                          // Set up the style for the icon, by making it a little more padded.
        {
            padding = new RectOffset(0, 0, 2, 0)

        };
        EditorGUI.LabelField(position, new GUIContent(tex), style);                                     // Finally, draw the texured Icon.
    }

    private void DrawText(String text)
    {
        var labelStyle = new GUIStyle(EditorStyles.label)                                               // Create a label style which will warp the words when they get collapse
        {
            wordWrap = true
        };

        EditorGUILayout.LabelField(text, labelStyle);                                                   // Create the text label that will hold the name of the Prefab and using the label style previously created

        GUILayout.FlexibleSpace();                                                                      // Add some dynamic white space to the right of the text label to fit between the label and the button
    }

    private void DrawButton(GameObject obj, bool selected)
    {
        if (selected)
        {
            if (GUILayout.Button(obj.name, GetBtnStyleSelected()))                                      // Add a button at the bottom of the window that displays Update All, and will be used to Update every Prefab
            {                                                                                           // by looping through the ArrayList of Project Prefabs.
                selectedGameObject = obj;
            }
            return;
        }

        if (GUILayout.Button(obj.name, GetBtnStyleNotSelected()))                                       // Add a button at the bottom of the window that displays Update All, and will be used to Update every Prefab
        {                                                                                               // by looping through the ArrayList of Project Prefabs.
            selectedGameObject = obj;

        }

        GUIStyle GetBtnStyleNotSelected()                                                               // Style for sections not selected
        {
            GUIStyle s = new GUIStyle
            {
                margin = new RectOffset(5, 0, 0, 0),
                wordWrap = true,
            };

            //s.normal.textColor = Color.black;                                                           // Set text color to black when not selected

            return s;
        }

        GUIStyle GetBtnStyleSelected()                                                                  // Style for selected section.
        {
            GUIStyle s = new GUIStyle
            {
                margin = new RectOffset(5, 0, 0, 0),
                wordWrap = true,
            };

            s.normal.textColor = Color.white;                                                           // Set text color to white when selected.
            return s;
        }
    }
}