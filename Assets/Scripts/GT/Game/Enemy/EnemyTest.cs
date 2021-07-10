using UnityEngine;

namespace GT.Game.Enemy
{
    public class EnemyTest : MonoBehaviour
    {
        [SerializeField] EnemyConfiguration _configuration;
        [SerializeField] int _totalCount;
        [SerializeField] EnemySpawn _spawner;

        [EditorButton]
        public void Test()
        {
            _spawner.Spawn(Vector2.zero, _configuration, _totalCount);
        }
    }
}
