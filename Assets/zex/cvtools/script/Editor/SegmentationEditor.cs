using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace zex.cvtools
{
	public class SegmentationEditor{



		[MenuItem("Zex/Segmentation/Color Settings", false, 0)]
		private static void ColorSettingsWindow()
		{
			EditorWindow window = EditorWindow.GetWindow<SegmentationSettingsWindow> ("Analysis", true);
			window.Show ();
		}

		[MenuItem("Zex/Segmentation/Import Settings", false, 50)]
		private static void ColorSettingsImport()
		{
			string path = EditorUtility.OpenFilePanel ("Import", "", "bytes");
			if (path == null || path == "")
				return;
			TagsManager.Load (path);
			EditorWindow.GetWindow<SegmentationSettingsWindow> ("Settings", true).Repaint ();

			Debug.Log ("Import successfully");
		}

		[MenuItem("Zex/Segmentation/Export Settings", false, 51)]
		private static void ColorSettingsExport()
		{
			string path = EditorUtility.SaveFilePanel ("Export", "", TagsManager.savingFileName, "bytes");
			if (path == null || path == "")
				return;
			TagsManager.Save (path);
			Debug.Log ("Export successfully");
		}
	}
}