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
            var module = ModuleFactory.CreateModule(data, pos, _swarm.transform);
            _swarm.AddModule(pos, module);
        }

        [EditorButton]
        public void TestAdd()
        {
            var connectorPosition = _toAdd == null || !_toAdd.IsActive ? GetConnectPosition() : _toAdd.Position;

            var module = ModuleFactory.CreateModule(GetRandomModuleData(), connectorPosition, _swarm.transform);

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
                    ModuleType.Speed => new SpeedModuleData(0, 0.2f, 1f, 2, 5),
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