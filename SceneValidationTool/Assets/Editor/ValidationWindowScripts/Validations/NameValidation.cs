using UnityEngine;
using System.Collections;
using UnityEditor;

public class NameValidation : MonoBehaviour
{
    public static bool CheckIfNameUnique()
    {
        var gameObjects = FindObjectsOfType<GameObject>();                       // Make an array list of all game objects

        for (int enumerator = 0; enumerator < gameObjects.Length; enumerator++)              // Double for loop to check if the names are the same. Only checks foward and not backwards.
        {
            for (int counter = enumerator + 1; counter < gameObjects.Length; counter++)
            {
                if (gameObjects[enumerator].name.Equals(gameObjects[counter].name))
                {
                    EditorUtility.DisplayDialog ("Scene Validation Tool",
                        $"The current scene has 2 game objects with the same name: ({gameObjects[enumerator].name}) in the scene." +
                        $" This needs to be corrected",
                        "OK");
                    return false;                                                           // If the same name is found, returns false, or else, will return true.
                }
            }
        }

        return true;
    }
}
