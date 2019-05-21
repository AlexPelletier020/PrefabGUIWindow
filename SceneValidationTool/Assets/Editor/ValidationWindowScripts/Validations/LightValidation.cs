using UnityEngine;
using System.Collections;
using UnityEditor;

public class LightValidation : MonoBehaviour
{
    public static bool CheckNumberOfEnabledLights()
    {
        int lightCount = FindObjectsOfType<Light>().Length;                           // Grabs all the lights from the scene and puts the count to an integer variable

        EditorUtility.DisplayDialog("Scene Validation Tool",                                // Display the amount of lights found.
                $"The current scene has {lightCount} Lights Prefab in the scene.",
                "OK");

        return true;
    }
}
