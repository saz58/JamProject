using System;
using GT.Data.Game;
using GT.Game.Connectors;
using UnityEngine;

namespace GT.Game.Modules
{
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

        public BaseModule CreateConnector(ModuleData data, Vector2 position, Transform parent)
        {
            var module = Instantiate(GetModulePrefab(data.Type), parent);
            module.Position = position;
            module.Initialize(data.ToStats());
            return module;
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
}