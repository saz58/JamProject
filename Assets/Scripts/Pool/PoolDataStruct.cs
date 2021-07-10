using UnityEngine;

namespace Pool
{
    public struct PoolDataStruct : IPoolData
    {
        public string GetId { get; private set; }
        public GameObject GetPrefab { get; private set; }
        public int GetPreloadCount { get; private set; }

        public PoolDataStruct(string getId, GameObject getPrefab, int getPreloadCount) : this()
        {
            GetId = getId;
            GetPrefab = getPrefab;
            GetPreloadCount = getPreloadCount;
        }

        public bool Equals(IPoolData other)
        {
            if (!(other is PoolDataStruct))
                return false;
            if (GetPrefab != null)
                return GetPrefab == other.GetPrefab;
            return GetId.Equals(other.GetId);
        }
    }
}
