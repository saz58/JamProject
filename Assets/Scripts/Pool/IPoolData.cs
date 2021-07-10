using System;
using UnityEngine;

namespace Pool
{
    public interface IPoolData : IEquatable<IPoolData>
    {
        string GetId { get; }
        GameObject GetPrefab { get; }
        int GetPreloadCount { get; }
    }
}
