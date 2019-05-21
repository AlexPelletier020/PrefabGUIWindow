using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.Playables;

public class DirectorValidation : MonoBehaviour
{
    public static bool CheckPlayableDirectorWrapMode()
    {
        var _PlayableDirectors = FindObjectsOfType<PlayableDirector>();

        foreach (var _PlayableDirector in _PlayableDirectors)
        {
            if (_PlayableDirector.extrapolationMode != DirectorWrapMode.None)
            {
                EditorUtility.DisplayDialog("Scene Validation Tool",
                        $"PlayableDirector: {_PlayableDirector.name} " +
                        $"has incorrect WrapMode. Please change it to None.",
                        "OK");
                return false;
            }
        }

        return true;
    }
}
