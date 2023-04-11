using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderSwitch : MonoBehaviour
{
    public Material m_SegmentationMaterial;
    Color m_SegmentationSkyColor;
    Camera m_Camera;
    Dictionary<string, Material> m_MaterialDictionary;
    Dictionary<string, Color> m_ColorDictionary;
    MaterialPropertyBlock m_propertyBlock = null;

    void Start()
    {
        m_MaterialDictionary = new Dictionary<string, Material>();
        m_ColorDictionary = new Dictionary<string, Color>();
        var renderers = GameObject.FindObjectsOfType<Renderer>();
        foreach (var r in renderers)
        {
            if (!m_MaterialDictionary.ContainsKey(r.gameObject.tag))
            {
                m_MaterialDictionary.Add(r.gameObject.tag, r.material);
                m_ColorDictionary.Add(r.gameObject.tag, r.material.GetColor("_BaseColor"));
            }
        }

        m_propertyBlock = new MaterialPropertyBlock();
        m_Camera = Camera.main;
        m_SegmentationSkyColor = new Color(0.5f, 0.74f, 1f, 1f);

    }

    void Update()
    {
        // Default view
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UpdateSettings(false, false);
        }
        // Segmentation view
        if (Input.GetKeyDown(KeyCode.E))
        {
            UpdateSettings(true, false);
        }
    }

    void UpdateSettings(bool segEnabled, bool depthEnabled)
    {
        if (segEnabled)
        {
            UseSegmentationMaterial();
            m_Camera.clearFlags = CameraClearFlags.SolidColor;
            m_Camera.backgroundColor = m_SegmentationSkyColor;
        }
        else
        {
            UseDefaultMaterial();
            m_Camera.clearFlags = CameraClearFlags.Skybox;
        }

    }

    void UseDefaultMaterial()
    {
        var renderers = GameObject.FindObjectsOfType<Renderer>();

        foreach (var r in renderers)
        {
            r.material = m_MaterialDictionary[r.gameObject.tag];
            m_propertyBlock.SetColor("_BaseColor", m_ColorDictionary[r.gameObject.tag]);
            r.SetPropertyBlock(m_propertyBlock);
        }
    }


    void UseSegmentationMaterial()
    {
        var renderers = GameObject.FindObjectsOfType<Renderer>();

        foreach (var r in renderers)
        {
            // Update MaterialPropertyBlock
            //m_propertyBlock.SetColor("_BaseColor", TagsManager.GetColor(r.gameObject.tag));
            r.SetPropertyBlock(m_propertyBlock);

            r.material = m_SegmentationMaterial;
        }
    }
}
