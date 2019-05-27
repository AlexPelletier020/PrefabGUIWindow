using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AOBufferSettings : AbstractSerializedFields, IBufferInterface
{
    AOSettings settings;

    bool realtimeLighting = true;
    bool mixedLighting = true;
    bool lightmapping = true;

    readonly string[] lightingModeOptions = { "Baked Indirect", "Substractive", "Shadowmask" };
    readonly string[] bouncesOptions = { "none", "1", "2", "3", "4" };
    readonly string[] filteringOptions = { "none", "Auto", "Advanced" };
    readonly string[] lightmapOptions = { "Enlighten", "Progressive CPU" };
    readonly string[] directionalModeOptions = { "Non-Directional", "Directional" };
    readonly string[] lightSize = { "32", "64", "128", "256", "512", "1024", "2048", "4096" };

    public string nameOfBuffer = "AO Buffer";
    public string version = "V.09";

    public void DrawSettingScreen()
    {
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
}

public struct AOSettings
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
