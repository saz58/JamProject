using System.Collections;
using GT.Game.Swarms;
using Pool;
using UnityEngine;

namespace GT.Game.Modules
{
    public static class RandomPickableModuleSpawner
    {
        private static Swarm _player;
        public static void Init(Swarm player)
        {
            _player = player;
            GameApplication.Instance.StartCoroutine(TimeRandomSpawn());
            _player.OnDestroied += swarm =>
            {
                GameApplication.Instance.StopCoroutine(TimeRandomSpawn());
            };
        }
        
        private static PickableModule SpawnPickableModule(Vector2 position)
        {
            var pickableModule = PoolManager.Get<PickableModule>(nameof(PickableModule));
            pickableModule.transform.position = position;

            return pickableModule;
        }

        /// <summary>
        /// Spawn group after destroying enemy.
        /// </summary>
        /// <param name="pos">last enemy position</param>
        /// <param name="count"></param>
        /// <param name="radius"></param>
        public static void SpawnGroupModulesByRadius(Vector2 pos, int count = 0, int radius = 3)
        {
            if (count <= 0)
                count = Random.Range(1, 3);
            
            for (int i = 0; i < count; i++)
            {
                var m = SpawnPickableModule(pos);
                m.AnimateDisplay(GetTargetPosition(pos, radius));
            }
        }

        private static IEnumerator TimeRandomSpawn()
        {
            while (true)
            {
                SpawnPickableModule(GetTargetPosition(_player.Position, Random.Range(5, 7)));
                yield return new WaitForSeconds(Random.Range(5F, 15F));
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