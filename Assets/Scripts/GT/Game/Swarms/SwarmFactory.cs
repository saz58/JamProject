using GT.Data.Game;
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
            movement.Setup(CacheLoader.MovementSettings, swarm.GetComponent<Rigidbody2D>());

            var control = swarm.gameObject.AddComponent<PlayerControl>();
            var modulepiker = swarm.gameObject.AddComponent<ModulePicker>();
            modulepiker.Transform = swarm.transform;
            control.Setup(movement);

            swarm.Setup(control);
            swarm.SubscribeControls();
            
            RandomPickableModuleSpawner.SpawnGroupModules(new Vector2(swarm.transform.position.x + 4,swarm.transform.position.y), 5, 4);
            return swarm;
        }

        public static Swarm CreateEnemy(Vector2 position)
        {
            var swarm = PoolManager.Get<Swarm>(nameof(Swarm));
            swarm.transform.position = position;

            var module = ModuleFactory.CreateModule(new CoreModuleData(200), SwarmFaction.Wasp, Vector2.zero, swarm.transform);
            swarm.SetCoreModule((CoreModule)module);

            var movement = swarm.gameObject.AddComponent<EnemyMovementBehaviour>();
            movement.Setup(CacheLoader.MovementEnemySettings, swarm.GetComponent<Rigidbody2D>());

            var attackBehaviour = swarm.gameObject.AddComponent<EnemyAttackBehaviour>();
            attackBehaviour.Setup(BattleScene.Player.Swarm);

            var control = swarm.gameObject.AddComponent<EnemyControl>();
            control.Setup(movement);
            control.SetEnemyAttackBehaviour(attackBehaviour);

            swarm.Setup(control);

            return swarm;
        }
    }
}
