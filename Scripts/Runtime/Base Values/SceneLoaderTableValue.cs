using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rubi.BaseValues;

namespace Rubi.SceneLoader
{
    [System.Serializable]
    public class SceneLoaderTableValue : GenericBaseValue<int, SceneLoaderTableObject, BaseValueUpdater>, ISerializationCallbackReceiver
    {

#if UNITY_EDITOR
        [SerializeField] string[] groups = new string[0];
#endif

        public SceneLoaderTableValue(int value) : base(value, true)
        {
        }

        public void LoadScenesRuntime()
        {
            base.ValueObject.LoadScenesRuntime(this.RawValue);
        }
        public void ReleaseScenesRuntime()
        {
            base.ValueObject.ReleaseScenesRuntime(this.RawValue);
        }

#if UNITY_EDITOR
        public void LoadScenesEditor()
        {
            base.ValueObject.LoadScenesEditor(this.RawValue);
        }

        public void UnloadScenesEditor()
        {
            base.ValueObject.UnloadScenesEditor(this.RawValue);
        }
#endif

        public void OnAfterDeserialize()
        {

        }

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if(base.ValueObject != null)
            {
                AsyncSceneLoaderGroup[] cached = base.ValueObject.AsyncSceneLoaderGroups;
                int length = cached.Length;
                this.groups = new string[length];
                for (int i = 0; i < length; i++)
                {
                    this.groups[i] = cached[i].groupName;
                }
            }
#endif
        }

    }
}
