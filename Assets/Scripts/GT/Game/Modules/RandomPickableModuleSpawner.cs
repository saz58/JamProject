using Pool;
using UnityEngine;

namespace GT.Game.Modules
{
    public static class RandomPickableModuleSpawner
    {
        public static PickableModule CreatePickableModule(Vector2 position)
        {
            var pickableModule = PoolManager.Get<PickableModule>(nameof(PickableModule));
            pickableModule.transform.position = position;

            return pickableModule;
        }
    }
}