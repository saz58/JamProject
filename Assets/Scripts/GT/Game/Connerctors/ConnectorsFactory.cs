using UnityEngine;

namespace GT.Game.Connectors
{
    [CreateAssetMenu(fileName = nameof(ConnectorsFactory), menuName = "ScriptableObjects/ConnectorsFactory")]
    public class ConnectorsFactory : ScriptableObject
    {
        [SerializeField] private ModuleConnector _prefab;

        public ModuleConnector CreateConnector(Vector2 position, Transform parent)
        {
            var connector = Instantiate(_prefab, parent);
            connector.Position = position;
            connector.IsActive = true;
            return connector;
        }
    }
}