using System.Collections.Generic;
using Silverback.EditorTools;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

public class ValidationWindow : OdinEditorWindow
{
    [MenuItem("Scene Validation/Validation Window")]
    private static void OpenWindow()
    {
        GetWindow<ValidationWindow>().Show();
    }

    [HorizontalGroup("Player", 5)]
    [LabelText(""), ReadOnly]
    public bool playerButton = false;



    [HorizontalGroup("Player", marginLeft: -30)]
    [Button("Validate", ButtonSizes.Medium)]
    public void ValidatePlayerPrefab()
    {
        playerButton = PlayerValidation.CheckPlayers();
    }

    [HorizontalGroup("Player", 5, marginLeft: -170)]
    [LabelText("Scene must only have One Player prefab"), LabelWidth(370)]
    public bool playerString;


    [HorizontalGroup("Director", 5)]
    [LabelText(""), ReadOnly]
    public bool directorButton = false;


    [HorizontalGroup("Director", marginLeft: -30)]
    [Button("Validate", ButtonSizes.Medium)]
    public void ValidatePlayableDirectorWrapMode()
    {
        directorButton = DirectorValidation.CheckPlayableDirectorWrapMode();
    }

    [HorizontalGroup("Director", 5, marginLeft: -170)]
    [LabelText("Playable Directors should all have extrapolationMode set to None"), LabelWidth(370)]
    public bool directorString;


    [HorizontalGroup("Hazard", 5)]
    [LabelText(""), ReadOnly]
    public bool hazardButton = false;

    [HorizontalGroup("Hazard", marginLeft: -30)]
    [Button("Validate", ButtonSizes.Medium)]
    public void ValidateHazardsHasUniqueInfraction()
    {
        hazardButton = HazardValidation.CheckHazardsHasUniqueInfraction();
    }

    [HorizontalGroup("Hazard", 5, marginLeft: -170)]
    [LabelText("Each hazard must be mapped to a unique Infraction"), LabelWidth(370)]
    public bool hazardString;


    [HorizontalGroup("Ocean", 5)]
    [LabelText(""), ReadOnly]
    public bool oceanButton = false;

    [HorizontalGroup("Ocean", marginLeft: -30)]
    [Button("Validate", ButtonSizes.Medium)]
    public void ValidateOcean()
    {
        oceanButton = OceanValidation.CheckForOcean();
    }

    [HorizontalGroup("Ocean", 5, marginLeft: -170)]
    [LabelText("Check if scene contains one OceanRenderer"), LabelWidth(370)]
    public bool oceanString;


    [HorizontalGroup("Hierarchy", 5)]
    [LabelText(""), ReadOnly]
    public bool hierarchyButton = false;

    [HorizontalGroup("Hierarchy", 365, marginLeft: -170, PaddingRight = 10)]
    [LabelText("Check and Create GameObjects"), LabelWidth(180)]
    [Required]
    public Hierarchy hierarchy;

    [HorizontalGroup("Hierarchy", marginLeft: -10)]
    [Button("Validate", ButtonSizes.Medium)]
    public void ValidateHierarchy()
    {
        hierarchyButton = HierarchyValidation.CheckAndCreateGameObjects(hierarchy);
    }

    [HorizontalGroup("Camera", 5)]
    [LabelText(""), ReadOnly]
    public bool cameraButton = false;

    [HorizontalGroup("Camera", marginLeft: -30)]
    [Button("Validate", ButtonSizes.Medium)]
    public void ValidateCamera()
    {
        cameraButton = CameraValidation.CheckCamera();
    }

    [HorizontalGroup("Camera", 5, marginLeft: -170)]
    [LabelText("Check if Scene has only one Camera"), LabelWidth(370)]
    public bool cameraString;

    [HorizontalGroup("Lights", 5)]
    [LabelText(""), ReadOnly]
    public bool lightButton = false;

    [HorizontalGroup("Lights", marginLeft: -30)]
    [Button("Validate", ButtonSizes.Medium)]
    public void ValidateLights()
    {
        lightButton = LightValidation.CheckNumberOfEnabledLights();
    }

    [HorizontalGroup("Lights", 5, marginLeft: -170)]
    [LabelText("Check Amount of Lights"), LabelWidth(370)]
    public bool lightString;

    [HorizontalGroup("Names", 5)]
    [LabelText(""), ReadOnly]
    public bool nameButton = false;

    [HorizontalGroup("Names", marginLeft: -30)]
    [Button("Validate", ButtonSizes.Medium)]
    public void ValidateNames()
    {
        nameButton = NameValidation.CheckIfNameUnique();
    }

    [HorizontalGroup("Names", 5, marginLeft: -170)]
    [LabelText("Check GameObject Names"), LabelWidth(370)]
    public bool nameString;


    [CustomValueDrawer("ValidateAllButton")]
    public bool fuckOffButton;
    private bool ValidateAllButton(bool text, GUIContent label)
    {
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("ValidateAll"))
        {
            ValidatePlayerPrefab();
            ValidatePlayableDirectorWrapMode();
            ValidateHazardsHasUniqueInfraction();
            ValidateOcean();
            ValidateHierarchy();
            ValidateCamera();
            ValidateLights();
            ValidateNames();
        }

        return true;
    }
}
