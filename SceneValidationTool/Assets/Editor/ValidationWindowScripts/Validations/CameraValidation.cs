using UnityEngine;
using System.Collections;
using UnityEditor;

public class CameraValidation : MonoBehaviour
{
    public static bool CheckCamera()
    {
        int cameraCount = Camera.allCameras.Length;                                         // Get the amount of cameras in the scene and set it equal to an int variable cameraCount

        if (cameraCount != 1)                                                               // Check if there is anything but one camera in the scene, if so, then display an error message
        {                                                                                   // and return false.
            EditorUtility.DisplayDialog("Scene Validation Tool",
                    $"The current scene has {cameraCount} Cameras." +
                    $"There should be one Camera.",
                    "OK");
            return false;
        }
        return true;
    }
}
