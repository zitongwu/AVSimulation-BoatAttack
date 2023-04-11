using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

[System.Serializable]
public class TagField{
	public bool valid;  //if tag is visible (true/false)

	[System.NonSerialized]private Color my_color = new Color(0,0,0,1);  //store unity color

	[SerializeField]private SerializableColor my_scolor = new SerializableColor(Color.black);  //store serializable color


	// serializable color
	[System.Serializable]
	public class SerializableColor{
		public float r;  //4 channels
		public float g;
		public float b;
		public float a;

		// constructor
		public SerializableColor(Color color){
			Fill(color);
		}

		//convert from unity color
		public void Fill(Color color){
			r = color.r;
			g = color.g;
			b = color.b;
			a = color.a;
		}

		// convert to unity color
		public Color ToColor(){
			return new Color (r, g, b, a);
		}
	}

	// constructor
	public TagField(bool valid, Color color){
		this.valid = valid;
		this.color = color;
	}

	// get/set color (unity color)
	public Color color{
		get{ return my_color; }
		set{
			my_color = value;
			my_scolor.Fill (my_color);
		}
	}

	// after deserialize serializable_color then update to unity_color
	public void OnAfterDeserialize(){
		my_color = my_scolor.ToColor ();
	}
}


// serializable key value pair
[System.Serializable]
public struct SerializableKeyValuePair<K, V>
{
	public K Key { get; set;}
	public V Value { get; set;}
}


[System.Serializable]
public static class TagsManager{

	public static string savingPath = Application.streamingAssetsPath;
	public static string savingFileName = "SegmentationSettings.bytes";


	public static Dictionary<string, TagField> tagfield = null;
	public static string[] taglist = null;

	public static Dictionary<string, TagField> TagInfo
	{
		get{
			if (tagfield == null)
				UpdateTags ();
			return tagfield; 
		}
		set{ 
			tagfield = value; 
		}
	}



	public static string[] Tags
	{
		get{ 
			if (taglist == null)
				UpdateTags ();
			return taglist; 
		}
		set{ 
			taglist = value; 
		}
	}

	public static string FileName
	{
		get { return savingFileName; }
		set { 
			if (value == null || value == "")
				return;
			savingFileName = value;
		}
	}

	public static void UpdateTagList()
	{
		#if UNITY_EDITOR
		taglist = UnityEditorInternal.InternalEditorUtility.tags;
		#endif

		//dump from tagfied
		if (taglist == null) {
			taglist = new string[tagfield.Keys.Count];
			tagfield.Keys.CopyTo (taglist, 0);
		}
			
	}

	public static void UpdateTagField()
	{
		var newtagfield = new Dictionary<string, TagField> ();


		foreach (string t in taglist) {
			if (tagfield.ContainsKey (t)) {
				newtagfield [t] = tagfield [t];
			} else {
				newtagfield.Add (t, new TagField (true, Color.black));
			}
		}

		tagfield = newtagfield;
	}

	public static void UpdateTags()
	{

		if (taglist == null && tagfield == null) {
			InitializeTags ();
		} else {
			UpdateTagList ();
			UpdateTagField ();
		}

		#if UNITY_EDITOR
		SaveToResourcesFolder(savingFileName);
		#endif
	}


	public static void InitializeTags()
	{
		LoadFromResourcesFolder (savingFileName);
		UpdateTagList ();
	}

	public static void ClearTags()
	{
		tagfield = new Dictionary<string, TagField> ();

		foreach (string t in taglist) {
			tagfield.Add (t, new TagField (true, Color.gray));
		}
	}

	public static TagField GetTagField(string tag)
	{
		if (tagfield != null && tagfield.ContainsKey (tag)) {
			return tagfield [tag];
		} else {
			UpdateTags ();
			if (tagfield.ContainsKey (tag)) {
				return tagfield [tag];
			}
			Debug.LogError ("tag " + tag + " not found!");
			return null;
		}
	}

	public static Color GetColor(string tag)
	{
		TagField field = GetTagField (tag);
		if (field == null)
			return Color.gray;
		else
			return field.color;
	}

	public static void SetColor(string tag, Color color)
	{
		TagField field = GetTagField (tag);
		if (field == null)
			return;
		else
			field.color = color;
	}

	public static bool GetValid(string tag)
	{
		TagField field = GetTagField (tag);
		if (field == null)
			return false;
		else
			return field.valid;
	}

	public static void SetValid(string tag, bool valid)
	{
		TagField field = GetTagField (tag);
		if (field == null)
			return;
		else
			field.valid = valid;
	}

	public static void SetAllValid()
	{
		foreach (var pair in tagfield) {
			pair.Value.valid = true;
		}
	}

	public static void SetRevertAllValid()
	{
		foreach (var pair in tagfield) {
			pair.Value.valid = !pair.Value.valid;
		}
	}

	public static void SetRandomColor()
	{
		int count = 0;

		foreach (var pair in tagfield) {
			float hue = (float)count / (float)tagfield.Count*3;
			float value = Mathf.Lerp (1, 0, Mathf.Floor(hue) / 5.0f * 0.8f);
			hue = hue - Mathf.Floor(hue) * 0.9f;

			pair.Value.color = Color.HSVToRGB (hue, 1, value);

			count++;
		}
	}

	public static void AutoColor (){
		SetRandomColor ();
	}

	public static void SaveToResourcesFolder(string path){
		if (!Directory.Exists (savingPath)) {
			Directory.CreateDirectory (savingPath);
		}

		Save (Path.Combine(savingPath, path));
	}



	public static void LoadFromResourcesFolder(string path){
		if (!Directory.Exists (savingPath)) {
			Directory.CreateDirectory (savingPath);
			SaveToResourcesFolder (path);
			return;
		}

		Load (Path.Combine(savingPath, path));
	}

	public static void Save(string path)
	{
		try{
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Create (path);

			var arr = new SerializableKeyValuePair<string, TagField>[tagfield.Count];
			int start_count = 0;
			foreach(var pair in tagfield){
				arr [start_count] = new SerializableKeyValuePair<string, TagField> ();
				arr [start_count].Key = pair.Key;
				arr [start_count++].Value = pair.Value;
			}

			bf.Serialize (file, arr);

			file.Close ();
		}
		catch(FileNotFoundException e)
		{
			Debug.LogException (e);
		}
		catch(IOException e) 
		{
			Debug.LogException (e);
		}
	}

	public static void Load(string path)
	{

		if(tagfield == null)
			tagfield = new Dictionary<string, TagField>();

		try{
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (path, FileMode.Open);
			var arr = (SerializableKeyValuePair<string, TagField>[])bf.Deserialize (file);
			file.Close ();



			foreach (var pair in arr) {
				
				// Convert Serialized Color to unity color
				pair.Value.OnAfterDeserialize();

				if(tagfield.ContainsKey(pair.Key))
				{
					tagfield[pair.Key] = pair.Value;
				}
				else
				{
					tagfield.Add (pair.Key, pair.Value);
				}
			}

		}catch{
			
			Debug.LogError ("Cannot load file " + path);
		}

		UpdateTags ();
	}
}
