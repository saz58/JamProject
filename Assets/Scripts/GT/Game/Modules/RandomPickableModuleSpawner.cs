using Pool;
using UnityEngine;

namespace GT.Game.Modules
{
    public static class RandomPickableModuleSpawner
    {
        private static PickableModule SpawnPickableModule(Vector2 position)
        {
            var pickableModule = PoolManager.Get<PickableModule>(nameof(PickableModule));
            pickableModule.transform.position = position;

            return pickableModule;
        }

        /// <summary>
        /// Spawn group after destroying enemy.
        /// </summary>
        /// <param name="centerPos">last enemy position</param>
        /// <param name="count"></param>
        /// <param name="radius"></param>
        public static void SpawnGroupModules(Vector2 centerPos, int count = 0, int radius = 3)
        {
            if (count <= 0)
                count = Random.Range(1, 3);
            
            for (int i = 0; i < count; i++)
            {
                var m = SpawnPickableModule(centerPos);
                m.AnimateDisplay(GetTargetPosition(centerPos, radius));
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