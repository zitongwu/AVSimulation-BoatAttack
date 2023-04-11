using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace zex.cvtools.editor
{
    [CustomEditor(typeof(SegmentationScript))]
    public class SegmentationScriptEditor : Editor
    {

        private SegmentationScript m_target;


        public override void OnInspectorGUI()
        {
            m_target.renderTexture = (RenderTexture)EditorGUILayout.ObjectField(
                                            new GUIContent("Render target", "RenderTexture to render segmentation results, leave blank for None for rendering on screen."), 
                                            m_target.renderTexture, typeof(RenderTexture), true);

            //m_target.enable = EditorGUILayout.Toggle(
            //                        new GUIContent("Enable segmentation", "Toggle this to enable segmentation script."),
            //                        m_target.enable);

            m_target.enablekey = (KeyCode)EditorGUILayout.EnumPopup("Enable/Disable key", m_target.enablekey);

        }

        private void OnEnable()
        {
            m_target = (SegmentationScript) target;
        }

    }
}

