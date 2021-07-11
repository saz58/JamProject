using GT.Data.Game;
using GT.Game.Enemy;
using GT.Game.Modules;
using GT.Game.SwarmControls;
using Pool;
using Scene;
using UnityEngine;

namespace GT.Game.Swarms
{
    public static class SwarmFactory
    {
        public static Swarm CreatePlayer(Vector2 position)
        {
            var swarm = PoolManager.Get<Swarm>(nameof(Swarm));
            swarm.transform.position = position;

            var module = ModuleFactory.CreateModule(new CoreModuleData(200), SwarmFaction.Bee, Vector2.zero, swarm.transform);
            swarm.SetCoreModule((CoreModule)module);

            var movement = swarm.gameObject.AddComponent<MovementBehaviour>();
            movement.Setup(CacheLoader.MovementSettings.Clone(), swarm.GetComponent<Rigidbody2D>());

            var control = swarm.gameObject.AddComponent<PlayerControl>();
            var modulepiker = swarm.gameObject.AddComponent<ModulePicker>();
            modulepiker.Transform = swarm.transform;
            control.Setup(movement);

            swarm.Setup(control);
            swarm.SubscribeControls();
            
            swarm.OnDestroied += DestroyPlayer;

            RandomPickableModuleSpawner.SpawnGroupModules(new Vector2(swarm.transform.position.x + 4,swarm.transform.position.y), 5, 4);

            return swarm;
        }
        
        public static Swarm CreateEnemy(Vector2 position, EnemyType type)
        {
            var swarm = PoolManager.Get<Swarm>(nameof(Swarm));
            swarm.transform.position = position;

            var module = ModuleFactory.CreateModule(new CoreModuleData(200), SwarmFaction.Wasp, Vector2.zero, swarm.transform);
            swarm.SetCoreModule((CoreModule)module);

            var movement = swarm.gameObject.AddComponent<EnemyMovementBehaviour>();
            movement.Setup(CacheLoader.MovementEnemySettings, swarm.GetComponent<Rigidbody2D>());

            var attackBehaviour = swarm.gameObject.AddComponent<EnemyAttackBehaviour>();
            attackBehaviour.Setup(swarm, BattleScene.Player.Swarm, ConfigurationHelper.Instance.GetAttackConfiguration(type));

            var control = swarm.gameObject.AddComponent<EnemyControl>();
            control.Setup(movement);
            control.SetEnemyAttackBehaviour(attackBehaviour);

            swarm.Setup(control);

            swarm.OnDestroied += DestroyPlayer;

            return swarm;
        }

        public static void DestroyPlayer(Swarm swarm)
        {
            RemoveComponent<MovementBehaviour>();
            RemoveComponent<PlayerControl>();
            RemoveComponent<ModulePicker>();
            RemoveComponent<EnemyMovementBehaviour>();
            RemoveComponent<EnemyAttackBehaviour>();
            RemoveComponent<EnemyControl>();

            void RemoveComponent<T>()
                where T : Component
            {
                var componnt = swarm.GetComponent<T>();
                if (componnt != null)
                {
                    Object.Destroy(componnt);
                }
            }

            EnemySpawner.EnemyDestroyed(swarm);
        }
    }
}
