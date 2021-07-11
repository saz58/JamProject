using CustomExtension;
using GT.Game.Swarms;
using UnityEngine;

namespace GT.Game.Enemy
{
    public class EnemySpawnTest : MonoBehaviour
    {
        [SerializeField] private EnemyConfiguration _configuration;
        [SerializeField] private Swarm _userSwarm;
        [SerializeField] private int _totalCount;

        [EditorButton]
        public void Test()
        {
            var type = EnumExts.RandomEnumValue<EnemyType>();
            RandomSwarmGenerator.SpawnEnemy(type, _userSwarm.Position, _configuration, _totalCount);
        }
    }
}
