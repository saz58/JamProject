using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Pool
{
    public sealed class PoolContainer
    {
        public delegate Transform InitialDelegate(IPoolData arg1);

        public readonly IPoolData Data;

        private readonly Queue<Transform> _available = new Queue<Transform>();
        private readonly InitialDelegate _createFunc;
        private readonly PoolManager _manager;

        public PoolContainer(IPoolData pc, InitialDelegate createFunc, PoolManager manager)
        {
            _createFunc = createFunc;
            _manager = manager;
            Data = pc;
            AddPreload(Data.GetPreloadCount);
        }

        private void AddPreload(int count)
        {
            Assert.IsFalse(count == 0, Data.GetPrefab != null ? Data.GetPrefab.name : "Prefab" + Data.GetId);
            for (var i = 0; i < count; i++)
            {
                var created = CreateObject(Data);
                created.gameObject.SetActive(false);
                _available.Enqueue(created);
            }
        }

        private Transform CreateObject(IPoolData pc)
        {
            var tr = _createFunc(pc);
            tr.name = Data.GetId;
            tr.SetParent(_manager.transform, true);
            return tr;
        }

        public void Return<T>(Transform t) where T : Component, IPoolObject
        {
            if (t == null)
                throw new NullReferenceException("Returned object can't be null");
            t.SetParent(_manager.transform, false);
            var g = t.GetComponent<T>();
            g.OnReturnToPool();
            t.gameObject.SetActive(false);
            _available.Enqueue(t);
        }

        public T Get<T>() where T : Component, IPoolObject
        {
            var g = GetHidden<T>();
            g.gameObject.SetActive(true);
            return g;
        }

        public T GetHidden<T>() where T : Component, IPoolObject
        {
            if (_available.Count == 0)
            {
                AddPreload(Data.GetPreloadCount / 2 + 1);
                Assert.IsFalse(_available.Count == 0);
            }

            var t = _available.Dequeue();
            t.SetParent(null, true);
            var g = t.GetComponent<T>();
            g.OnGetWithPool();
            return g;
        }

        public GameObject Get()
        {
            if (_available.Count == 0)
            {
                AddPreload(Data.GetPreloadCount / 2 + 1);
                Assert.IsFalse(_available.Count == 0);
            }

            var t = _available.Dequeue();
            t.SetParent(null, true);
            GameObject gameObject;
            (gameObject = t.gameObject).SetActive(true);
            return gameObject;
        }

        public void Return(Transform t)
        {
            if (t == null)
                throw new NullReferenceException("Returned object can't be null");
            t.transform.SetParent(_manager.transform, false);
            t.gameObject.SetActive(false);
            _available.Enqueue(t);
        }
    }
}