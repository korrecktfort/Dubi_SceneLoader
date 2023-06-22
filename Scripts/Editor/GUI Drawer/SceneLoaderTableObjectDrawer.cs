using UnityEditor;
using UnityEngine;
using Rubi.SceneLoader;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[CustomEditor(typeof(SceneLoaderTableObject))]
public class SceneLoaderTableObjectDrawer : Editor
{    
    public override void OnInspectorGUI()
    {
        GUI.enabled = false;
        EditorGUILayout.PropertyField(base.serializedObject.FindProperty("m_Script"));
        GUI.enabled = true;

        EditorGUILayout.PropertyField(base.serializedObject.FindProperty("value"));
        EditorGUILayout.PropertyField(base.serializedObject.FindProperty("constantValue"));

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(base.serializedObject.FindProperty("asyncSceneLoaderGroups"), true);

        if (EditorGUI.EndChangeCheck())
        {
            base.serializedObject.ApplyModifiedProperties();
        }       
    }
}
