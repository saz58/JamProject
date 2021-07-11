using UnityEngine;
using Pool;
using UnityEngine.UIElements;

namespace GT.Game.Connectors
{
    public static class ConnectorsFactory
    {
        public static ModuleConnector CreateConnector(Vector2 position, Transform parent)
        {
            var connector = PoolManager.Get<ModuleConnector>(nameof(ModuleConnector));
            connector.transform.SetParent(parent);
            connector.Position = position;
            connector.IsActive = true;
            return connector;
        }

        public static void DestroyConnector(ModuleConnector connector)
        {
            connector.IsActive = false;
            PoolManager.Return(nameof(ModuleConnector), connector);
        }
    }
}