using System;
using System.Collections.Generic;
using UnityEngine;
using Libs;

namespace Pool
{
    public interface IPoolObject
    {
        void OnGetWithPool();
        void OnReturnToPool();
    }

    [SingletonAttribute(false)]
    public sealed class PoolManager : Singleton<PoolManager>
    {
        private readonly List<PoolContainer> _pools = new List<PoolContainer>();

        private void Awake()
        {
            AwakeInstance(this);
        }

        public PoolContainer GetPool(string id)
        {
            foreach (var d in _pools)
            {
                if (d.Data.GetId.Equals(id))
                    return d;
            }

            throw new NullReferenceException("Pool not found " + id);
        }

        private PoolContainer GetPool(IPoolData id)
        {
            foreach (var d in _pools)
            {
                if (d.Data.Equals(id))
                    return d;
            }

            throw new NullReferenceException("Pool not found " + id + id?.GetId);
        }

        private bool IsRegistered(IPoolData data)
        {
            foreach (var d in _pools)
            {
                if (d.Data.Equals(data))
                    return true;
            }

            return false;
        }

        private bool IsRegistered(string strid)
        {
            foreach (var d in _pools)
            {
                if (d.Data.GetId.Equals(strid))
                    return true;
            }

            return false;
        }

        public void Register(IPoolData pc, PoolContainer.InitialDelegate provider = null)
        {
            if (pc == null)
            {
                throw new NullReferenceException("IPoolData can't be null");
            }

            if (IsRegistered(pc))
            {
                return;
            }

            if (pc.GetPrefab == null)
            {
                throw new NullReferenceException("IPoolData GetPrefab can't be null " + pc.GetId);
            }

            AddPool(new PoolContainer(pc, provider ?? BaseCreateObject, this));
        }

        private void AddPool(PoolContainer poolContainer)
        {
            _pools.Add(poolContainer);
            OnRegister?.Invoke(poolContainer.Data.GetId, poolContainer.Data.GetPreloadCount);
        }

        public static event Action<string> OnGet;
        public static event Action<string> OnReturn;
        public static event Action<string, int> OnRegister;

        public static T Get<T>(string id) where T : Component, IPoolObject
        {
            OnGet?.Invoke(id);
            return Instance.GetPool(id).Get<T>();
        }

        public static T GetHidden<T>(string id) where T : Component, IPoolObject
        {
            return Instance.GetPool(id).GetHidden<T>();
        }

        public static T Get<T>(IPoolData data) where T : Component, IPoolObject
        {
            return Instance.GetPool(data).Get<T>();
        }

        public static void Return<T>(string id, T typeObject) where T : Component, IPoolObject
        {
            OnReturn?.Invoke(id);
            Instance.GetPool(id).Return<T>(typeObject.transform);
        }

        public static GameObject Get(string id)
        {
            return Instance.GetPool(id).Get();
        }

        public static void Return(string id, Transform typeObject)
        {
            Instance.GetPool(id).Return(typeObject);
        }

        private static Transform BaseCreateObject(IPoolData pc)
        {
            return Instantiate(pc.GetPrefab).transform;
        }

        public static bool Exist(string id)
        {
            foreach (var d in Instance._pools)
            {
                if (d.Data.GetId.Equals(id))
                    return true;
            }

            return false;
        }
    }
}