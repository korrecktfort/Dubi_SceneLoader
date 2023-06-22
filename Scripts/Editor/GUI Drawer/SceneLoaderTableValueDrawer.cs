using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rubi.SceneLoader;
using UnityEditor;
using Rubi.BaseValues;

[CustomPropertyDrawer(typeof(SceneLoaderTableValue))]
public class SceneLoaderTableValueDrawer : BaseValueDrawer<SceneLoaderTableObject>
{
    protected override void DisplayValueField(Rect position, SerializedProperty property)
    {
        SerializedProperty rawValueProp = base.baseProperty.FindPropertyRelative("value");
        SerializedProperty groupsProp = base.baseProperty.FindPropertyRelative("groups");

        string[] groupsArray = new string[groupsProp.arraySize];
        for (int i = 0; i < groupsProp.arraySize; i++)
        {
            groupsArray[i] = groupsProp.GetArrayElementAtIndex(i).stringValue;
        }

        EditorGUI.BeginChangeCheck();
        position.height = EditorGUIUtility.singleLineHeight;
        int selected = EditorGUI.Popup(position, rawValueProp.intValue, groupsArray);


        if (EditorGUI.EndChangeCheck())
        {
            rawValueProp.intValue = selected;
            rawValueProp.serializedObject.ApplyModifiedProperties();

            if(EditorApplication.isPlaying)
            ((BaseValue)property.serializedObject.targetObject).Call();
            
        }
    }
}
