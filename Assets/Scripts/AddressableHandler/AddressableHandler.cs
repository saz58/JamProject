using System;
using System.Collections.Generic;
using Libs;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

[Singleton(true)]
public sealed class AddressableHandler : Singleton<AddressableHandler>
{
    private const LogLevel LogLevel = global::LogLevel.Error;

    private readonly Dictionary<string, GameObject> _container = new Dictionary<string, GameObject>();

    private readonly List<string> _queue = new List<string>();

    public float Progress => (_queue.Count == 0 || _startedCount == 0) ? 1 : 1f - (1f * _queue.Count / _startedCount);

    public bool IsDone => _queue.Count == 0;

    private int _startedCount = 0;

    public event Action OnAllLoaded;

    public GameObject Get(string id)
    {
        if (_container.TryGetValue(id, out var go))
        {
            return go;
        }
        if (LogLevel.Contains(LogLevel.Warn))
            Debug.LogWarning("[AddressableHandler] Get null " + id);
        return null;
    }

    public bool TryGet(string id, out GameObject asset)
    {
        if (_container.TryGetValue(id, out asset))
        {
            return true;
        }
        asset = null;
        return false;
    }

    public bool GetAsync(string id, Action<GameObject> callback = null)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("assetPath cannot be null or whitespace.", nameof(id));
        }

        if (_container.TryGetValue(id, out var go))
        {
            callback?.Invoke(go);
            return false;
        }
        else
        {
            _queue.Add(id);
            if (LogLevel.Contains(LogLevel.Info))
                Debug.Log($"[AddressableHandler] GetAsync Start {id} {_queue.Count}");

            LoadExistAssets(id, callback);
            return true;
            /*Addressables.LoadResourceLocationsAsync(id).Completed += (loc) =>
            {
                if (loc.Result.Count > 0)
                {
                   LoadResourceLocationAssets(id, loc.Result, callback);
                }
                else
                {
                    if(LogLevel.Contains(LogLevel.Error))
                        Debug.LogErrorFormat("Reference {0} not found in Addressable System", id);
                    LoadEnd(id);
                    callback?.Invoke(null);
                }
            };*/
        }
    }

    private void LoadResourceLocationAssets(string id, IList<IResourceLocation> ids, Action<GameObject> callback = null)
    {
        var req = Addressables.LoadAssetAsync<GameObject>(ids);
        req.Completed += x =>
        {
            LoadEnd(id);
            var resGo = x.Result;
            if (resGo != null && !_container.ContainsKey(id))
            {
                _container.Add(id, resGo);
            }
            callback?.Invoke(resGo);
        };
    }

    private void LoadExistAssets(string id, Action<GameObject> callback = null)
    {
        var req = Addressables.LoadAssetAsync<GameObject>(id);
        req.Completed += x =>
        {
            var resGo = x.Result;
            if (resGo != null)
            {
                if (!_container.ContainsKey(id))
                {
                    _container.Add(id, resGo);
                }
                else
                {
                    if (LogLevel.Contains(LogLevel.Info))
                        Debug.Log($"[AddressableHandler] LoadExistAssets null  {id} {_queue.Count}");
                }
            }
            callback?.Invoke(resGo);
            LoadEnd(id);
        };
    }

    private void LoadEnd(string id)
    {
        _queue.Remove(id);
        if (LogLevel.Contains(LogLevel.Info))
            Debug.Log($"[AddressableHandler] GetAsync LoadEnd {id} {_queue.Count}");
        if (_queue.Count == 0)
        {
            _startedCount = 0;
            OnAllLoaded?.Invoke();
        }
    }

    public void GetAsync(List<string> slist)
    {
        _startedCount += slist.Count;
        var toDelete = new List<string>();
        foreach (var key in _container.Keys)
        {
            if (!slist.Contains(key))
            {
                toDelete.Add(key);
            }
        }
        foreach (var del in toDelete)
        {
            Unload(del);
        }

        foreach (var s in slist)
        {
            GetAsync(s);
        }

        if (IsDone)
        {
            _startedCount = 0;
            OnAllLoaded?.Invoke();
        }
    }

    public void GetDiffAsync(List<string> slist)
    {
        _startedCount += slist.Count;
        var diff = new List<string>();
        foreach (var s in slist)
        {
            if (!_container.ContainsKey(s))
            {
                diff.Add(s);
            }
        }

        foreach (var s in diff)
        {
            GetAsync(s);
        }

        if (IsDone)
        {
            _startedCount = 0;
            OnAllLoaded?.Invoke();
        }
    }

    public void Unload(string id)
    {
        if (LogLevel.Contains(LogLevel.Info))
            Debug.Log($"[AddressableHandler] unload {id}");
        //TODO: add unload assets, not remove
        _container.Remove(id);
    }

    public void UnloadAll()
    {
        if (LogLevel.Contains(LogLevel.Info))
            Debug.Log($"[AddressableHandler] UnloadAll");
        _container.Clear();
    }

    public bool HasAsset(string id) => _container.TryGetValue(id, out var go);
}