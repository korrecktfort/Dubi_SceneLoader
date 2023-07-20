using System.Collections;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.ResourceProviders;
using System.Linq;
using Dubi.SingletonSpace;

namespace Dubi.SceneLoader
{
    public class LoadedSceneInstances : Singleton<LoadedSceneInstances>
    {    
        List<SceneInstance> sceneInstances = new List<SceneInstance>();
        List<string> currentLoadingScenes = new List<string>();

        /// Dependency Injection
        ComponentInjector componentInjector = new ComponentInjector();        

        public void RegisterSceneInstance(params SceneInstance[] sceneInstances)
        {
            List<SceneInstance> list = sceneInstances.ToList();
            foreach (SceneInstance instance in list)
            {
                if (!this.sceneInstances.Contains(instance))
                {
                    /// Add scene instance to list
                    this.sceneInstances.Add(instance);
                    
                    /// Dependency Injection
                    this.componentInjector.OnSceneLoaded(instance.Scene);                   
                }
            }
        }

        public void DeregisterSceneInstance(params SceneInstance[] sceneInstances)
        {
            List<SceneInstance> list = sceneInstances.ToList();
            foreach (SceneInstance instance in list)
            {
                if (this.sceneInstances.Contains(instance))
                {
                    /// Dependency Dissection
                    this.componentInjector.OnSceneUnload(instance.Scene);

                    /// Remove scene instance from list
                    this.sceneInstances.Remove(instance);
                }
            }
        }

        public SceneInstance GetSceneInstance(string internalID)
        {
            foreach (SceneInstance instance in this.sceneInstances)
            {
                if (instance.Scene.path == internalID)
                {                    
                    DeregisterSceneInstance(instance);
                    return instance;
                }
            }

            return new SceneInstance();
        }

        public bool IsCurrentlyLoading(string internalID)
        {
            return this.currentLoadingScenes.Contains(internalID);
        }

        public void RegisterCurrentLoadingScene(string internalID)
        {
            if (!this.currentLoadingScenes.Contains(internalID))
            {
                this.currentLoadingScenes.Add(internalID);
            }
        }

        public void DeregisterCurrentLoadingScene(string internalID)
        {
            if (this.currentLoadingScenes.Contains(internalID))
            {
                this.currentLoadingScenes.Remove(internalID);
            }
        }
    }
}