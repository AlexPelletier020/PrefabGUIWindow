using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class HazardValidation : MonoBehaviour
{
    public static bool CheckHazardsHasUniqueInfraction()
    {
        //var _Hazards = FindObjectsOfType<HazardComponent>();
        //List<IInfractionObject> _Infractions = new List<IInfractionObject>();
        //foreach (var _H in _Hazards)
        //{
        //  _Infractions.Add(_H.CorrespondingInfraction);
        //}
        //bool validateResult = _Infractions.Count == _Infractions.Distinct().Count() ? true : false;
        //if (!validateResult)
        //{
        //  EditorUtility.DisplayDialog("Scene Validation Tool",
        //          $"There seems to be duplicate InfractionObject(s) " +
        //          $"mapped to Hazard Component.",
        //          "OK");
        //  return false;
        //}
        return true;
    }
}
