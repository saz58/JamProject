using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace GT.Asset
{
    public static class AddressableHelper
    {
        /*Addressables.LoadAssetsAsync is also useful when used in conjunction with the Addressable label feature.
         If a label is passed in as the key, Addressables.LoadAssetsAsync loads every asset marked with that label.*/
        public static IEnumerator LoadByLabel<T>(IResourceReferenceHolder referenceHolder, string label,
            Action<IList<T>> onLoadSuccess)
        {
            var counter = 0;
            var handle = Addressables.LoadAssetsAsync<T>(
                label, obj => { counter++; });

            yield return handle;

            Debug.Log($"Loaded {counter} assets by label {label}: {handle.Status}");
            referenceHolder.DestroyResource += () => { Addressables.Release(handle); };
            onLoadSuccess?.Invoke(handle.Result);
        }

        public static void ClearCachedAssetBundles(string key)
        {
            // todo: check fix: with new Addressables version.
            // Clear all cached AssetBundles 
            // Addressables.ClearDependencyCacheAsync(key, true);
            // after fix: https://forum.unity.com/threads/cleardependencycacheasync-appears-to-do-nothing.934089/#post-6864323
        }

        public static IEnumerator InstantiateAsset<T>(IResourceReferenceHolder referenceHolder, string key,
            Transform parent, Action<T> created)
        {
            AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(key, parent);
            yield return handle;

            if (handle.Result != null)
            {
                var asset = handle.Result.GetComponent<T>();
                referenceHolder.DestroyResource = () => { Addressables.Release(handle); };
                created?.Invoke(asset);
            }
            else
            {
                Addressables.Release(handle);
            }
        }

        public static IEnumerator LoadListResources<T>(IResourceReferenceHolder referenceHolder, AssetPrefix prefix,
            int count, List<T> list, Action listPacked, Action onFail = null)
        {
            for (var i = 0; i < count; i++)
                LoadAtlasedSprite<T>(referenceHolder, $"{prefix}{i}", list.Add, onFail);

            while (count > list.Count) yield return null;

            listPacked?.Invoke();
        }

        public static void LoadAtlasedSprite<T>(IResourceReferenceHolder holder, string key, Action<T> onLoad,
            Action onFail = null)
        {
            string address = $"{AtlasAddress.UIAtlas}[{key}]";
            var handler = Addressables.LoadAssetAsync<T>(address);
            holder.DestroyResource += () => Addressables.Release(handler);
            handler.Completed += (handle) =>
            {
                switch (handle.Status)
                {
                    case AsyncOperationStatus.Succeeded:
                        onLoad?.Invoke(handle.Result);
                        break;
                    case AsyncOperationStatus.Failed:
                        Debug.LogError($"Failed load atlas sprite {key}");
                        onFail?.Invoke();
                        break;
                }
            };
        }

        public static void LoadAsset<T>(IResourceReferenceHolder referenceHolder, string key, Action<T> onLoad,
            Action onFail = null)
        {
            AsyncOperationHandle<T> handler = Addressables.LoadAssetAsync<T>(key);
            referenceHolder.DestroyResource += () => { Addressables.Release(handler); };
            handler.Completed += handle =>
            {
                switch (handle.Status)
                {
                    case AsyncOperationStatus.Succeeded:
                        onLoad?.Invoke(handle.Result);
                        break;
                    case AsyncOperationStatus.Failed:
                        Debug.LogError($"Failed loading Unity Asset {key}");
                        onFail?.Invoke();
                        break;
                }
            };
        }

        /// <summary> Runtime loading single asset. </summary>
        /// <param name="key">addressable asset key/name</param>
        /// <param name="onLoad">on load action with T, operation handler for manual handle.</param>
        /// <typeparam name="T">Asset Unity type</typeparam>
        /// <typeparam name="TK">struct type for key</typeparam>
        public static void LoadAsset<T, TK>(TK key, Action<T, AsyncOperationHandle<T>> onLoad) where TK : struct
        {
            AsyncOperationHandle<T> handler = Addressables.LoadAssetAsync<T>(key.ToString());
            handler.Completed += handle =>
            {
                switch (handle.Status)
                {
                    case AsyncOperationStatus.Succeeded:
                        onLoad?.Invoke(handle.Result, handler);
                        break;
                    case AsyncOperationStatus.Failed:
                        Debug.LogError($"Failed loading Unity Asset {key}");
                        break;
                }
            };
        }

        /// <summary> Runtime loading single asset. </summary>
        /// <param name="key"></param>
        /// <param name="onLoad">on load action with T, operation handler for manual handle.</param>
        /// <typeparam name="T"></typeparam>
        public static void LoadAsset<T>(string key, Action<T, AsyncOperationHandle<T>> onLoad)
        {
            AsyncOperationHandle<T> handler = Addressables.LoadAssetAsync<T>(key);
            handler.Completed += handle =>
            {
                switch (handle.Status)
                {
                    case AsyncOperationStatus.Succeeded:
                        onLoad?.Invoke(handle.Result, handler);
                        break;
                    case AsyncOperationStatus.Failed:
                        Debug.LogError($"Failed loading Unity Asset {key}");
                        break;
                }
            };
        }

        public static IEnumerator LoadAssetsByEnum<T1, T2>(IResourceReferenceHolder referenceHolder,
            Action<Dictionary<T1, T2>> dictCompleted, Action onFail = null, bool atlased = false)
            where T1 : struct, IConvertible
        {
            var enumValues = Enum.GetValues(typeof(T1));
            var counter = 0;
            Dictionary<T1, T2> dict = new Dictionary<T1, T2>();

            if (atlased)
            {
                for (int i = 0; i < enumValues.Length; i++)
                {
                    var e = enumValues.GetValue(i);
                    LoadAtlasedSprite<T2>(referenceHolder, $"{e}", o =>
                    {
                        counter++;
                        dict.Add((T1) e, o);
                    }, onFail);
                }
            }
            else
            {
                for (int i = 0; i < enumValues.Length; i++)
                {
                    var e = enumValues.GetValue(i);
                    LoadAsset<T2>(referenceHolder, $"{e}", o =>
                    {
                        counter++;
                        dict.Add((T1) e, o);
                    }, onFail);
                }
            }

            while (counter < enumValues.Length)
            {
                yield return null;
            }

            dictCompleted.Invoke(dict);
        }

        #region Scene

        public static void LoadSceneByKey(string key, Action onLoad, Action<float> progressUpdate)
        {
            GameApplication.Instance.StartCoroutine(LoadScene());

            IEnumerator LoadScene()
            {
                AsyncOperationHandle<SceneInstance> asyncOperationHandle =
                    Addressables.LoadSceneAsync($"{key}", LoadSceneMode.Single);

                while (asyncOperationHandle.IsDone == false)
                {
                    yield return new WaitForSeconds(.1f);
                    // Debug.Log(_asyncOperationHandle.PercentComplete);
                    progressUpdate?.Invoke(asyncOperationHandle.PercentComplete);
                    yield return null;
                }

                yield return asyncOperationHandle;

                Debug.Log($"Loaded scene by key {key} status {asyncOperationHandle.Status}");

                switch (asyncOperationHandle.Status)
                {
                    case AsyncOperationStatus.Succeeded:
                        Debug.Log($"{asyncOperationHandle.Result.Scene.name} successfully loaded");

                        onLoad?.Invoke();
                        break;
                    case AsyncOperationStatus.None:
                    case AsyncOperationStatus.Failed:
                        Debug.LogError($" Failed loading scene by key: {key}.");
                        break;
                }
            }
        }

        #endregion
    }
}