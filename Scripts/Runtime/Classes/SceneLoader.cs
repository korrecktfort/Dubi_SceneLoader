using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Rubi.SceneLoader
{
    public class SceneLoader : MonoBehaviour
    {
        public SceneLoaderTableValue sceneLoaderTable = new SceneLoaderTableValue(0);    

        public void LoadScenesRuntime()
        {
            this.sceneLoaderTable.LoadScenesRuntime();
        }

        public void ReleaseScenesRuntime()
        {
            this.sceneLoaderTable.ReleaseScenesRuntime();
        }

#if UNITY_EDITOR
        public void LoadScenesEditor()
        {
            this.sceneLoaderTable.LoadScenesEditor();
        }

        public void UnloadScenesEditor()
        {
            this.sceneLoaderTable.UnloadScenesEditor();
        }
#endif
    }
}
