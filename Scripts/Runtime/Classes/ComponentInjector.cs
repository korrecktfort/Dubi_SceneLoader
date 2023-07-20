using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dubi.SceneLoader
{
    public class ComponentInjector
    {
        public List<ISourceComponent> stackedSources = new List<ISourceComponent>();
        public List<IReceiveComponent> stackedReceivers = new List<IReceiveComponent>();

        public void OnSceneLoaded(Scene scene)
        {
            GameObject[] rootObjects = scene.GetRootGameObjects();
            List<ISourceComponent> newSources = GetComponents<ISourceComponent>(rootObjects);
            List<IReceiveComponent> newReceivers = GetComponents<IReceiveComponent>(rootObjects);

            /// inject new sources into old receivers (bad design descisions might lead to this)
            Inject(newSources, this.stackedReceivers);

            /// stack sources
            AddComponents(newSources, this.stackedSources);

            /// inject all sources into new receivers
            Inject(this.stackedSources, newReceivers);

            /// add new receivers to stack
            AddComponents(newReceivers, this.stackedReceivers);
        }

        public void OnSceneUnload(Scene scene)
        {
            GameObject[] rootObjects = scene.GetRootGameObjects();
            List<ISourceComponent> oldSources = GetComponents<ISourceComponent>(rootObjects);
            List<IReceiveComponent> oldReceivers = GetComponents<IReceiveComponent>(rootObjects);

            RemoveComponents(oldReceivers, this.stackedReceivers);
            RemoveComponents(oldSources, this.stackedSources);

            /// dissect old sources from stacked receivers
            Dissect(oldSources, this.stackedReceivers);

            /// dissect all sources from old receivers
            Dissect(this.stackedSources, oldReceivers);
            Dissect(oldSources, oldReceivers);            
        }
      
        
        void Inject(List<ISourceComponent> sources, List<IReceiveComponent> receivers)
        {
            foreach(IReceiveComponent receiver in receivers)
            {
                foreach(ISourceComponent source in sources)
                {
                    receiver.InjectComponent(source);
                }
            }
        }

        void Dissect(List<ISourceComponent> sources, List<IReceiveComponent> receivers)
        {
            foreach (IReceiveComponent receiver in receivers)
            {
                foreach (ISourceComponent source in sources)
                {
                    receiver.DissectComponent(source);
                }
            }
        }


        void AddComponents<T>(List<T> newEntries, List<T> stack)
        {
            if(newEntries != null)
            {
                foreach(T entry in newEntries)
                {
                    if (!stack.Contains(entry))
                    {
                        stack.Add(entry);
                    }
                }
            }
        }

        void RemoveComponents<T>(List<T> oldEntries, List<T> stack)
        {
            if (oldEntries != null)
            {
                foreach (T entry in oldEntries)
                {
                    if (stack.Contains(entry))
                    {
                        stack.Remove(entry);
                    }
                }
            }
        }

        List<T> GetComponents<T>(GameObject[] objects, bool children = true)
        {
            if (objects != null && objects.Length > 0)
            {
                List<T> components = new List<T>();
                foreach (GameObject obj in objects)
                {
                    T component = children ? obj.GetComponentInChildren<T>() : obj.GetComponent<T>();
                    if (component != null && !components.Contains(component))
                    {
                        components.Add(component);
                    }
                }

                return components;
            }

            return null;
        }
    }
}