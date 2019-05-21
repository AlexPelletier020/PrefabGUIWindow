using UnityEditor;
using UnityEngine;

namespace Silverback.EditorTools
{
	public class ScrollState
	{
		public Vector2 scrollPosition;
	}

	/// <summary>
	/// Utility functions for drawing the scene validator window
	/// </summary>
	public static class SceneValidatorDrawing
	{
		private const string CrossIconPath = "cross";
		private const string CheckIconPath = "check";
		private const string QuestionIconPath = "warning";

		public static bool ValidationItem(ValidationResult validationState, string text)
		{
			using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
			{
				var rect = GUILayoutUtility.GetRect(15f, 15f, GUILayout.ExpandWidth(true));
				switch (validationState)
				{
					case ValidationResult.Success:
						DrawIcon(rect, CheckIconPath);
						break;
					case ValidationResult.Fail:
						DrawIcon(rect, CrossIconPath);
						break;
					case ValidationResult.Unknown:
						DrawIcon(rect, QuestionIconPath);
						break;
				}
				var labelStyle = new GUIStyle(EditorStyles.label)
				{
					wordWrap = true
				};
				EditorGUILayout.LabelField(text, labelStyle);
				GUILayout.FlexibleSpace();
				return GUILayout.Button("Validate", GUILayout.MinHeight(15f));
			}
		}

		private static void DrawIcon(Rect position, string iconPath)
		{
			var tex = Resources.Load(iconPath, typeof(Texture2D)) as Texture2D;
			var style = new GUIStyle(EditorStyles.whiteLargeLabel)
			{
				alignment = TextAnchor.MiddleCenter,
				fontStyle = FontStyle.Bold,
				padding = new RectOffset(0,0,2,0)
			};
			EditorGUI.LabelField(position, new GUIContent(tex), style);
		}
	}
}