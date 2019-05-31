using UnityEditor;
using UnityEngine;

public abstract class AbstractSerializedFields
{
    // Class which holds the GUI draw methods for the setting interfaces.

    public float DrawFloatField(float floatValue, string label, int spacePixel)
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(spacePixel);
        floatValue = EditorGUILayout.FloatField(label, floatValue);
        EditorGUILayout.EndHorizontal();
        return floatValue;
    }

    public int DrawIntField(int intValue, string label, int spacePixel)
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(spacePixel);
        intValue = EditorGUILayout.IntField(label, intValue);
        EditorGUILayout.EndHorizontal();
        return intValue;
    }

    public string DrawTextField(string stringValue, string label, int spacePixel)
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(spacePixel);
        stringValue = EditorGUILayout.TextField(label, stringValue);
        EditorGUILayout.EndHorizontal();
        return stringValue;
    }

    public void DrawLabel(string label, int spacePixel)
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(spacePixel);
        EditorGUILayout.LabelField(label);
        EditorGUILayout.EndHorizontal();
    }

    public int DrawPopup(int integerValue, string[] stringArray, string label, int spacePixel)
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(spacePixel);
        integerValue = EditorGUILayout.Popup(label, integerValue, stringArray);
        EditorGUILayout.EndHorizontal();
        return integerValue;
    }

    public float DrawFloatSlider(float floatValue, float floatMinValue, float floatMaxValue, string label, int spacePixel)
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(spacePixel);
        floatValue = EditorGUILayout.Slider(label, floatValue, floatMinValue, floatMaxValue);
        EditorGUILayout.EndHorizontal();
        return floatValue;
    }

    public int DrawIntSlider(int intValue, int minValue, int maxValue, string label, int spacePixel)
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(spacePixel);
        intValue = EditorGUILayout.IntSlider(label, intValue, minValue, maxValue);
        EditorGUILayout.EndHorizontal();
        return intValue;
    }

    public Material DrawMaterialField(Material mat, string label, int spacePixel)
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(spacePixel);
        mat = (Material)EditorGUILayout.ObjectField(label, mat, typeof(Material), true);
        EditorGUILayout.EndHorizontal();
        return mat;
    }

    public Light DrawLightField(Light light, string label, int spacePixel)
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(spacePixel);
        light = (Light)EditorGUILayout.ObjectField(label, light, typeof(Light), true);
        EditorGUILayout.EndHorizontal();
        return light;
    }

    public bool DrawFoldout(bool boolValue, string label)
    {
        EditorGUILayout.BeginHorizontal();
        boolValue = EditorGUILayout.Foldout(boolValue, label);
        EditorGUILayout.EndHorizontal();
        return boolValue;
    }

    public bool DrawToggle(bool boolValue, string label, int spacePixel)
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(spacePixel);
        boolValue = EditorGUILayout.Toggle(label, boolValue);
        EditorGUILayout.EndHorizontal();
        return boolValue;
    }
}
