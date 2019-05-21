using Silverback.EditorTools;

[SceneValidator("ContentSceneValidationRules")]
public class ContentSceneValidationRules
{
	[ValidationMethod("Scene must only have One Player prefab")]
	public bool ValidatePlayerPrefab()
	{
		return SceneHelper.CheckPlayers();
	}

	[ValidationMethod("Playable Directors should all have extrapolationMode set to None")]
	public bool ValidatePlayableDirectorWrapMode()
	{
		return SceneHelper.CheckPlayableDirectorWrapMode();
	}

	[ValidationMethod("Each hazard must be mapped to a unique Infraction")]
	public bool ValidateHazardComponents()
	{
		return SceneHelper.CheckHazardsHasUniqueInfraction();
	}

	[ValidationMethod("Check if scene contains one OceanRenderer")]
	public bool ValidateOcean()
	{
		return SceneHelper.CheckForOcean();
	}

	[ValidationMethod("Check and Create GameObjects")]
	public bool ValidateGameObjectCreation()
	{
        //return SceneHelper.CheckAndCreateGameObjects();

        return true;
	}

	[ValidationMethod("Check if Scene has only one Camera")]
	public bool ValidateCamera()
	{
		return SceneHelper.CheckCamera();
	}

	[ValidationMethod("Check Amount of Lights")]
	public bool ValidateLights()
	{
		return SceneHelper.CheckNumberOfEnabledLights();
	}

	[ValidationMethod("Check GameObject Names")]
	public bool ValidateNames()
	{
		return SceneHelper.CheckIfNameUnique();
	}
}
