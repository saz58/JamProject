using GT.Data.Game;
using GT.Game.Modules;
using UnityEngine;

namespace GT.Game.Swarms
{
    [CreateAssetMenu(fileName = "SwarmFactory", menuName = "ScriptableObjects/SwarmFactory")]
    public class SwarmFactory : ScriptableObject
    {
        [SerializeField] private ModuleFactory _moduleFactory;
        [SerializeField] private Swarm _prefab;

        public Swarm CreateSwarm(Vector2 position)
        {
            var swarm = Instantiate(_prefab);
            swarm.transform.position = position;

            // TODO: get data from outside
            _moduleFactory.CreateModule(new CoreModuleData(200), Vector2.zero, swarm.transform);

            return swarm;
        }
    }
}
