using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerValidation : MonoBehaviour
{
    public static bool CheckPlayers()
    {
        //var _Players = FindObjectsOfType<PlayerCore>();
        //if (_Players.Length != 1)
        //{
        //  EditorUtility.DisplayDialog("Scene Validation Tool",
        //          $"The current scene has {_Players.Length} Player Prefab." +
        //          $"There should be one Player Prefab.",
        //          "OK");
        //  return false;
        //}
        return true;
    }


}
