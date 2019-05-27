using UnityEditor;
using UnityEngine;

public class WindowBuffer : EditorWindow
{
    static readonly ImportTab importTab = new ImportTab();
    static readonly UpdateTab updateTab = new UpdateTab();
    static int toolbarInt = 0;
    readonly string[] toolbarStrings = { "Settings", "Update" };

    private const string FILE_EXTENSION = ".prefab";

    [MenuItem("Buffered/BufferListWindow")]
    public static void Init()
    {
        var window = GetWindow<WindowBuffer>();                                                         // Create a window variable

        window.Show();                                                                                  // Show the window
    }

    public static void SetTabValue(int value)
    {
        toolbarInt = value;
    }

    void OnGUI()
    {
        EditorGUILayout.BeginVertical();                                                                // Begin the vertical section
        toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarStrings);
        Draw();
        EditorGUILayout.EndVertical();                                                                  // End the verticle section
    }

    public static void Draw()
    {
        switch (toolbarInt)
        {
            case 0:
                importTab.Draw();
                break;
            case 1:
                updateTab.Draw();
                break;
        }
    }

    [MenuItem("Assets/Buffer/Check For Updates", priority = 1)]                                         // Functionality for right click in the project window.
    private static void SetupPrefab()                                                                   // This will add the name of the prefab to the search field.
    {
        if (updateTab.searchField == null)
        {
            updateTab.OnEnable();
        }
        GameObject obj = (UnityEngine.GameObject)Selection.activeObject;
        string objPath = AssetDatabase.GetAssetPath(obj);
        if (objPath.Contains(FILE_EXTENSION))
        {
            SetTabValue(1);
            updateTab.searchField.searchString = obj.name;
            updateTab.searchInput = obj.name.ToLower();
            Init();
        }
    }
}