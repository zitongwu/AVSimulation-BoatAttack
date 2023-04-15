using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderSwitch : MonoBehaviour
{
    public Material m_SegmentationMaterial;
    public Camera m_Camera;
    public Camera m_SegmentationCamera;
    public GameObject cloud;
    public GameObject terrain;
    public Material terrainSegmentationMat;
    public GameObject water;
    public GameObject waterPlane;
    MaterialPropertyBlock m_propertyBlock = null;
    Lidar3 lidar;
    public GameObject lighting;
    public GameObject dynamicObjects;


    void Start()
    {
        m_propertyBlock = new MaterialPropertyBlock();
        m_Camera.enabled = true;
        m_SegmentationCamera.enabled = false;
        lidar = GetComponent<Lidar3>();
    }

    void Update()
    {
        // Segmentation view
        if (Input.GetKeyDown(KeyCode.U))
        {
            UpdateSettings(true);
        }
    }

    void UpdateSettings(bool segEnabled)
    {
        if (segEnabled)
        {
            cloud.SetActive(false);

            water.SetActive(false);
            waterPlane.SetActive(true);

            m_Camera.enabled = false;
            m_SegmentationCamera.enabled = true;

            RenderSettings.fog = false;
            lidar.drawLidar = false;
            lighting.SetActive(false);
            UseSegmentationMaterial();
        }

    }


    void UseSegmentationMaterial()
    {
        var renderers = GameObject.FindObjectsOfType<Renderer>();
        TurnOffParticleSystem(dynamicObjects);
        foreach (var r in renderers)
        {
            // Update MaterialPropertyBlock
            m_propertyBlock.SetColor("_BaseColor", TagsManager.GetColor(r.gameObject.tag));
            r.SetPropertyBlock(m_propertyBlock);
            Material[] mat = r.materials;
            for (int i = 0; i < mat.Length; i++)
            {
                mat[i] = m_SegmentationMaterial;
            }
            r.materials = mat;
        } 

        for (int i = 0; i < terrain.transform.childCount; i++)
        {
            Terrain child = terrain.transform.GetChild(i).gameObject.GetComponent<Terrain>();
            child.materialTemplate = terrainSegmentationMat;
        }
    }

    void TurnOffParticleSystem(GameObject obj)
    {
        if (obj.transform.childCount != 0)
        {
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                GameObject child = obj.transform.GetChild(i).gameObject;
                if (child.GetComponent<ParticleSystem>())
                {
                    child.GetComponent<ParticleSystem>().Stop();
                }
                TurnOffParticleSystem(child);
            }
        }
    }
}
