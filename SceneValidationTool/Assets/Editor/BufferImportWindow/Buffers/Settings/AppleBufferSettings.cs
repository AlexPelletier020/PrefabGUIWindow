using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AppleBufferSettings : AbstractSerializedFields, IBufferInterface
{
    AppleSettings settings;

    bool environmentFold = true;
    bool realtimeLighting = true;
    bool mixedLighting = true;
    bool lightmapping = true;

    readonly string[] skyLightOptions = { "Skybox", "Gradient", "Color" };
    readonly string[] ambientOptions = { "Realtime", "Baked" };
    readonly string[] reflectionSourceOptions = { "Skybox", "Custom" };
    readonly string[] resolutionOptions = { "16", "32", "64", "128", "256", "512", "1024", "2048" };
    readonly string[] compressionOptions = { "Uncompressed", "Compressed", "Auto" };
    readonly string[] lightingModeOptions = { "Baked Indirect", "Substractive", "Shadowmask" };
    readonly string[] bouncesOptions = { "none", "1", "2", "3", "4" };
    readonly string[] filteringOptions = { "none", "Auto", "Advanced" };
    readonly string[] directionalModeOptions = { "Non-Directional", "Directional" };
    readonly string[] lightSize = { "32", "64", "128", "256", "512", "1024", "2048", "4096" };
    readonly string[] lightmapOptions = { "Enlighten", "Progressive CPU" };

    public string nameOfBuffer = "Apple Buffer";
    public string version = "V.08";

    public void DrawSettingScreen()
    {
        DrawEnvironmentSection();

        DrawRealtimeLighting();

        DrawMixedLighting();

        DrawLightmapSettings();
    }

    private void DrawLightmapSettings()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        lightmapping = DrawFoldout(lightmapping, "Lightmap Settings");

        if (lightmapping)
        {
            settings.lightmapOption = DrawPopup(settings.lightmapOption, lightmapOptions, "Lightmapper", 20);

            if (settings.lightmapOption.Equals(1))
            {
                DrawCPUSettings();
            }

            settings.indirectRes = DrawFloatField(settings.indirectRes, "Indirect Resolution", 20);

            settings.lightResolution = DrawFloatField(settings.lightResolution, "Lightmap Resolution", 20);

            settings.lightPadding = DrawIntField(settings.lightPadding, "Lightmap Padding", 20);

            settings.lightSizeOption = DrawPopup(settings.lightSizeOption, lightSize, "Lightmap Size", 20);

            settings.compressLight = DrawToggle(settings.compressLight, "Compress Lightmaps", 20);

            settings.ambientOcclusion = DrawToggle(settings.ambientOcclusion, "Ambient Occlusion", 20);

            settings.directionalModeOption = DrawPopup(settings.directionalModeOption, directionalModeOptions, "Directional Mode", 20);

            settings.indirectIntensity = DrawFloatSlider(settings.indirectIntensity, 0, 5, "Indirect Intensity", 20);

            settings.albedoBoost = DrawFloatSlider(settings.albedoBoost, 1, 10, "Albedo Boost", 20);
        }
        EditorGUILayout.EndVertical();
    }

    private void DrawCPUSettings()
    {
        settings.usePrioritizeView = DrawToggle(settings.usePrioritizeView, "Prioritize View", 30);

        settings.amountOfDirectSamples = DrawIntField(settings.amountOfDirectSamples, "Direct Sample", 30);

        settings.amountOfIndirectSamples = DrawIntField(settings.amountOfIndirectSamples, "Indirect Samples", 30);

        settings.bouncesOption = DrawPopup(settings.bouncesOption, bouncesOptions, "Bounces", 30);

        settings.filteringOption = DrawPopup(settings.filteringOption, filteringOptions, "Filtering", 30);
    }

    private void DrawMixedLighting()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        mixedLighting = DrawFoldout(mixedLighting, "Mixed Lighting");

        if (mixedLighting)
        {
            settings.useBGI = DrawToggle(settings.useBGI, "Baked Global Illumination", 20);

            settings.lightModeOption = DrawPopup(settings.lightModeOption, lightingModeOptions, "Lighting Mode", 20);
        }
        EditorGUILayout.EndVertical();
    }

    private void DrawRealtimeLighting()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        realtimeLighting = DrawFoldout(realtimeLighting, "Realtime Lighting");

        if (realtimeLighting)
        {
            settings.useRGI = DrawToggle(settings.useRGI, "Realtime Global Illumination", 20);
        }
        EditorGUILayout.EndVertical();
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

public struct AppleSettings
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