using System;
using System.Linq;
using GT.Data.Game;
using GT.Game.Modules;
using GT.Game.Swarms;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GT.Game.Enemy
{
    [Serializable]
    public class EnemyConfiguration
    {
        public float AttackblocksPersent;
        public float ShieldBlockPersent;
        public float SpeedBlockPersent;
        public float FullLevelChanse;

        public static EnemyConfiguration GetDefault()
        {
            return new EnemyConfiguration()
            {
                AttackblocksPersent = 0.33f,
                ShieldBlockPersent = 0.33f,
                SpeedBlockPersent = 0.33f,
                FullLevelChanse = 0.25f,
            };
        }

        public static EnemyConfiguration GetForType(EnemyType type)
        {       
            switch (type)
            {
                case EnemyType.Runner:
                    return new EnemyConfiguration()
                    {
                        AttackblocksPersent = 0.25f,
                        ShieldBlockPersent = 0.05f,
                        SpeedBlockPersent = 0.7f,
                        FullLevelChanse = 0.25f,
                    };

                case EnemyType.Shooter:
                    return new EnemyConfiguration()
                    {
                        AttackblocksPersent = 0.75f,
                        ShieldBlockPersent = 0.15f,
                        SpeedBlockPersent = 0.15f,
                        FullLevelChanse = 0.25f,
                    };

                case EnemyType.Walker:
                    return new EnemyConfiguration()
                    {
                        AttackblocksPersent = 0.4f,
                        ShieldBlockPersent = 0.2f,
                        SpeedBlockPersent = 0.4f,
                        FullLevelChanse = 0.25f,
                    };

                default:
                    return EnemyConfiguration.GetDefault();
            }
        }
    }

    public enum EnemyType
    {
        Walker,
        Runner,
        Shooter,
    }

    public static class RandomSwarmGenerator
    {
        public static Swarm SpawnEnemy(Vector2 spawnPosition, EnemyConfiguration enemyConfiguration, int totalModulesCound)
        {
            var enemy = SwarmFactory.CreateEnemy(spawnPosition);
            GenerateBlocks(enemy, enemyConfiguration, totalModulesCound);
            return enemy;
        }

        private static void GenerateBlocks(Swarm swarm, EnemyConfiguration enemyConfiguration, int totalModulesCound)
        {
            var modulesLeft = totalModulesCound;

            while (modulesLeft > 0)
            {
                var allFreeConnectors = swarm.FreeConnectors().ToArray();
                var spawnChanse = (int)(enemyConfiguration.FullLevelChanse * modulesLeft);
                int modulesOnLevel = GetModulesOnLevel(modulesLeft, allFreeConnectors.Length, spawnChanse);

                modulesLeft -= modulesOnLevel;

                RandomizeArray(allFreeConnectors);

                foreach (var connector in allFreeConnectors.Take(modulesOnLevel))
                {
                    var moduleData = RandomizeModule(enemyConfiguration);
                    var module = ModuleFactory.CreateModule(moduleData, SwarmFaction.Wasp, connector, swarm.transform);
                    swarm.AddModule(connector, module);
                }
            }

            static int GetModulesOnLevel(int modulesLeft, int maxModules, int spawnChanse)
            {
                if (spawnChanse <= 0 || spawnChanse > modulesLeft)
                    return modulesLeft;
                else if (spawnChanse > maxModules)
                    return maxModules;
                else
                    return spawnChanse;
            }
        }

        private static ModuleData RandomizeModule(EnemyConfiguration enemyConfiguration)
        {
            var attackChanse = enemyConfiguration.AttackblocksPersent;
            var shieldChanse = enemyConfiguration.AttackblocksPersent + enemyConfiguration.ShieldBlockPersent;

            var random = Random.value;
            if (random <= attackChanse)
            {
                return new AttackModuleData(0);
            }

            if (random > attackChanse && random <= shieldChanse)
            {
                return new ShieldModuleData(0);
            }

            return new SpeedModuleData(0, 0.2f, 1f, 2, 5);
        }

        private static void RandomizeArray(Vector2[] array)
        {
            for (var i = 0; i < array.Length; i++)
            {
                var m = Random.Range(0, array.Length);
                var n = Random.Range(0, array.Length);
                
                if (m == n)
                    continue;

                (array[m], array[n]) = (array[n], array[m]);
            }

        }
    }
}