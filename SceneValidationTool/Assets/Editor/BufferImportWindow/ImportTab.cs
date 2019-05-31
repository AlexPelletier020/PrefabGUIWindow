using UnityEngine;
using UnityEditor;
using System.IO;
using Silverback.EditorTools;

public class ImportTab
{
    readonly AppleBufferSettings m_buffer = new AppleBufferSettings();                          // Buffer Objects as data membersa
    readonly AOBufferSettings m_aoBufferSettings = new AOBufferSettings();
    readonly PhoneBufferSettings m_phoneBufferSettings = new PhoneBufferSettings();

    private const string BLUE_SQUARE = "blueSquare";                                            // Blue square texture for background when selected
    private const string GEAR_ICON = "gearIcon";                                                // Texture for the gear Icon
    private const int MAX_BOTTOM_SPACE = 3;                                                     // const for spacing when using GUILayout.space();

    static string info = "";                                                                    // String datamember for the text in the text area.

    static readonly string text = "Hello World ";                                               // Place holders for filler buffers
    static readonly string versionNumber = "V.10"; 
                                                 
    static int selected = -1;                                                                   // int datamember to control which buffer is selected
    string bufferName = "";                                                                     // String data member to hold the selected buffer's name

    public int window = 0;                                                                      // int data member to hold which window should be drawn.

    public void Draw()
    {
        // Main Draw method called by WindowBuffer.cs to be ablke to draw this window.
        switch (window)
        {
            case 0:
                DrawMainWindow();                                                                // Draw the main window which lists all the Buffersa.
                break;
            case 1:
                GetBufferName();                                                                 // Get the buffer name for the title of the page
                DrawSettingsWindow();                                                            // Call the draw method for the settings window.
                break;
        }
    }

    private void GetBufferName()
    {
        // Get the selected buffer's name using the selected data member
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
            default:
                bufferName = "";
                break;
        }
    }

    private void DrawSettingsWindow()
    {
        // Draw the settings window.

        DrawBackButton();                                                                               // Draw the back button to be able to go back to the main window.

        var scrollControlID = GUIUtility.GetControlID(FocusType.Passive);                               // Add Scrolling to the window
        var scrollState = GetScrollState(scrollControlID);

        using (var scrollView = new GUILayout.ScrollViewScope(scrollState.scrollPosition))              // Scrolling section of the window.
        {
            scrollState.scrollPosition = scrollView.scrollPosition;

            switch (selected)                                                                           // Using the selected int data member, determine which buffer was slected and draw their own interface settings 
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
        // Method to draw the back button and the functionality for it.

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("<", GUILayout.MaxWidth(20)))                                              // Draw the back button "<". If clicked by the user, set the data member window to 0 to draw the main window.
        {
            window = 0;
        }
        GUILayout.FlexibleSpace();

        EditorGUILayout.LabelField(bufferName, FontStyle());                                            // Draw the buffer name to the right of the back button using a self made GUIStyle to increase the font.                     

        GUIStyle FontStyle()
        {
            // Customn GUIStyle to make the font bigger so it looks like a title
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
        // Method called to draw the main window which is the list of buffers.

        var e = Event.current;                                                                          // Get any mouse click events.

        DrawScrollBox();                                                                                // Draw the scroll box which also contains the list

        DrawTextField();                                                                                // Draw the text firled which holds the extra info about a selected buffer

        DrawButtons();                                                                                  // Draw the cancel or Create buttons at the bottom of the screen

        GUILayout.Space(MAX_BOTTOM_SPACE);                                                              // Add a little space at the bottom to give the buttons some space from the bottom.

        LeftMouseClick(e);                                                                              // Check for left click.
    }

    private void LeftMouseClick(Event e)
    {
        // Methnod to check if a left click happened, if so set selected to -1 to unselect everything.

        if (e?.type == EventType.MouseDown && e?.button == 0)                                           // Check for left mouse click if off the screen, or trying to unselect.
        {
            selected = -1;
            info = "";

        }
    }

    private void DrawTextField()
    {
        // Method to draw the giant text field under the list.

        GUILayout.FlexibleSpace();                                                                      // Add some space between the list and the textbox

        using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
        {
            EditorGUILayout.LabelField(info, WordWrap());                                               // Create a label sectiuon using a custom GUIStyle called WordWrap

            GUIStyle WordWrap()                                                                                 
            {
                // Custom GUIStyle to give the label field word wrap and a fixed height of 50
                GUIStyle s = new GUIStyle
                {
                    wordWrap = true,
                    fixedHeight = 50
                };

                if (!EditorGUIUtility.isProSkin)
                {   // Check if the user is using the pro version dark theme.
                    return s;
                }

                s.normal.textColor = Color.white;                                                       // Set text color to white for drak theme users.

                return s;
            }
        }
    }

    private void DrawButtons()
    {
        // MEthod used to Draw the bottom buttons

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Create", GUILayout.MinWidth(100)))                                        // Draw the Create Button. When clicked, open CreateNewBuffer class as a pop up.
        {
            CreateNewBuffer m_createNewBuffer = ScriptableObject.CreateInstance<CreateNewBuffer>();
            m_createNewBuffer.Init();
        }
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Import", GUILayout.MinWidth(100)))                                        // Add the import button, when clicked, this will open a file explorer window.
        {
            string path = EditorUtility.OpenFilePanel("Overwrite with png", "", "png");
            if (path.Length != 0)
            {
                var fileContent = File.ReadAllBytes(path);
            }
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }

    private void DrawScrollBox()
    {
        // Draw scroll box method. The scroll box will hold the list of buffers.

        var scrollControlID = GUIUtility.GetControlID(FocusType.Passive);                               // Add Scrolling to the window
        var scrollState = GetScrollState(scrollControlID);

        using (var scrollView = new GUILayout.ScrollViewScope(scrollState.scrollPosition))              // Scrolling section of the window.
        {
            scrollState.scrollPosition = scrollView.scrollPosition;

            // **** These will need to come from a list. ******
            DrawTextArea(0, m_buffer.nameOfBuffer, m_buffer.version);                                   // Draw the first buffer by calling DrawTextArea

            DrawTextArea(1, m_aoBufferSettings.nameOfBuffer, m_aoBufferSettings.version);               // Draw the second buffer

            DrawTextArea(2, m_phoneBufferSettings.nameOfBuffer, m_phoneBufferSettings.version);         // Draw the thrid buffer

            for(int i = 3; i < 10; i++)
            {                                                                                           // Draw the filler buffers
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
        // Method to draw the setting icon of the gear when they get selected.

        var rectIcon = GUILayoutUtility.GetRect(10f, 10f, GetBtnStyleSelected());                       // Rectangle for the check or cross icon

        var tex = Resources.Load(GEAR_ICON, typeof(Texture2D)) as Texture2D;                            // Add a texture of the Icon
         
        if (GUI.Button(rectIcon, tex, GetBtnStyleSelected()))
        {
            window = 1;                                                                                 // If the gear icon is clicked, set the window data member to 1 to go to the settings window.
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

   private void DrawButton(int count, string name, string version)
    {
        // Method that draws each buffer as a selectable button. 
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
        // Method used to draw the selected buffer by making the background blue and displaying the gerar icon.

        if (GUILayout.Button("  " + name, GetBtnStyleSelected()))
        {
            info = "Once I have a list of the different things, I can change this text.\t" + version;

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
        // Method to draw the buffers that aren't selected.

        if (GUILayout.Button("  " + name, GetBtnStyleNotSelected()))                                    // If they get clicked, then the user selected this buffer.
        {
            selected = count;

            info = "Once I have a list of the different things, I can change this text.\t" + version;
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
