using UnityEngine;

namespace Pool
{
    public struct PoolDataGoStruct : IPoolData
    {
        public string GetId { get; private set; }
        public GameObject GetPrefab { get; private set; }
        public int GetPreloadCount { get; private set; }

        public PoolDataGoStruct(GameObject getPrefab, int getPreloadCount) : this()
        {
            GetId = getPrefab.GetHashCode().ToString();
            GetPrefab = getPrefab;
            GetPreloadCount = getPreloadCount;
        }

        public bool Equals(IPoolData other)
        {
            if (!(other is PoolDataGoStruct))
                return false;
            if (GetPrefab != null)
                return GetPrefab == other.GetPrefab;
            return GetId.Equals(other.GetId);
        }
    }
}
