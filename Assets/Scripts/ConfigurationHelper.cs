using System.Collections;
using System.Collections.Generic;
using GT.Game.Enemy;
using Libs;
using UnityEngine;

public class ConfigurationHelper : Singleton<ConfigurationHelper>
{
    public EnemyAttackData[] AttackConfigs;

    public EnemyAttackData GetAttackConfiguration(EnemyType type)
    {
        foreach (var config in AttackConfigs)
        {
            if (config.Type == type)
                return config;
        }
        return null;
    }
}
