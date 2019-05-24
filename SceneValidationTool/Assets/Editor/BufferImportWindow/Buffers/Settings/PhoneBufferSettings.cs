

using UnityEditor;
using UnityEngine;

public class PhoneBufferSettings : IBufferInterface
{
    bool environmentFold = true;

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

    public string nameOfBuffer = "Phone Buffer";
    public string version = "V.12";

    public void DrawSettingScreen()
    {
        DrawEnvironmentSection();
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
