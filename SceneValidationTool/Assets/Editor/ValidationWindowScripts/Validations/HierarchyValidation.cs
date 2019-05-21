using UnityEngine;
using System.Collections;
using UnityEditor;

public class HierarchyValidation : MonoBehaviour
{
    public static bool CheckAndCreateGameObjects(Hierarchy folders)
    {
        ArrayList missingObjects = new ArrayList();

        GameObject[] gameObjects = FindObjectsOfType<GameObject>();

        if(folders != null)
        {
            foreach (NewFolder folder in folders.hierarchyList)
            {
                if (CheckForObjects(folder, gameObjects))
                {
                    missingObjects.Add(folder);
                }
                else
                {
                    foreach (string child in folder.childrenFolders)
                    {
                        if (!CheckForObjectInScene(gameObjects, child))
                        {
                            CreateNewObjects(child, folder.parentFolder);
                        }

                    }
                }
            }

            if (missingObjects.Count > 0)
            {
                if (EditorUtility.DisplayDialog("Scene Validation Tool",
                            $"The current scene is missing {missingObjects.Count} game objects that form the directories. " +
                            $"Would you like to create them?.",
                            "Yes", "No"))
                {
                    foreach (NewFolder folder in missingObjects)
                    {
                        CreateNewObjects(folder.parentFolder, null);

                        foreach (string child in folder.childrenFolders)
                        {
                            CreateNewObjects(child, folder.parentFolder);
                        }
                    }

                    return true;
                }

                return false;
            }

            return true;
        }

        return false;
    }


    private static bool CheckForObjects(NewFolder folder, GameObject[] gameObjects)
    {
        if (folder != null)
        {
            if (!CheckForObjectInScene(gameObjects, folder.parentFolder))
            {
                return true;
            }
            return false;
        }
        return false;
    }

    private static bool CheckForObjectInScene(GameObject[] gameObjects, string name)
    {
        foreach (var folder in gameObjects)
        {
            if (folder.name.Equals(name))                                                  // Check if the name passed in is equal to any names in the list of gameObjects in the scene
            {
                return true;
            }
        }
        return false;
    }

    private static void CreateNewObjects(string objectName, string parent)
    {
        var newObject = new GameObject(objectName);                                    // Create the Game Object if yes is selected

        if (parent != null)
        {
            newObject.transform.parent = GameObject.Find(parent).transform;            // If a parent is specified, add the parent to the new object
        } 
    }
}
