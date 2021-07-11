using Pool;
using UnityEngine;

namespace GT.Game.Modules
{
    public static class RandomPickableModuleSpawner
    {
        public static PickableModule SpawnPickableModule(Vector2 position)
        {
            var pickableModule = PoolManager.Get<PickableModule>(nameof(PickableModule));
            pickableModule.transform.position = position;

            return pickableModule;
        }

        public static void SpawnGroupModules(Vector2 centerPos)
        {
            var count = Random.Range(0, 3);
            for (int i = 0; i < count; i++)
            {
                var m = SpawnPickableModule(centerPos);
                m.AnimateDisplay(GetTargetPosition(centerPos, count + 1));
            }
        }
        
        // todo: move to extension. 
        /// <summary>
        /// Get position by radius.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        private static Vector2 GetTargetPosition(Vector2 pos, int radius)
        {
            Vector2 p = pos;
            Vector2 randPos = Random.insideUnitCircle * radius;
            p += new Vector2(randPos.x, randPos.y);
            return p;
        }
    }
}