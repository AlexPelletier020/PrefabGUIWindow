using UnityEditor;
using UnityEngine;

public class WindowBuffer : EditorWindow
{
    static readonly ImportTab importTab = new ImportTab();
    static readonly UpdateTab updateTab = new UpdateTab();

    static int toolbarInt = 0;
    readonly string[] toolbarStrings = { "Settings", "Update" };

    private const string FILE_EXTENSION = ".prefab";
    private const int MAX_BOTTOM_SPACE = 20;
    private const string BUFFERING_TEXTURE = "bufferingTexture";
    private const string PREFAB_TEXTURE = "prefabTexture";

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
        EditorGUILayout.BeginVertical();
        if (toolbarInt == 0)
        {
            DrawImage(BUFFERING_TEXTURE);
        }
        else
        {
            DrawImage(PREFAB_TEXTURE);
        }
        toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarStrings);
        Draw();
        EditorGUILayout.EndVertical();                                                                  // End the verticle section
    }

    void Draw()
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

    private void DrawImage(string textureName)
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        var rect = GUILayoutUtility.GetRect(250f, 125f, GUILayout.ExpandWidth(false));                  // Rectangle for the prefab icon.
        var tex = Resources.Load(textureName, typeof(Texture2D)) as Texture2D;                             // Add a texture of the Icon
        EditorGUI.LabelField(rect, new GUIContent(tex));
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
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

    [MenuItem("Assets/Buffer/Import Buffer", priority = 1)]                                             // Functionality for right click in the project window.
    private static void ImportBufferMenu()                                                              // This will add the name of the prefab to the search field.
    {
        SetTabValue(0);
        importTab.window = 0;
        Init();
    }

    [MenuItem("Assets/Import Buffer", priority = 20)]                                                   // Functionality for right click in the project window.
    private static void ImportBuffer()                                                                  // This will add the name of the prefab to the search field.
    {
        SetTabValue(0);
        importTab.window = 0;
        Init();
    }
}