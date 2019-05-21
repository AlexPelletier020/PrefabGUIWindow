using System;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Silverback.EditorTools
{
	public class SceneValidatorWindow : EditorWindow
	{
		private SceneValidatorState sceneValidatorState;
		private WindowState windowState;
		private static bool validationType = false;

		private enum WindowState
		{
			Valid,
			NotExactlyOneSceneOpen,
			NoValidatorForScene
		}

		[MenuItem("Window/Scene Validation Tool", priority = -50)]
		public static void ShowWindow()
		{
			var window = GetWindow<SceneValidatorWindow>("Scene Validation");
			window.Show();
			validationType = false;
			window.SetupSceneValidator();
		}

		[MenuItem("Window/Scene Validation Tool (Current Scene)", priority = -49)]
		public static void ShowWindowForCurrentScene()
		{
			var window = GetWindow<SceneValidatorWindow>("Scene Validation Tool");
			window.Show();
			validationType = true;
			window.SetupSceneValidator(true);
		}

		void OnEnable()
		{
			if (sceneValidatorState == null)
			{
				SetupSceneValidator(validationType);
			}
			Undo.willFlushUndoRecord += Repaint;
			EditorSceneManager.sceneOpened += OnSceneOpened;
			AssemblyReloadEvents.afterAssemblyReload += OnAssemblyReloaded;
		}

		void OnDisable()
		{
			Undo.willFlushUndoRecord -= Repaint;
			EditorSceneManager.sceneOpened -= OnSceneOpened;
			AssemblyReloadEvents.afterAssemblyReload -= OnAssemblyReloaded;
		}

		void OnGUI()
		{
			var style = new GUIStyle(EditorStyles.whiteLabel)
			{
				alignment = TextAnchor.MiddleCenter,
				fontStyle = FontStyle.Bold,
			};
			EditorGUILayout.BeginVertical();
			EditorGUILayout.LabelField("Scene Validation Tool - "
							  + (SceneManager.GetActiveScene().name == string.Empty ? "Untitled" : SceneManager.GetActiveScene().name)
							  + (validationType ? " - Current Scene" : ""), style);

			switch (windowState)
			{
				case WindowState.Valid:
					DrawValidatorItemsGUI(sceneValidatorState);
					GUILayout.FlexibleSpace();
					if (GUILayout.Button("Validate All"))
					{
						sceneValidatorState.ValidateAll();
					}
					break;
				case WindowState.NotExactlyOneSceneOpen:
					EditorGUILayout.HelpBox("Only works with exactly one scene open.", MessageType.Error);
					break;
				case WindowState.NoValidatorForScene:
					EditorGUILayout.HelpBox("No validator script for this scene.", MessageType.Info);
					break;
			}

			EditorGUILayout.EndVertical();
		}

		public static void DrawValidatorItemsGUI(SceneValidatorState sceneValidatorState)
		{
			var scrollControlID = GUIUtility.GetControlID(FocusType.Passive);
			var scrollState = (ScrollState)GUIUtility.GetStateObject(typeof(ScrollState), scrollControlID);
			using (var scrollView = new GUILayout.ScrollViewScope(scrollState.scrollPosition))
			{
				scrollState.scrollPosition = scrollView.scrollPosition;
				foreach (var validationInfo in sceneValidatorState.GetValidationInfosSorted())
				{

					if (SceneValidatorDrawing.ValidationItem(validationInfo.Result, validationInfo.Description))
					{
						var validatorInstance = Activator.CreateInstance(sceneValidatorState.ValidatorClass);
						var success = (bool)validationInfo.ValidationMethod.Invoke(validatorInstance, null);
						validationInfo.Result = success ? ValidationResult.Success : ValidationResult.Fail;
					}
				}
			}
		}

		private void OnSceneOpened(Scene scene, OpenSceneMode mode)
		{
			SetupSceneValidator(validationType);
			Repaint();
		}

		private void OnAssemblyReloaded()
		{
			SetupSceneValidator(validationType);
		}

		private void SetupSceneValidator(bool state = false)
		{
			sceneValidatorState = null;
			if (EditorSceneManager.loadedSceneCount == 1)
			{
				var scenePath = EditorSceneManager.GetActiveScene().path;
				Type validatorClass = SceneValidatorReflectionUtility.GetValidatorFor(scenePath, state);
				if (validatorClass != null)
				{
					sceneValidatorState = new SceneValidatorState(validatorClass);
					windowState = WindowState.Valid;
				}
				else
				{
					windowState = WindowState.NoValidatorForScene;
				}
			}
			else
			{
				windowState = WindowState.NotExactlyOneSceneOpen;
			}
		}
	}
}