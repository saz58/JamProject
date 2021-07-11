using System;
using System.Linq;
using GT.Data.Game;
using GT.Game.Connectors;
using GT.Game.Modules;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GT.Game.Swarms
{
    public class SwarmConstructor : MonoBehaviour
    {
        [SerializeField] private Swarm _swarm;

        [Header("Editor tests")]
        [SerializeField] private BaseModule _toDestroy;
        [SerializeField] private ModuleConnector _toAdd;

        public void AddModule(ModuleData data, Vector2 pos)
        {
            var module = ModuleFactory.CreateModule(data, SwarmFaction.Bee, pos, _swarm.transform);
            _swarm.AddModule(pos, module);
        }

        [EditorButton]
        public void AddCoreModule()
        {
            var module = ModuleFactory.CreateModule(new CoreModuleData(0), SwarmFaction.Bee, Vector2.zero, _swarm.transform);
            _swarm.SetCoreModule((CoreModule)module);
        }

        [EditorButton]
        public void TestAdd()
        {
            var connectorPosition = _toAdd == null || !_toAdd.IsActive ? GetConnectPosition() : _toAdd.Position;

            var module = ModuleFactory.CreateModule(GetRandomModuleData(), SwarmFaction.Bee, connectorPosition, _swarm.transform);

            _swarm.AddModule(connectorPosition, module);

            Vector2 GetConnectPosition()
            {
                var allConnectors = _swarm.FreeConnectors().ToArray();
                return allConnectors[Random.Range(0, allConnectors.Length)];
            }

            static ModuleData GetRandomModuleData()
            {
                var allTypes = Enum.GetValues(typeof(ModuleType))
                .Cast<ModuleType>()
                .Except(new[] { ModuleType.Core }).ToArray();

                var randomType = allTypes[Random.Range(0, allTypes.Length)];

                return randomType switch
                {
                    ModuleType.Core => new CoreModuleData(0),
                    ModuleType.Attack => new AttackModuleData(0),
                    ModuleType.Shield => new ShieldModuleData(0),
                    ModuleType.Speed => new SpeedModuleData(0),
                };
            }
        }

        [EditorButton]
        public void TestRemove()
        {
            if (_toDestroy == null)
            {
                return;
            }

            _swarm.RemoveModule(_toDestroy.Position);
        }
    }
}