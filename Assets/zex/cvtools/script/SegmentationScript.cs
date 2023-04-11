using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace zex.cvtools
{

	[RequireComponent(typeof(Camera))]
	public class SegmentationScript : MonoBehaviour {

		[SerializeField, HideInInspector]
		public Shader shader=null;

		[SerializeField, HideInInspector]
		private RenderTexture m_renderTexture=null;

		[SerializeField, HideInInspector]
		private bool m_enable = true;

		[SerializeField, HideInInspector]
		private KeyCode m_onoffkey = KeyCode.None;


		private GameObject m_dummy = null;
		private Camera m_maincam;
		private Camera m_dummycam = null;
		private MaterialPropertyBlock m_propertyBlock = null;

		


		public RenderTexture renderTexture
		{
			get{ return m_renderTexture; }
			set{ 
				m_renderTexture = value; 
				if(m_dummycam != null)
					m_dummycam.targetTexture = m_renderTexture; 
			}
		}

		public bool enable
		{
			get{ return m_enable; }
			set{ 
				m_enable = value; 
				if(m_dummycam != null)
				{
					m_dummy.SetActive(enable);
					m_dummycam.enabled = enable;
				}
			}
		}

		public KeyCode enablekey
		{
			get {return m_onoffkey; }
			set {m_onoffkey = value; }
		}



		void CreateDummyCamera(){

			m_maincam = this.GetComponent<Camera>();

			if (m_dummy == null)
			{
				m_dummy = new GameObject ();
				m_dummy.name = "HiddenSegmentationCamera";
				m_dummy.transform.SetParent (this.transform);
				m_dummy.transform.localPosition = Vector3.zero;
				m_dummy.transform.localRotation = Quaternion.identity;
				m_dummy.transform.localScale = Vector3.one;

				m_dummy.hideFlags = HideFlags.HideInHierarchy;
			}
			
			if (m_dummycam == null)
			{
				m_dummycam = m_dummy.AddComponent<Camera> ();

				m_dummycam.cullingMask = m_maincam.cullingMask;
				m_dummycam.aspect = m_maincam.aspect;
				m_dummycam.nearClipPlane = m_maincam.nearClipPlane;
				m_dummycam.farClipPlane = m_maincam.farClipPlane;
				m_dummycam.fieldOfView = m_maincam.fieldOfView;
				m_dummycam.rect = m_maincam.rect;
				m_dummycam.depth = m_maincam.depth + 1;
				m_dummycam.clearFlags = CameraClearFlags.Color;
				m_dummycam.backgroundColor = Color.black;
				m_dummycam.targetTexture = m_renderTexture;
			}
		}

		// Use this for initialization
		void Start () {

			// setting shader
			if(shader == null)
				shader = Shader.Find ("Hidden/SegmentationShader");

			// create dummy camera
			CreateDummyCamera ();

			// set segmentation shader as replacement shader
			m_dummycam.SetReplacementShader (shader, "");

			// initialize property block
			m_propertyBlock = new MaterialPropertyBlock();

			UpdateMaterialPropertyBlock ();
		}

		void UpdateMaterialPropertyBlock(){
			var renderers = GameObject.FindObjectsOfType<Renderer> ();

			foreach (var r in renderers) {
				var tag = r.gameObject.tag;
				m_propertyBlock.SetColor ("_ObjectColor", TagsManager.GetColor (tag));
				r.SetPropertyBlock (m_propertyBlock);
			}
		}
		
		// Update is called once per frame
		void Update () {
			if(enable)
				UpdateMaterialPropertyBlock ();

			if(Input.GetKeyDown(m_onoffkey))
			{
				enable = !enable;
				if (enable == true)
				{
					Debug.Log("Enable segmentation shader");
				}
				else
				{
					Debug.Log("Disable segmentation shader");
				}
			}

		}

		void OnDisable()
		{
			enable = false;
		}

		void OnEnable()
		{
			enable = true;
		}

		public Camera GetDummyCamera(){
			return m_dummycam;
		}

		public void Render(){
			m_dummycam.Render ();
		}
	}

}