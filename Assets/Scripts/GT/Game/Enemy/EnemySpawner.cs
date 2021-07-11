using System.Collections;
using System.Collections.Generic;
using GT.Game.Swarms;
using UnityEngine;

namespace GT.Game.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private float _distanceFromPlayer;
        [SerializeField] private int _maxAmmount = 200;
        [SerializeField] private float _minSpawnInterval = 2f;
        [SerializeField] private float _startDelay = 5f;

        public static List<Swarm> _allEnemy = new List<Swarm>(200);

        private PlayerController _playerController;
        private CameraController _cameraController;

        public void Setup(PlayerController playerController, CameraController cameraController)
        {
            _playerController = playerController;
            _cameraController = cameraController;
            InvokeRepeating(nameof(Spawn), _startDelay, _minSpawnInterval);
        }

        private void Spawn()
        {
            if(_allEnemy.Count > _maxAmmount)
                return;

            var spawnPosition = GetPosition();
            var enemy = RandomSwarmGenerator.SpawnEnemy(spawnPosition, GetConfiguration(), GetModulCount());
            CorrectPositionOnScreen(enemy);
            _allEnemy.Add(enemy);
        }

        private Vector2 GetPosition()
        {
            Vector3 playerPosition = _playerController.MyTransform.position;
            var direction = _playerController.MovementDirection();

            var random = Random.insideUnitCircle.normalized * 5;
            var pos3d = (Vector2)playerPosition + (Vector2)direction * 5 + random;
            return pos3d;
        }

        public void CorrectPositionOnScreen(Swarm enemy)
        {
            var bound = enemy.GetBound();
            if(_cameraController.IsInCameraView(bound))
            {
                var direction = enemy.transform.position - _cameraController.Camera.transform.position;
                direction.Normalize();
                enemy.transform.position += direction * Mathf.Max(bound.size.x, bound.size.y);
            }
        }

        private EnemyConfiguration GetConfiguration()
        {
            var type = EnemyType.Walker;
            return EnemyConfiguration.GetForType(type);
        }

        private int GetModulCount()
        {
            return Random.Range(2, 6);
        }

    }
}
