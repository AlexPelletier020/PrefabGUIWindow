using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.SceneManagement;
using Silverback.EditorTools;
using System;
using System.Collections.Generic;

public class BufferObjectWindow : EditorWindow
{
    private const string CROSS_ICON_PATH = "cross";
    private const string CHECK_ICON_PATH = "check";
    private const string BLUE_SQUARE = "blueSquare";
    private const string PREFAB_ICON = "PrefabIcon";
    private const string FILE_EXTENSION = ".prefab";

    private int choice = 0;                                                                             // Drop down menu integer user choice for sorting.
    private string[] choices = new string[] { "Alphabetical", "Unalphabetical" };                       // List of items in drop down menu.    
    private static String searchInput = "";                                                             // Search input string variable used to define the search.
    private static GameObject selectedGameObject = null;                                                // Game Object selected by user.
    private readonly List<GameObject> listOfUpdateObjects = new List<GameObject>();                     // List of prefabs that are being updated when the button update all is pressed.
    private static AutocompleteSearchField.AutocompleteSearchField searchField;                         // Create a searchField bar.

    private ListOfPrefabs prefabList = new ListOfPrefabs();                                             // Object linked to the list of prefabs.

    [MenuItem("Buffered/Update Buffer Objects")]
    static void Init()                                                                                  //Initialize the window creation for Editor Window
    {
        var window = GetWindow<BufferObjectWindow>();                                                   // Create a window variable

        window.Show();                                                                                  // Show the window
    }

    void OnEnable()
    {
        if (searchField == null)
        {
            searchField = new AutocompleteSearchField.AutocompleteSearchField();
        }
        searchField.onInputChanged = OnInputChanged;
    }

    void OnInputChanged(string searchString)
    {
        searchField.ClearResults();
        if (!string.IsNullOrEmpty(searchString))
        {
            searchInput = searchString.ToLower();                                                       // Set the string search input equal to the user input case insensitive by making all upper case.
            return;
        }
        searchInput = "";                                                                               // Set string search input to blank if nothing is in the bar
    }

    void RightClick()
    {
        Debug.Log(AssetDatabase.GetAssetPath(selectedGameObject) + "\n");
    }

    void OnGUI()                                                                                        // This is where all the GUI elements for the window is located for the window
    {
        var e = Event.current;                                                                          // Get Key events from the user including mouse keys

        CheckForRightClick(e);                                                                          // Check right click

        listOfUpdateObjects.Clear();                                                                    // Clear the list being drawn so it's not repeated.

        prefabList.PopulateList(searchInput, choice);                                                   // Populate the list inside the ListOfPrefabs class

        DrawEntireUI();                                                                                 // This is the header method for drawing the Window

        LeftMouseClick(e);                                                                              // Check for mouse left click.
    }

    private void DrawEntireUI()                                                                         // Main UI header method to draw the UI for the window
    {
        EditorGUILayout.BeginVertical();                                                                // Begin the vertical section

        DrawSearchBar();                                                                                // Draw the search bar using the AutocompleteSearchField class 

        DrawScrollingSection();                                                                         // Draw the section of th window which has a scrolling portion

        DrawTextBox();                                                                                  // Draw the text box at the bottom of the list of prefabs to hold information

        DrawUpdateButtons();                                                                            // Draw the update buttons.

        EditorGUILayout.EndVertical();                                                                  // End the verticle section
    }

    private static void LeftMouseClick(Event e)
    {
        if (e.type == EventType.MouseDown && e.button == 0)                                             // Check for left mouse click if off the screen, or trying to unselect.
        {
            selectedGameObject = null;
            Init();
        }
    }

    private void DrawUpdateButtons()
    {
        using (new GUILayout.HorizontalScope(EditorStyles.helpBox))                                     // Put each Prefab in the list inside of it's own horizontal help box
        {
            DrawButtons();
        }
    }

    private void DrawButtons()
    {
        if (selectedGameObject != null)
        {
            if (GUILayout.Button("Update"))                                                             // Add a button at the bottom of the window that displays Update All, and will be used to Update every Prefab
            {
                Debug.Log(AssetDatabase.GetAssetPath(selectedGameObject));
            }
        }
        if (GUILayout.Button("Update All"))                                                             // Add a button at the bottom of the window that displays Update All, and will be used to Update every Prefab
        {
            foreach (GameObject obj in listOfUpdateObjects)
            {
                Debug.Log(AssetDatabase.GetAssetPath(obj) + "\n");
            }
        }
    }

    private void DrawTextBox()
    {
        GUILayout.FlexibleSpace();                                                                      // Add some white space until the bottom of the window

        using (new GUILayout.HorizontalScope(EditorStyles.helpBox))                                     // Put each Prefab in the list inside of it's own horizontal help box
        {
            if (selectedGameObject != null)
            {
                DrawText("Enter Problem Here For: " + selectedGameObject.name);
            }
        }
    }

    private void DrawScrollingSection()
    {
        var scrollControlID = GUIUtility.GetControlID(FocusType.Passive);                               // Add Scrolling to the window
        var scrollState = GetScrollState(scrollControlID);

        using (var scrollView = new GUILayout.ScrollViewScope(scrollState.scrollPosition))              // Scrolling section of the window.
        {
            scrollState.scrollPosition = scrollView.scrollPosition;

            DrawList();
        }

    }

    private static ScrollState GetScrollState(int scrollControlID)
    {
        return (ScrollState)GUIUtility.GetStateObject(typeof(ScrollState), scrollControlID);
    }

    private void DrawSearchBar()
    {
        using (new GUILayout.HorizontalScope(EditorStyles.helpBox))                                     // Put each Prefab in the list inside of it's own horizontal help box
        {
            searchField.OnGUI();
            choice = DrawPopUpButton();
        }
    }

    private int DrawPopUpButton()
    {
        return EditorGUILayout.Popup(choice, choices, GUILayout.MaxWidth(200));
    }

    private void DrawList()
    {
        foreach (GameObject obj in prefabList.listOfProjectPrefabs)                                     // Loop through the list of project Prefabs to create the GUI portion of the window.
        {
            bool selected = CheckIfSelected(obj);
            CreateHorizontalHelpBox(obj, selected);
        }
    }

    private void CreateHorizontalHelpBox(GameObject obj, bool selected)
    {
        using (new GUILayout.HorizontalScope(EditorStyles.helpBox))                                     // Put each Prefab in the list inside of it's own horizontal help box
        {
            CreateHorizontalBox(selected);

            DrawHorizontalBox(obj, selected);

            EditorGUILayout.EndHorizontal();
        }
    }

    private void DrawHorizontalBox(GameObject obj, bool selected)
    {
        var rectPrefab = GUILayoutUtility.GetRect(15f, 15f, GUILayout.ExpandWidth(false));              // Rectangle for the prefab icon.

        DrawIcon(rectPrefab, PREFAB_ICON);                                                              // Draw the prefab icon
        DrawButton(obj, selected);                                                                      // Draw the button with the obj name as a label.

        var rectIcon = GUILayoutUtility.GetRect(15f, 15f, GUILayout.ExpandWidth(false));                // Rectangle for the check or cross icon

        switch (CheckIfValid(obj))                                                                      // Check if the obj is valid or not, then draws the coresponding icon
        {
            case (int)ValidationResult.Success:
                DrawIcon(rectIcon, CHECK_ICON_PATH);                                                    // Draw check mark icon
                break;
            case (int)ValidationResult.Fail:
                DrawIcon(rectIcon, CROSS_ICON_PATH);                                                    // Draw cross Icon
                break;
        }
    }

    private static void CreateHorizontalBox(bool selected)
    {
        if (selected)
        {
            EditorGUILayout.BeginHorizontal(GetBtnStyleSelected());                                     // If button selected, use style to set background blue
            return;
        }

        EditorGUILayout.BeginHorizontal();                                                              // If not selected, use default style

        return;

        GUIStyle GetBtnStyleSelected()                                                                  // Method for styling selected buttons to have a blue background
        {
            GUIStyle s = new GUIStyle();
            s.normal.background = Resources.Load(BLUE_SQUARE, typeof(Texture2D)) as Texture2D;

            return s;
        }
    }

    private static bool CheckIfSelected(GameObject obj)
    {
        return obj.Equals(selectedGameObject);
    }

    private void CheckForRightClick(Event e)                                                            // Method which checks if the right mouse key is hit. If so, then make a menu for the right click menu
    {
        if (e.type == EventType.MouseDown && e.button == 1)
        {
            GenericMenu menu = new GenericMenu();

            menu.AddItem(new GUIContent("Get File Path:"), false, RightClick);                          // Add menu item to right click menu, which calls the method YourCallBack.
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

            if (!EditorGUIUtility.isProSkin)
            {
                return s;
            }

            s.normal.textColor = Color.white;                                                           // Set text color to white when not selected

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

    [MenuItem("Assets/Buffer/Check For Updates", priority = 1)]                                         // Functionality for right click in the project window.
    private static void SetupPrefab()                                                                   // This will add the name of the prefab to the search field.
    {
        GameObject obj = (UnityEngine.GameObject)Selection.activeObject;
        string objPath = AssetDatabase.GetAssetPath(obj);
        if (objPath.Contains(FILE_EXTENSION))
        {
            Init();
            searchField.searchString = obj.name;
            searchInput = obj.name.ToLower();
        }
    }
}