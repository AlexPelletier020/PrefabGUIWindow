using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AppleBufferSettings : IBufferInterface
{
    bool environmentFold = true;
    bool realtimeLighting = true;
    bool mixedLighting = true;
    bool lightmapping = true;

    Material mat = null;
    Light mainLight = null;
    int skyLightOption = 0;
    int ambientOption = 0;
    float intensityLight = 1;
    int skyReflectionOption = 0;
    int resolutionOption = 3;
    int compressionOption = 2;
    float intensityReflection = 1;
    int bounces = 1;

    bool useRGI = true;

    bool useBGI = true;
    int lightModeOption = 2;

    int lightmapOption = 1;
    bool usePrioritizeView = true;
    int amountOfDirectSamples = 32;
    int amountOfIndirectSamples = 500;
    int bouncesOption = 2;
    int filteringOption = 1;
    float indirectRes = 2f;
    float lightResolution = 40;
    int lightPadding = 2;
    int lightSizeOption = 3;
    bool compressLight = true;
    bool ambientOcclusion = false;
    int directionalModeOption = 1;
    float indirectIntensity = 1;
    float albedoBoost = 1;

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
        EditorGUILayout.BeginHorizontal();
        lightmapping = EditorGUILayout.Foldout(lightmapping, "Lightmapping Settings");
        EditorGUILayout.EndHorizontal();
        if (lightmapping)
        {
            EditorGUILayout.BeginHorizontal();
            string[] lightmapOptions = { "Enlighten", "Progressive CPU" };
            GUILayout.Space(20);
            lightmapOption = EditorGUILayout.Popup("Lightmapper", lightmapOption, lightmapOptions);
            EditorGUILayout.EndHorizontal();

            if (lightmapOption.Equals(1))
            {
                DrawCPUSettings();
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);
            indirectRes = EditorGUILayout.FloatField("Indirect Resolution", indirectRes);
            EditorGUILayout.LabelField("Texels per Unit");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);
            lightResolution = EditorGUILayout.FloatField("Lightmap Resolution", lightResolution);
            EditorGUILayout.LabelField("Texels per Unit");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);
            lightPadding = EditorGUILayout.IntField("Lightmap Padding", lightPadding);
            EditorGUILayout.LabelField("Texels");
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);
            string[] lightSize = { "32", "64", "128", "256", "512", "1024", "2048", "4096" };
            lightSizeOption = EditorGUILayout.Popup("Lightmap Size", lightSizeOption, lightSize);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);
            compressLight = EditorGUILayout.Toggle("Compress", compressLight);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);
            ambientOcclusion = EditorGUILayout.Toggle("Compress", ambientOcclusion);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);
            string[] directionalModeOPtions = { "Non-Directional", "Directional" };
            directionalModeOption = EditorGUILayout.Popup("Directional Mode", directionalModeOption, directionalModeOPtions);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);
            EditorGUILayout.LabelField("Indirect Intensity");
            indirectIntensity = EditorGUILayout.Slider(indirectIntensity, 0, 5);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);
            EditorGUILayout.LabelField("Albedo Boost");
            albedoBoost = EditorGUILayout.Slider(albedoBoost, 1, 10);
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
    }

    private void DrawCPUSettings()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(30);
        usePrioritizeView = EditorGUILayout.Toggle("Prioritize View", usePrioritizeView);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(30);
        amountOfDirectSamples = EditorGUILayout.IntField("Direct Samples", amountOfDirectSamples);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(30);
        amountOfIndirectSamples = EditorGUILayout.IntField("Indirect Samples", amountOfIndirectSamples);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        string[] bouncesOptions = { "none", "1", "2", "3", "4" };
        GUILayout.Space(30);
        bouncesOption = EditorGUILayout.Popup("Bounces", bouncesOption, bouncesOptions);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        string[] filteringOptions = { "none", "Auto", "Advanced" };
        GUILayout.Space(30);
        filteringOption = EditorGUILayout.Popup("Filtering", filteringOption, filteringOptions);
        EditorGUILayout.EndHorizontal();
    }

    private void DrawMixedLighting()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.BeginHorizontal();
        mixedLighting = EditorGUILayout.Foldout(mixedLighting, "Mixed Lighting");
        EditorGUILayout.EndHorizontal();
        if (mixedLighting)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);
            useBGI = EditorGUILayout.Toggle("Baked Global Illumination", useBGI);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            string[] lightingModeOptions = { "Baked Indirect", "Substractive", "Shadowmask" };
            GUILayout.Space(20);
            lightModeOption = EditorGUILayout.Popup("Lighting Mode", lightModeOption, lightingModeOptions);
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
    }

    private void DrawRealtimeLighting()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.BeginHorizontal();
        realtimeLighting = EditorGUILayout.Foldout(realtimeLighting, "Realtime Lighting");
        EditorGUILayout.EndHorizontal();
        if (realtimeLighting)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);
            useRGI = EditorGUILayout.Toggle("Realtime Global Illumination", useRGI);
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
    }

    private void DrawEnvironmentSection()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.BeginHorizontal();
        environmentFold = EditorGUILayout.Foldout(environmentFold, "Environment");
        EditorGUILayout.EndHorizontal();
        if (environmentFold)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);
            mat = (Material)EditorGUILayout.ObjectField("SkyBox Material", mat, typeof(Material));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);
            mainLight = (Light)EditorGUILayout.ObjectField("Sun Source", mainLight, typeof(Light));
            EditorGUILayout.EndHorizontal();

            DrawEnvironmentLighting();

            DrawEnvironmentReflection();
        }
        EditorGUILayout.EndVertical();
    }

    private void DrawEnvironmentReflection()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(20);
        EditorGUILayout.LabelField("Environment Reflections");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        string[] reflectionSourceOptions = { "Skybox", "Custom" };
        GUILayout.Space(30);
        skyReflectionOption = EditorGUILayout.Popup("Source", skyReflectionOption, reflectionSourceOptions);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        string[] resolutionOptions = { "16", "32", "64", "128", "256", "512", "1024", "2048" };
        GUILayout.Space(30);
        resolutionOption = EditorGUILayout.Popup("Resolution", resolutionOption, resolutionOptions);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        string[] compressionOptions = { "Uncompressed", "Compressed", "Auto" };
        GUILayout.Space(30);
        compressionOption = EditorGUILayout.Popup("Compression", compressionOption, compressionOptions);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(30);
        intensityReflection = EditorGUILayout.Slider("Intensity Multiple", intensityReflection, 0, 1);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(30);
        bounces = EditorGUILayout.IntSlider("Bounces", bounces, 1, 5);
        EditorGUILayout.EndHorizontal();
    }

    private void DrawEnvironmentLighting()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(20);
        EditorGUILayout.LabelField("Environment Lighting");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        string[] skyLightOptions = { "Skybox", "Gradient", "Color" };
        GUILayout.Space(30);
        skyLightOption = EditorGUILayout.Popup("Source", skyLightOption, skyLightOptions);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(30);
        intensityLight = EditorGUILayout.Slider("Intensity Multiple", intensityLight, 0, 8);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        string[] ambientOptions = { "Realtime", "Baked" };
        GUILayout.Space(30);
        ambientOption = EditorGUILayout.Popup("Ambient Mode", ambientOption, ambientOptions);
        EditorGUILayout.EndHorizontal();
    }
}
