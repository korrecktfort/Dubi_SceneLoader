using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using Rubi.SceneLoader.CustomDrawer;

namespace Rubi.SceneLoader
{    
    /// TO DO: Unloading and Loading Scenes in EDITOR

    [System.Serializable]
    public class AsyncSceneLoaderGroup : ISerializationCallbackReceiver
    {
        [SerializeField] public string groupName = "";
        [SerializeField, ALR_NoLabel] public AssetLabelReference[] labelReferences = new AssetLabelReference[0];

#if UNITY_EDITOR
        [SerializeField] string[] labelStrings = new string[0];
#endif

        public void LoadScenesAsync()
        {
            AsyncSceneLoaderExtensions.LoadScenesByLabel(this.labelReferences);
        }


        public void UnloadScenesAsync()
        {
            AsyncSceneLoaderExtensions.ReleaseSceneByLabel(this.labelReferences);
        }

#if UNITY_EDITOR
        public void LoadScenesEditor()
        {
            AsyncSceneLoaderExtensions.LoadScenesEditor(this.labelStrings);
        }

        public void UnloadScenesEditor()
        {
            AsyncSceneLoaderExtensions.UnloadScenesEditor(this.labelStrings);
        }
#endif

        public void OnAfterDeserialize()
        {
#if UNITY_EDITOR            
#endif
        }

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            int length = this.labelReferences.Length;
            this.labelStrings = new string[length];
            for (int i = 0; i < length; i++)
            {
                this.labelStrings[i] = this.labelReferences[i].labelString;
            }
#endif
        }
    }
}