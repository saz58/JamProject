using GT.Game.Swarms;
using UnityEngine;

namespace GT.Game.Enemy
{
    public class EnemySpawnTest : MonoBehaviour
    {
        [SerializeField] private EnemyConfiguration _configuration;
        [SerializeField] private Swarm _userSwarm;
        [SerializeField] private int _totalCount;
        [SerializeField] private EnemySpawn _spawner;

        [EditorButton]
        public void Test()
        {
            _spawner.Spawn(_userSwarm.Position, _configuration, _totalCount);
        }
    }
}
