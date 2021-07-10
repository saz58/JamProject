using System;
using System.Collections.Generic;
using System.Linq;
using GT.Game.Connectors;
using GT.Game.Modules;
using GT.Game.SwarmControls;
using UnityEngine;

namespace GT.Game.Swarms
{
    public class Swarm : MonoBehaviour
    {
        [SerializeField] private ConnectorsFactory _connectorsFactory;
        [SerializeField] private BaseControl _baseControl;

        private Dictionary<Vector2, BaseModule> _allModules = new Dictionary<Vector2, BaseModule>(FloatComparer.Instance);
        private Dictionary<Vector2, ModuleConnector> _allConnectors = new Dictionary<Vector2, ModuleConnector>(FloatComparer.Instance);

        public float Speed { get; private set; }

        public event Action<Vector2> OnTargetPositionChanged;
        public event Action OnFire;

        private void Awake()
        {
            _baseControl.OnTargetPositionChanged += pos => OnTargetPositionChanged?.Invoke(pos);
            _baseControl.OnFire += () => OnFire?.Invoke();
        }

        public void IncreaseSpeed(float speedAdd)
        {
            Speed += speedAdd;
        }

        public void AddModule(Vector2 position, BaseModule module)
        {
            CheckIfConnectorFree(position);
            _allModules.Add(position, module);
            module.Position = position;
            module.ConnectTo(this);
            UpdateConnectors(position);
        }

        public void RemoveModule(Vector2 position)
        {
            if (!_allModules.TryGetValue(position, out var _))
            {
                return;
            }

            ProcessCascadeDestroy(position);
        }

        public IEnumerable<Vector2> FreeConnectors()
        {
            return _allConnectors.Values.Where(c => c.IsActive).Select(c => c.Position);
        }

        private void CheckIfConnectorFree(Vector2 position)
        {
            if (position == Vector2.zero)
            {
                return;
            }

            foreach (var connector in _allConnectors)
            {
                if (FloatComparer.Instance.Equals(connector.Key, position))
                {
                    return;
                }
            }

            throw new ArgumentException($"{position} is not valid connector position. Can't add module!", nameof(position));
        }

        private void UpdateConnectors(Vector2 position)
        {
            if (_allConnectors.TryGetValue(position, out var connector))
            {
                connector.IsActive = false;
            }

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
                _allConnectors.Add(newConnectorPosition, newConnector);
            }
        }

        // TODO: not effective implementation. could be time consuming
        private void ProcessCascadeDestroy(Vector2 position)
        {
            HashSet<Vector2> processed = new HashSet<Vector2>(FloatComparer.Instance);

            Stack<Vector2> processQueue = new Stack<Vector2>();
            processQueue.Push(Vector2.zero);

            while (processQueue.Count > 0)
            {
                var process = processQueue.Pop();
                if (FloatComparer.Instance.Equals(position, process))
                {
                    continue;
                }

                processed.Add(process);
                foreach (var direction in BaseModule.ConnectDirrections)
                {
                    var newConnectorPosition = process + direction;
                    if (!_allModules.TryGetValue(newConnectorPosition, out var module) || module == null)
                    {
                        continue;
                    }

                    if (processed.Contains(newConnectorPosition))
                    {
                        continue;
                    }

                    processQueue.Push(newConnectorPosition);
                }
            }

            var removeKeys = _allModules.Keys.Except(processed).ToArray();
            foreach (var key in removeKeys)
            {
                var module = _allModules[key];
                _allModules.Remove(key);
                module.Destroy();
            }

            foreach (var connector in _allConnectors.Values)
            {
                connector.IsActive = false;
            }

            foreach (var key in processed)
            {
                UpdateConnectors(key);
            }
        }

        // Helper class to compare floats with lower accuracy
        private class FloatComparer : IEqualityComparer<Vector2>
        {
            public static readonly FloatComparer Instance = new FloatComparer();

            public bool Equals(Vector2 x, Vector2 y)
            {
                return (x - y).sqrMagnitude < 0.1f;
            }

            public int GetHashCode(Vector2 obj)
            {
                return new Vector2((float)Math.Round(obj.x, 2), (float)Math.Round(obj.y, 2)).GetHashCode();
            }
        }
    }
}