using System;
using System.Collections.Generic;
using UnityEngine;

public class Swarm : MonoBehaviour
{
    [SerializeField] private ConnectorsFactory _connectorsFactory;

    private Dictionary<Vector2, BaseModule> _allModules = new Dictionary<Vector2, BaseModule>();

    private Dictionary<Vector2, ModuleConnector> _allConnectors = new Dictionary<Vector2, ModuleConnector>();

    private float _speed;

    public event Action<Vector2> OnMouseMove;
    public event Action OnMouseClick;

    public void AddModule(Vector2 position, BaseModule module)
    {
        _allModules.Add(position, module);
        module.Position = position;
        module.ConnectTo(this);
        UpdateConnectors(position);
    }

    public void IncreaseSpeed(float speedAdd)
    {
        _speed += speedAdd;
    }

    public IEnumerable<ModuleConnector> AllFreePositions()
    {
        return _allConnectors.Values;
    }

    private void UpdateConnectors(Vector2 position)
    {
        var connector = _allConnectors[position];

        connector.IsActive = false;

        foreach (var direction in BaseModule.ConnectDirrections)
        {
            var newConnectorPosition = position + direction;
            if (_allModules.TryGetValue(newConnectorPosition, out var module) && module != null)
            {
                continue;
            }
            
            if (_allConnectors.TryGetValue(newConnectorPosition, out var oldConnector) && oldConnector != null)
            {
                oldConnector.IsActive = true;
                continue;
            }

            var newConnector = _connectorsFactory.CreateConnector(newConnectorPosition, transform);
            _allConnectors[position] = newConnector;
        }
    }
}
