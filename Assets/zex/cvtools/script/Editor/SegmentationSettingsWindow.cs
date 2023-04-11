using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace zex.cvtools
{

	public class SegmentationSettingsWindow : EditorWindow {

		private bool ColorSettingsFoldoutIsOpen = true;
		private Vector2 scrollPosition;

		private GUILayoutOption miniButtonWidth = GUILayout.Width(40f);



		/*
		private void LeftToggleColorField(string label, bool value, Color color, out bool outvalue, out Color outcolor){
			EditorGUILayout.BeginHorizontal ();

			Rect rect = GUILayoutUtility.GetRect (18, 18);
			outvalue = EditorGUI.ToggleLeft (rect, "", value);

			outcolor = EditorGUILayout.ColorField (label, color);



			EditorGUILayout.EndHorizontal ();
		}*/



		private void ColorSettingsContents()
		{
			//   [ auto | clear ] buttone
			EditorGUILayout.BeginHorizontal ();  // <b3>

			if (GUILayout.Button ("Auto", EditorStyles.miniButtonLeft, miniButtonWidth)) {
				TagsManager.AutoColor ();
			}
			if (GUILayout.Button ("Clear", EditorStyles.miniButtonRight, miniButtonWidth)) {
				TagsManager.ClearTags();
			}

			EditorGUILayout.EndHorizontal ();   // </b3>

			TagsManager.UpdateTags();
			string[] tags = TagsManager.Tags;
			foreach (string t in tags)
			{
				Color color= EditorGUILayout.ColorField (t, TagsManager.GetColor (t));
				TagsManager.SetColor (t, color);
			}
		}


		private GUIStyle CustomFoldoutStyle()
		{
			GUIStyle foldoutStyle = new GUIStyle (EditorStyles.foldout);
			foldoutStyle.fontStyle = FontStyle.Bold;
			foldoutStyle.focused.textColor = Color.black;
			foldoutStyle.active.textColor = Color.black;
			foldoutStyle.normal.textColor = Color.black;
			foldoutStyle.onActive.textColor = Color.black;
			foldoutStyle.onFocused.textColor = Color.black;
			return foldoutStyle;
		}

		private void OnGUI()
		{
			
			EditorGUI.BeginChangeCheck (); // <b1>
			scrollPosition = GUILayout.BeginScrollView (scrollPosition); // <b2>


			ColorSettingsFoldoutIsOpen = EditorGUILayout.Foldout (ColorSettingsFoldoutIsOpen, "Color Settings", true, CustomFoldoutStyle());

			if (ColorSettingsFoldoutIsOpen)
			{
				EditorGUI.indentLevel++;  //    <I1>

				ColorSettingsContents ();

				EditorGUI.indentLevel--;  //    </ I1>
			}



			GUILayout.EndScrollView(); //    </ b2>
			EditorGUI.EndChangeCheck (); //    </ b1>
		}
	}


}

