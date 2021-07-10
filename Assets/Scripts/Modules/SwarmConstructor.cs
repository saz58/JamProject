using System;
using System.Linq;
using Modules;
using UnityEngine;
using Random = UnityEngine.Random;

public class SwarmConstructor : MonoBehaviour
{
    [SerializeField] private Swarm _swarm;
    [SerializeField] private ModuleFactory _midulesFactory;

    [SerializeField] private BaseModule _toDestroy;
    [SerializeField] private ModuleConnector _toAdd;

    private void Awake()
    {
        var module = _midulesFactory.CreateConnector(Modules.ModuleType.Core, Vector2.zero, _swarm.transform);
        _swarm.AddModule(Vector2.zero, module);
    }

    [EditorButton]
    public void TestAdd()
    {
        var connectorPosition = _toAdd == null || !_toAdd.IsActive ? _swarm.FreeConnectors().First() : _toAdd.Position;

        var allTypes = Enum.GetValues(typeof(ModuleType))
            .Cast<ModuleType>()
            .Except(new[] { ModuleType.Core }).ToArray();

        var randomType = allTypes[Random.Range(0, allTypes.Length)];


        var module = _midulesFactory.CreateConnector(randomType, connectorPosition, _swarm.transform);

        _swarm.AddModule(connectorPosition, module);
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
