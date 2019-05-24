using UnityEngine;
using UnityEditor;
using System.IO;
using Silverback.EditorTools;
using Sirenix.OdinInspector.Editor;

public class ImportTab
{
    readonly AppleBufferSettings m_buffer = new AppleBufferSettings();
    readonly AOBufferSettings m_aoBufferSettings = new AOBufferSettings();
    readonly PhoneBufferSettings m_phoneBufferSettings = new PhoneBufferSettings();

    private const string BLUE_SQUARE = "blueSquare";
    private const string GEAR_ICON = "gearIcon"; 

    static string info = "";
    static readonly string text = "Hello World ";
    static readonly string versionNumber = "V.10";
    static int selected = -1;
    string bufferName = "";

    int window = 0;

    public void Draw()
    {
        switch (window)
        {
            case 0:
                DrawMainWindow();
                break;
            case 1:
                GetBufferName();
                DrawSettingsWindow();
                break;
        }
    }

    private void GetBufferName()
    {
        switch (selected)
        {
            case 0:
                bufferName = m_buffer.nameOfBuffer;
                break;
            case 1:
                bufferName = m_aoBufferSettings.nameOfBuffer;
                break;
            case 2:
                bufferName = m_phoneBufferSettings.nameOfBuffer;
                break;
        }
    }

    private void DrawSettingsWindow()
    {
        DrawBackButton();

        var scrollControlID = GUIUtility.GetControlID(FocusType.Passive);                               // Add Scrolling to the window
        var scrollState = GetScrollState(scrollControlID);

        using (var scrollView = new GUILayout.ScrollViewScope(scrollState.scrollPosition))              // Scrolling section of the window.
        {
            scrollState.scrollPosition = scrollView.scrollPosition;

            switch (selected)
            {
                case 0:
                    m_buffer.DrawSettingScreen();
                    break;
                case 1:
                    m_aoBufferSettings.DrawSettingScreen();
                    break;
                case 2:
                    m_phoneBufferSettings.DrawSettingScreen();
                    break;
            }

            GUILayout.FlexibleSpace();
        }
    }



    private void DrawBackButton()
    {
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("<", GUILayout.MaxWidth(20)))
        {
            Debug.Log("Hello World");
            window = 0;
        }
        GUILayout.FlexibleSpace();

        EditorGUILayout.LabelField(bufferName, FontStyle());                         //********** Find a way to grab the name of the buffer for the title

        GUIStyle FontStyle()
        {
            GUIStyle s = new GUIStyle()
            {
                fontSize = 20,
            };
            return s;
        }

        GUILayout.FlexibleSpace();

        EditorGUILayout.EndHorizontal();
    }

    private void DrawMainWindow()
    {
        var e = Event.current;

        DrawScrollBox();

        DrawTextField();

        DrawAddButton();

        LeftMouseClick(e);
    }

    private void LeftMouseClick(Event e)
    {
        if (e.type == EventType.MouseDown && e.button == 0)                                             // Check for left mouse click if off the screen, or trying to unselect.
        {
            selected = -1;
            info = "";
        }
    }

    private void DrawTextField()
    {
        GUILayout.FlexibleSpace();

        using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
        {
            EditorGUILayout.LabelField(info, WordWrap());

            GUIStyle WordWrap()                                                               // Style for sections not selected
            {
                GUIStyle s = new GUIStyle
                {
                    wordWrap = true,
                    fixedHeight = 50
                };

                if (!EditorGUIUtility.isProSkin)
                {
                    return s;
                }

                s.normal.textColor = Color.white;                                                           // Set text color to white when not selected

                return s;
            }
        }
    }

    private void DrawAddButton()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("+"))                                                                      // Add a button at the bottom of the window that displays Update All, and will be used to Update every Prefab
        {
            string path = EditorUtility.OpenFilePanel("Overwrite with png", "", "png");
            if (path.Length != 0)
            {
                var fileContent = File.ReadAllBytes(path);
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    private void DrawScrollBox()
    {
        var scrollControlID = GUIUtility.GetControlID(FocusType.Passive);                               // Add Scrolling to the window
        var scrollState = GetScrollState(scrollControlID);

        using (var scrollView = new GUILayout.ScrollViewScope(scrollState.scrollPosition))              // Scrolling section of the window.
        {
            scrollState.scrollPosition = scrollView.scrollPosition;

            DrawTextArea(0, m_buffer.nameOfBuffer, m_buffer.version);

            DrawTextArea(1, m_aoBufferSettings.nameOfBuffer, "\t" + m_aoBufferSettings.version);

            DrawTextArea(2, m_phoneBufferSettings.nameOfBuffer, m_phoneBufferSettings.version);

            for(int i = 3; i < 10; i++)
            {
                DrawTextArea(i, text, versionNumber);
            }
        }
    }

    private ScrollState GetScrollState(int scrollControlID)
    {
        return (ScrollState)GUIUtility.GetStateObject(typeof(ScrollState), scrollControlID);
    }

    private void DrawTextArea(int count, string name, string version)
    {
        EditorGUILayout.BeginHorizontal();
        DrawButton(count, name, version);
        if (selected.Equals(count))
        {
            DrawSettingsButton();
        }
        EditorGUILayout.EndHorizontal();
        return;
    }

    private void DrawSettingsButton()
    {
        var rectIcon = GUILayoutUtility.GetRect(10f, 10f, GetBtnStyleSelected());                       // Rectangle for the check or cross icon

        var tex = Resources.Load(GEAR_ICON, typeof(Texture2D)) as Texture2D;                         // Add a texture of the Icon
         
        if (GUI.Button(rectIcon, tex, GetBtnStyleSelected()))
        {
            window = 1;
        }

        GUIStyle GetBtnStyleSelected()                                                               // Style for sections not selected
        {
            GUIStyle s = new GUIStyle
            {
                margin = new RectOffset(0, 5, 7, 0),
                fixedWidth = 20
            };

            return s;
        }
    }

   private void DrawButton(int count, string name, string version)
    {
        using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
        {
            if (selected.Equals(count))
            {
                DrawSelected(count, name, version);

                return;
            }


            DrawNotSelected(count, name, version);
        }
    }

    private void DrawSelected(int count, string name, string version)
    {
        if (GUILayout.Button(name + "\t\t" + version, GetBtnStyleSelected()))
        {
            info = "Once I have a list of the different things, I can change this text.";

            selected = count;
        }

        GUIStyle GetBtnStyleSelected()                                                               // Style for sections not selected
        {
            GUIStyle s = new GUIStyle
            {
                margin = new RectOffset(5, 0, 0, 0),
                wordWrap = true,
            };

            s.normal.background = Resources.Load(BLUE_SQUARE, typeof(Texture2D)) as Texture2D;

            s.normal.textColor = Color.white;                                                           // Set text color to white when not selected

            return s;
        }
    }

    private void DrawNotSelected(int count, string name, string version)
    {
        if (GUILayout.Button(name + "\t\t" + version, GetBtnStyleNotSelected()))
        {
            selected = count;

            info = "Once I have a list of the different things, I can change this text.";
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
    }
}
