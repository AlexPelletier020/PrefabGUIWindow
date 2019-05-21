#if UNITY_EDITOR
//using Crest;
//using Silverback.Scooter.Infractions;
//using Silverback.Scooter.Playables.Timeline;
//using Silverback.Scooter.Player;

using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

namespace Silverback.EditorTools
{
    public class SceneHelper : MonoBehaviour
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

		public static bool CheckPlayers()
		{
			//var _Players = FindObjectsOfType<PlayerCore>();
			//if (_Players.Length != 1)
			//{
			//	EditorUtility.DisplayDialog("Scene Validation Tool",
			//			$"The current scene has {_Players.Length} Player Prefab." +
			//			$"There should be one Player Prefab.",
			//			"OK");
			//	return false;
			//}
			return true;
		}

		public static bool CheckHazardsHasUniqueInfraction()
		{
			//var _Hazards = FindObjectsOfType<HazardComponent>();
			//List<IInfractionObject> _Infractions = new List<IInfractionObject>();
			//foreach (var _H in _Hazards)
			//{
			//	_Infractions.Add(_H.CorrespondingInfraction);
			//}
			//bool validateResult = _Infractions.Count == _Infractions.Distinct().Count() ? true : false;
			//if (!validateResult)
			//{
			//	EditorUtility.DisplayDialog("Scene Validation Tool",
			//			$"There seems to be duplicate InfractionObject(s) " +
			//			$"mapped to Hazard Component.",
			//			"OK");
			//	return false;
			//}
			return true;
		}

		public static bool CheckForOcean()
		{
			//var _Ocean = FindObjectsOfType<OceanRenderer>();
			//return _Ocean.Length == 1;
			return true;
		}

        /*
		public static bool CheckAndCreateGameObjects()
		{
            GameObject[] gameObjects = FindObjectsOfType<GameObject>();

			public Hierarchy _Objects;					// This is where I am running into an issue of using a specific string instead of an object to load the data *


			foreach(Hierarchy masterParent in _Objects.)
			{
				CheckForObjects(masterParent, gameObjects);
			}
            
			return true;
		}
		*/


        /*
		public static void CheckForObjects(Master_Parents parentFolder, GameObject[] gameObjects)
		{
			if (parentFolder != null)
			{
				if (!CheckForObjects(gameObjects, parentFolder._Parent))
				{
					CreateNewObjects(parentFolder._Parent, null);
				}
				foreach (var _Child in parentFolder._Children)
				{
					if (!CheckForObjects(gameObjects, _Child))
					{
						CreateNewObjects(_Child, parentFolder._Parent);
					}
				}
			}
		}
		*/

		public static void CreateNewObjects(string objectName, string parent)
		{
			if (EditorUtility.DisplayDialog("Scene Validation Tool",
							$"The current scene does not have a {objectName} game object created. " +
							$"Would you like to create one?.",
							"Yes", "No"))
			{
				var _NewObject = new GameObject(objectName);									// Create the Game Object if yes is selected

				if (parent != null)
				{
					_NewObject.transform.parent = GameObject.Find(parent).transform;			// If a parent is specified, add the parent to the new object
				}
			}
		}

		public static bool CheckForObjects(GameObject[] _GameObjects, string name)
		{
			foreach (var _Object in _GameObjects)
			{
				if (_Object.name.Equals(name))													// Check if the name passed in is equal to any names in the list of gameObjects in the scene
				{
					return true;
				}
			}
			return false;
		}

		public static bool CheckCamera()
		{
			int cameraCount = Camera.allCameras.Length;											// Get the amount of cameras in the scene and set it equal to an int variable cameraCount

			if (cameraCount != 1)																// Check if there is anything but one camera in the scene, if so, then display an error message
			{																					// and return false.
				EditorUtility.DisplayDialog("Scene Validation Tool",
						$"The current scene has {cameraCount} Cameras." +
						$"There should be one Camera.",
						"OK");
				return false;
			}
			return true;
		}

		public static bool CheckNumberOfEnabledLights()
		{
			int lightCount = FindObjectsOfType<Light>().Length;							// Grabs all the lights from the scene and puts the count to an integer variable

			EditorUtility.DisplayDialog("Scene Validation Tool",								// Display the amount of lights found.
					$"The current scene has {lightCount} Lights Prefab in the scene.",
					"OK");

			return true;
		}

		public static bool CheckIfNameUnique()
		{
			var gameObjects = FindObjectsOfType<GameObject>();                       // Make an array list of all game objects

			for(int enumerator = 0; enumerator < gameObjects.Length; enumerator++)				// Double for loop to check if the names are the same. Only checks foward and not backwards.
			{
				for(int counter = enumerator + 1; counter < gameObjects.Length; counter++)
				{
					if (gameObjects[enumerator].name.Equals(gameObjects[counter].name))
					{
						EditorUtility.DisplayDialog("Scene Validation Tool",
							$"The current scene has 2 game objects with the same name: ({gameObjects[enumerator].name}) in the scene." +
							$" This needs to be corrected",
							"OK");
						return false;															// If the same name is found, returns false, or else, will return true.
					}
				}
			}

			return true;
		}		
	}
}
#endif
