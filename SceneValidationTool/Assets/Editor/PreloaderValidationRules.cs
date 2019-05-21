using Silverback.EditorTools;
using UnityEngine;

[SceneValidator("Assets/_Content/_Scenes/Base Scenes/Preloader_Base.unity")]
public class PreloaderValidationRules
{
	[ValidationMethod("Has PersistentServices prefab")]
	public bool ValidatePersistentServices()
	{
		var _GameObject = GameObject.Find("PersistentServices");
		return _GameObject is null ? false : true;
	}
}
