using System;
using Modules;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(ModuleFactory), menuName = "ScriptableObjects/ModuleFactory")]
public class ModuleFactory : ScriptableObject
{
    [Serializable]
    private struct ModulesPrefabs
    {
        public ModuleType Type;
        public BaseModule ModulePrefab;
    }

    [SerializeField] private ModulesPrefabs[] _prefabs;

    public BaseModule CreateConnector(ModuleType type, Vector2 position, Transform parent)
    {
        var module = Instantiate(GetModulePrefab(type), parent);
        module.Position = position;
        module.Initialize(GetStats(type));
        return module;
    }

    // TODO: should be out of here but ok to start
    private ModuleStats GetStats(ModuleType type)
    {
        return type switch
        {
            ModuleType.Attack => new AttackModuleStats(100, 50),
            ModuleType.Speed => new SpeedModuleStats(100, 2),
            ModuleType.Core => new ModuleStats(200),
            ModuleType.Shield => new ModuleStats(300),
            _ => throw new ArgumentException($"{type} is not supported!", nameof(type))
        };
    }

    private BaseModule GetModulePrefab(ModuleType type)
    {
        foreach (var item in _prefabs)
        {
            if (item.Type == type)
            {
                return item.ModulePrefab;
            }
        }

        throw new ArgumentException($"{type} is not supported!", nameof(type));
    }
}