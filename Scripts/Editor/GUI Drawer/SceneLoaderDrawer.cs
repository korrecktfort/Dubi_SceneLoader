using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Dubi.SceneLoader;

[CustomEditor(typeof(SceneLoader))]
public class SceneLoaderDrawer : Editor
{
    SceneLoader sceneLoader = null;

    private void OnEnable()
    {
        this.sceneLoader = target as SceneLoader;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
               
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Load Scenes"))
        {
            if (EditorApplication.isPlaying)
            {
                this.sceneLoader.LoadScenesRuntime();
            }
            else
            {
                this.sceneLoader.LoadScenesEditor();
            }
        }

        if(GUILayout.Button("Unload Scenes"))
        {
            if (EditorApplication.isPlaying)
            {
                this.sceneLoader.ReleaseScenesRuntime();
            }
            else
            {
                this.sceneLoader.UnloadScenesEditor();
            }
        }

        GUILayout.EndHorizontal();       
    }
}
