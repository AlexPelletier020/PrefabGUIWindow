using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ButtonLogic
{
    public void UpdateButton(GameObject obj)
    {
        Debug.Log(AssetDatabase.GetAssetPath(obj));    
    }

    public void UpdateAllButton(List<GameObject> listOfUpdateObjects)
    {
        foreach (GameObject obj in listOfUpdateObjects)
        {
            Debug.Log(AssetDatabase.GetAssetPath(obj) + "\n");
        }
    }

    public void RightClickLogic(GameObject obj)
    {
        Debug.Log(AssetDatabase.GetAssetPath(obj) + "\n");
    }
}
