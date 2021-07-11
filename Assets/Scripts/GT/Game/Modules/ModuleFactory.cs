using GT.Data.Game;
using UnityEngine;
using Pool;
using GT.Game.Swarms;
using System;

namespace GT.Game.Modules
{
    public static class ModuleFactory
    {
        public static BaseModule CreateModule(ModuleData data, SwarmFaction faction, Vector2 position, Transform parent)
        {
            var module = GetModule(data, faction);
            var returnAction = GetModuleReturnToPoolAction(data, faction);

            module.transform.SetParent(parent);
            module.transform.rotation = Quaternion.Euler(0, 0, parent.rotation.eulerAngles.z);
            module.Position = position;
            module.Initialize(data.ToStats(), returnAction);
            return module;
        }

        private static BaseModule GetModule(ModuleData data, SwarmFaction faction)
        {
            return PoolManager.Get<BaseModule>(GetPoolId(data, faction));
        }

        private static Action<BaseModule> GetModuleReturnToPoolAction(ModuleData data, SwarmFaction faction)
        {
            Action<BaseModule> returnAction = m => PoolManager.Return(GetPoolId(data, faction), m);
            
            if (faction == SwarmFaction.Wasp)
            {
                returnAction += m => AddScores(m);
            }

            return returnAction;
        }

        private static void AddScores(BaseModule baseModule)
        {
            if (baseModule is CoreModule)
            {
                ScoreManager.AddScore(GameConsts.ScoreForSwarm);
            }
            else
            {
                ScoreManager.AddScore(GameConsts.ScoreForModule);
            }
        }

        private static string GetPoolId(ModuleData data, SwarmFaction faction)
        {
            return data.Type switch
            {
                ModuleType.Attack => $"{faction}{nameof(AttackModule)}",
                ModuleType.Core => $"{faction}{nameof(CoreModule)}",
                ModuleType.Shield => $"{faction}{nameof(ShieldModule)}",
                ModuleType.Speed => $"{faction}{nameof(SpeedModule)}",
                _ => throw new ArgumentException($"Unsupported module type: {data.Type}", nameof(data))
            };
        }
    }
}