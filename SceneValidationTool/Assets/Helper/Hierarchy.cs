using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "New Hierarchy")]
public class Hierarchy : SerializedScriptableObject
{
    [BoxGroup("Hierarchy Breakdown")]
    public List<NewFolder> hierarchyList = new List<NewFolder>();
}


public class NewFolder
{
    [Required]
    public string parentFolder = "New Parent";

    public string[] childrenFolders = new string[1];
}
