using GT.Data.Game;
using UnityEngine;
using Pool;
using GT.Game.Swarms;

namespace GT.Game.Modules
{
    public static class ModuleFactory
    {
        public static BaseModule CreateModule(ModuleData data, SwarmFaction faction, Vector2 position, Transform parent)
        {
            BaseModule module = data.Type switch
            {
                ModuleType.Attack => PoolManager.Get<AttackModule>($"{faction}{nameof(AttackModule)}"),
                ModuleType.Core => PoolManager.Get<CoreModule>($"{faction}{nameof(CoreModule)}"),
                ModuleType.Shield => PoolManager.Get<ShieldModule>($"{faction}{nameof(ShieldModule)}"),
                ModuleType.Speed => PoolManager.Get<SpeedModule>($"{faction}{nameof(SpeedModule)}"),
                _ => PoolManager.Get<CoreModule>($"{faction}{nameof(CoreModule)}"),
            };

            module.transform.SetParent(parent);
            module.transform.rotation = Quaternion.Euler(0, 0, parent.rotation.eulerAngles.z);
            module.Position = position;
            module.Initialize(data.ToStats());
            return module;
        }
    }
}