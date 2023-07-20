using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dubi.BaseValues;
using UnityEngine.AddressableAssets;

namespace Dubi.SceneLoader
{
    [CreateAssetMenu(menuName ="Dubi/Scene Loader/Scene Loader Table")]
    public class SceneLoaderTableObject : GenericValueObject<int>
    {
        public AsyncSceneLoaderGroup[] AsyncSceneLoaderGroups
        {
            get => this.asyncSceneLoaderGroups;
        }

        [SerializeField] AsyncSceneLoaderGroup[] asyncSceneLoaderGroups = new AsyncSceneLoaderGroup[0]; 
        
        public void LoadScenesRuntime(int index)
        {
            this.asyncSceneLoaderGroups[index].LoadScenesAsync();
        }

        public void ReleaseScenesRuntime(int index)
        {
            this.asyncSceneLoaderGroups[index].UnloadScenesAsync();
        }

#if UNITY_EDITOR
        public void LoadScenesEditor(int index)
        {
            this.asyncSceneLoaderGroups[index].LoadScenesEditor();
        }

        public void UnloadScenesEditor(int index)
        {
            this.asyncSceneLoaderGroups[index].UnloadScenesEditor();
        }
#endif
    }
}
