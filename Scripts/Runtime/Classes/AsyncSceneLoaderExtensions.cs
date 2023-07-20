using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.SceneManagement;
#endif

namespace Dubi.SceneLoader
{
    public static class AsyncSceneLoaderExtensions
    {
        public static void LoadScenesByLabel(params AssetLabelReference[] labels)
        {
            foreach(AssetLabelReference label in labels)
            {
                LoadSceneByLabelString(label.labelString);
            }            
        }

        public static void LoadScenesByLabelString(params string[] labels)
        {
            foreach (string label in labels)
            {
                LoadSceneByLabelString(label);
            }
        }

        static void LoadSceneByLabelString(string labelString)
        {
            Addressables.LoadResourceLocationsAsync(labelString).Completed += (asyncHandlerResourceLocations) =>
            {
                IList<IResourceLocation> list = asyncHandlerResourceLocations.Result as IList<IResourceLocation>;

                foreach (IResourceLocation location in list)
                {
                    if (!SceneManager.GetSceneByPath(location.InternalId).isLoaded)
                    {
                        if (!LoadedSceneInstances.Instance.IsCurrentlyLoading(location.InternalId))
                        {
                            LoadedSceneInstances.Instance.RegisterCurrentLoadingScene(location.InternalId);

                            Addressables.LoadSceneAsync(location, LoadSceneMode.Additive, true).Completed += (asyncHandler) =>
                            {
                                LoadedSceneInstances.Instance.RegisterSceneInstance(asyncHandler.Result);
                                LoadedSceneInstances.Instance.DeregisterCurrentLoadingScene(location.InternalId);
                            };
                        }
                    }
                }
            };
        }

        public static void ReleaseSceneByLabel(params AssetLabelReference[] labels)
        {
            foreach(AssetLabelReference label in labels)
            {
                ReleaseScenesByLabelString(label.labelString);
            }
        }

        public static void ReleaseSceneByLabelStrings(params string[] labelStrings)
        {
            foreach (string labelString in labelStrings)
            {
                ReleaseScenesByLabelString(labelString);
            }
        }

        public static void ReleaseScenesByLabelString(string labelString)
        {
            Addressables.LoadResourceLocationsAsync(labelString).Completed += (asyncHandlerResourceLocations) =>
            {
                IList<IResourceLocation> list = asyncHandlerResourceLocations.Result as IList<IResourceLocation>;

                foreach (IResourceLocation location in list)
                {
                    if (SceneManager.GetSceneByPath(location.InternalId).isLoaded)
                    {
                        SceneInstance sceneInstance = LoadedSceneInstances.Instance.GetSceneInstance(location.InternalId);

                        if (sceneInstance.Scene != null)
                        {
                            Addressables.UnloadSceneAsync(sceneInstance, true);
                        }
                    }
                }
            };
        }


#if UNITY_EDITOR

        public static void LoadScenesEditor(params string[] labelString)
        {
            foreach(string path in GetSceneAssetPaths(labelString))
            {
                Scene scene = SceneManager.GetSceneByPath(path);
                if (!scene.isLoaded)
                {
                    EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
                }
            }
        }

        public static void UnloadScenesEditor(params string[] labelString)
        {
            foreach (string path in GetSceneAssetPaths(labelString))
            {
                Scene scene = SceneManager.GetSceneByPath(path);
                if (scene.isLoaded)
                {
                    EditorSceneManager.CloseScene(scene, true);
                }
            }
        }

        static string[] GetSceneAssetPaths(params string[] labelStrings)
        {
            List<string> scenePaths = new List<string>();

            List<AddressableAssetGroup> groups = AddressableAssetSettingsDefaultObject.Settings.groups;
            foreach (AddressableAssetGroup group in groups)
            {
                foreach (AddressableAssetEntry entry in group.entries)
                {
                    if (entry.IsScene)
                    {
                        foreach (string entryLabelString in entry.labels)
                        {
                            foreach(string arrayLabelString in labelStrings)
                            {
                                if (entryLabelString == arrayLabelString && !scenePaths.Contains(entry.AssetPath))
                                {
                                    scenePaths.Add(entry.AssetPath);
                                }
                            }                           
                        }
                    }
                }
            }

            return scenePaths.ToArray();
        }
#endif
    }
}