using UnityEditor;
using UnityEngine;

public class PhoneBufferSettings : AbstractSerializedFields, IBufferInterface
{
    PhoneSettings settings;

    bool environmentFold = true;

    readonly string[] skyLightOptions = { "Skybox", "Gradient", "Color" };
    readonly string[] ambientOptions = { "Realtime", "Baked" };
    readonly string[] reflectionSourceOptions = { "Skybox", "Custom" };
    readonly string[] resolutionOptions = { "16", "32", "64", "128", "256", "512", "1024", "2048" };
    readonly string[] compressionOptions = { "Uncompressed", "Compressed", "Auto" };

    public string nameOfBuffer = "Phone Buffer";
    public string version = "V.12";

    public void DrawSettingScreen()
    {
        DrawEnvironmentSection();
    }

    private void DrawEnvironmentSection()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        environmentFold = DrawFoldout(environmentFold, "Environment");

        if (environmentFold)
        {
            settings.material = DrawMaterialField(settings.material, "Skybox Material", 20);

            settings.mainLight = DrawLightField(settings.mainLight, "Sun Source", 20);

            DrawEnvironmentLighting();

            DrawEnvironmentReflection();
        }
        EditorGUILayout.EndVertical();
    }

    private void DrawEnvironmentReflection()
    {
        DrawLabel("Environment Reflections", 20);

        settings.skyReflectionOption = DrawPopup(settings.skyReflectionOption, reflectionSourceOptions, "Source", 30);

        settings.resolutionOption = DrawPopup(settings.resolutionOption, resolutionOptions, "Resolution", 30);

        settings.compressionOption = DrawPopup(settings.compressionOption, compressionOptions, "Compression", 30);

        settings.intensityReflection = DrawFloatSlider(settings.intensityReflection, 0, 1, "Intensity Multiplier", 30);

        settings.bounces = DrawIntSlider(settings.bounces, 1, 5, "Bounces", 30);
    }

    private void DrawEnvironmentLighting()
    {
        DrawLabel("Environment Lighting", 20);

        settings.skyLightOption = DrawPopup(settings.skyLightOption, skyLightOptions, "Source", 30);

        settings.intensityLight = DrawFloatSlider(settings.intensityLight, 0, 8, "Intensity Multiple", 30);

        settings.ambientOption = DrawPopup(settings.ambientOption, ambientOptions, "Ambient Mode", 30);
    }
}

public struct PhoneSettings
{
    public Material material;
    public Light mainLight;
    public int skyLightOption;
    public int ambientOption;
    public float intensityLight;
    public int skyReflectionOption;
    public int resolutionOption;
    public int compressionOption;
    public float intensityReflection;
    public int bounces;
    public bool useRGI;
    public bool useBGI;
    public int lightModeOption;
    public int lightmapOption;
    public bool usePrioritizeView;
    public int amountOfDirectSamples;
    public int amountOfIndirectSamples;
    public int bouncesOption;
    public int filteringOption;
    public float indirectRes;
    public float lightResolution;
    public int lightPadding;
    public int lightSizeOption;
    public bool compressLight;
    public bool ambientOcclusion;
    public int directionalModeOption;
    public float indirectIntensity;
    public float albedoBoost;
}
