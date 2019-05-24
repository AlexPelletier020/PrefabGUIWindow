using UnityEditor;

public class BufferWindow : EditorWindow
{
    public int toolbarInt = 0;

    readonly ImportTab importTab = new ImportTab();

    [MenuItem("Buffered/Buffer Window")]
    static void Init()
    {
        var window = GetWindow<BufferWindow>();                                                         // Create a window variable

        window.Show();                                                                                  // Show the window
    }

    void OnGUI()
    {
        EditorGUILayout.BeginVertical();                                                                // Begin the vertical section

        importTab.Draw();

        EditorGUILayout.EndVertical();                                                                  // End the verticle section
    }
}