using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEditor.AddressableAssets;
using UnityEditor;

namespace Dubi.SceneLoader.CustomDrawer
{
    [CustomPropertyDrawer(typeof(ALR_NoLabel))]
    public class AssetLabelReferenceNoLabelDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            List<string> labelList = AddressableAssetSettingsDefaultObject.Settings.GetLabels();
            SerializedProperty labelString = property.FindPropertyRelative("m_LabelString");
            int index = labelList.IndexOf(labelString.stringValue);

            EditorGUI.BeginChangeCheck();
            int selected = EditorGUI.Popup(position, index, labelList.ToArray());

            if (EditorGUI.EndChangeCheck())
            {
                labelString.stringValue = labelList[selected];
                labelString.serializedObject.ApplyModifiedProperties();
            }
            
            EditorGUI.EndProperty();
        }
    }
}
