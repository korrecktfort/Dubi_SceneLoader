using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Rubi.SceneLoader;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine.AddressableAssets;

[CustomPropertyDrawer(typeof(AsyncSceneLoaderGroup), true)]
public class AsyncSceneLoaderGroupDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        
        EditorGUI.BeginChangeCheck();
                
        Rect rect = position;
        rect.y = position.y + EditorGUIUtility.standardVerticalSpacing;
        rect.height = EditorGUIUtility.singleLineHeight;
        float leftColumnWidth = 175.0f;
        rect.width = leftColumnWidth;
        EditorGUI.PropertyField(rect, property.FindPropertyRelative("groupName"), GUIContent.none);

        rect.width = leftColumnWidth * 0.5f;
        rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        SerializedProperty labels = property.FindPropertyRelative("labelStrings");
        string[] array = new string[labels.arraySize];
        for (int i = 0; i < labels.arraySize; i++)
        {
            array[i] = labels.GetArrayElementAtIndex(i).stringValue;
        }

        if (GUI.Button(rect, "Load"))
        {            
            if (!EditorApplication.isPlaying)
            {
                /// Load Scenes Editor
                AsyncSceneLoaderExtensions.LoadScenesEditor(array);
            }
            else
            {
                /// Load Scenes Runtime                
                AsyncSceneLoaderExtensions.LoadScenesByLabelString(array);
            }
        }

        rect.x += rect.width;        
        if (GUI.Button(rect, "Unload"))
        {
            if (!EditorApplication.isPlaying)
            {
                /// Unload Scenes Editor
                AsyncSceneLoaderExtensions.UnloadScenesEditor(array);
            }
            else
            {
                /// Release Scenes Runtime
                AsyncSceneLoaderExtensions.ReleaseSceneByLabelStrings(array);
            }
        }

        rect.y = position.y + EditorGUIUtility.standardVerticalSpacing;
        rect.x += rect.width + 20.0f; 
        rect.width = EditorGUIUtility.currentViewWidth - rect.x - 10.0f;
        EditorGUI.PropertyField(rect, property.FindPropertyRelative("labelReferences"), true);


        if (EditorGUI.EndChangeCheck())
        {
            property.serializedObject.ApplyModifiedProperties();
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float listHeight = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("labelReferences"), label);
        float minHeight = EditorGUIUtility.singleLineHeight * 2.0f + EditorGUIUtility.standardVerticalSpacing * 2.0f;

        return Mathf.Max(minHeight, listHeight);
    }
}
