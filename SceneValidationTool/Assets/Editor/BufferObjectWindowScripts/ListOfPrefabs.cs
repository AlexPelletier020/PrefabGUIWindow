using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;
using System.Linq;
using UnityEngine.SceneManagement;
using Silverback.EditorTools;

public class ListOfPrefabs
{
    public List<GameObject> listOfProjectPrefabs;

    public void PopulateList(String searchInput, int sortingChoice)
    {
        listOfProjectPrefabs = CreateList(searchInput);                                                            // Repopulate the list by calling CreateList method.

        listOfProjectPrefabs = Sort(sortingChoice);
    }

    private List<GameObject> Sort(int sortingChoice)
    {
        if (sortingChoice == (int)SortingChoices.Alphabetical)                                           // Sort the list alphabetically
        {
            listOfProjectPrefabs = listOfProjectPrefabs.OrderBy(obj => obj.name).ToList();

            return listOfProjectPrefabs;
        }

        listOfProjectPrefabs = listOfProjectPrefabs.OrderByDescending(obj => obj.name).ToList();                           // Or sort the list Unalphabetical

        return listOfProjectPrefabs;
    }

    private List<GameObject> CreateList(String searchInput)
    {
        listOfProjectPrefabs = new List<GameObject>();                                                  // ArrayList which will hold only the prfabs in the project and not the ones in the scene                                         

        ArrayList allObjects = new ArrayList();                                                         // GameObject array which contains all of the Game Objects, both in the scene and in the project
        string[] allPaths = FetchAllPaths();

        foreach (String path in allPaths)
        {
            allObjects.Add(AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)));
        }

        listOfProjectPrefabs = CheckPrefab(listOfProjectPrefabs, allObjects, searchInput);

        return listOfProjectPrefabs;
    }


    private static List<GameObject> CheckPrefab(List<GameObject> listOfProjectPrefabs, ArrayList allObjects, String searchInput)
    {
        foreach (GameObject obj in allObjects)                                                          // Loop through the array of all objects and check if they have the component required, 
        {                                                                                               // and check that the gameObjects are not located in the current scene
            if (obj.scene.name != SceneManager.GetActiveScene().name &&
                obj.name.ToLower().Contains(searchInput))                                               // Check if the gameObject is not located in the current scene.
            {
                listOfProjectPrefabs.Add(obj);                                                          // If both are true, then add the gameObject to the ArrayList of project prefabs
            }
        }

        return listOfProjectPrefabs;
    }

    private static string[] FetchAllPaths()
    {
        return System.IO.Directory.GetFiles("Assets/", "*.prefab", System.IO.SearchOption.AllDirectories);
    }
}
