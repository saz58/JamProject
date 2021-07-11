using System;
using GT.Data.Game;
using GT.Game.Connectors;
using UnityEngine;
using Pool;

namespace GT.Game.Modules
{
    public static class ModuleFactory
    {
        public static BaseModule CreateModule(ModuleData data, Vector2 position, Transform parent)
        {
            BaseModule module = null;
            switch (data.Type)
            {
                case ModuleType.Attack:
                    module = PoolManager.Get<AttackModule>(nameof(AttackModule));
                    break;
                case ModuleType.Core:
                    module = PoolManager.Get<CoreModule>(nameof(CoreModule));
                    break;
                case ModuleType.Shield:
                    module = PoolManager.Get<ShieldModule>(nameof(ShieldModule));
                    break;
                case ModuleType.Speed:
                    module = PoolManager.Get<SpeedModule>(nameof(SpeedModule));
                    break;
                default:
                    module = PoolManager.Get<CoreModule>(nameof(CoreModule));
                    break;
            }
            module.transform.SetParent(parent);
            module.Position = position;
            module.Initialize(data.ToStats());
            return module;
        }
    }
}