using UnityEditor;

public class WindowBuffer : EditorWindow
{
    readonly ImportTab importTab = new ImportTab();

    [MenuItem("Buffered/BufferListWindow")]
    static void Init()
    {
        var window = GetWindow<WindowBuffer>();                                                         // Create a window variable

        window.Show();                                                                                  // Show the window
    }

    void OnGUI()
    {
        EditorGUILayout.BeginVertical();                                                                // Begin the vertical section

        importTab.Draw();

        EditorGUILayout.EndVertical();                                                                  // End the verticle section
    }
}