using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[CreateAssetMenu(fileName = "GameAssetsData", menuName = "ScriptableObjects/GameAssetsData", order = 1)]

public class GameAssetsData : ScriptableObject
{
    [SerializeField] private List<Asset> _assets;

    public List<string> ItemKeys => _assets.Select(a => a.key).ToList<string>();
    public List<Asset> Assets => _assets;
}

[Serializable]
public struct Asset
{
    public string key;
    public int count;
}
