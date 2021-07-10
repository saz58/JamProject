using UnityEngine;

namespace Pool
{
    [CreateAssetMenu(fileName = "PoolData", menuName = "ScriptableObjects/PoolData", order = 1)]
    public sealed class PoolData : ScriptableObject, IPoolData
    {
        [SerializeField] public string Id = "";
        [SerializeField] public GameObject Prefab;
        [SerializeField] public int PreloadCount = 10;

        public string GetId => Id;

        public GameObject GetPrefab => Prefab;

        public int GetPreloadCount => PreloadCount;

        public bool Equals(IPoolData other)
        {
            return other is PoolData && GetId.Equals(other.GetId);
        }
    }
}