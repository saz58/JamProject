using GT.Data.Game;
using GT.Game.Modules;
using GT.Game.SwarmControls;
using Pool;
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

            return swarm;
        }

        public static Swarm CreateEnemy(Vector2 position)
        {
            var swarm = PoolManager.Get<Swarm>(nameof(Swarm));
            swarm.transform.position = position;

            var module = ModuleFactory.CreateModule(new CoreModuleData(200), SwarmFaction.Wasp, Vector2.zero, swarm.transform);
            swarm.SetCoreModule((CoreModule)module);

            var movement = swarm.gameObject.AddComponent<EnemyMovementBehaviour>();
            movement.Setup(CacheLoader.MovementSettings, swarm.GetComponent<Rigidbody2D>());

            var control = swarm.gameObject.AddComponent<EnemyControl>();
            control.Setup(movement);

            swarm.Setup(control);

            return swarm;
        }
    }
}
