using System.Collections;
using System.Collections.Generic;
using CustomExtension;
using GT.Game.Modules;
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

        public static List<Swarm> AllEnemy = new List<Swarm>(200);
        private static List<Swarm> EnemyToDestroy = new List<Swarm>(200);

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
            CheckTooFar();

            if (AllEnemy.Count > _maxAmmount)
                return;

            var spawnPosition = GetPosition();
            var type = EnumExts.RandomEnumValue<EnemyType>();
            var enemy = RandomSwarmGenerator.SpawnEnemy(type, spawnPosition, GetConfiguration(type), GetModulCount());
            enemy.OnDestroied += SpawnItemsOnDestroy;
            CorrectPositionOnScreen(enemy);
            AllEnemy.Add(enemy);
        }

        private void CheckTooFar()
        {
            EnemyToDestroy.Clear();
            foreach (var enemy in AllEnemy)
            {
                var dist = Vector3.Distance(_cameraController.Camera.transform.position, enemy.Center());
                var cameraArround = Mathf.Max(_cameraController.Bound.size.x, _cameraController.Bound.size.y);
                if(dist > cameraArround * 7)
                {
                    EnemyToDestroy.Add(enemy);
                }
            }

            foreach (var enemy in EnemyToDestroy)
            {
                EnemyDestroyed(enemy);
                enemy.DestroySwarm();
            }
        }

        public static void EnemyDestroyed(Swarm swarm)
        {
            AllEnemy.Remove(swarm);
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
            bound.center = enemy.Center();
            if(_cameraController.IsInCameraView(bound))
            {
                var direction = enemy.transform.position - _cameraController.Camera.transform.position;
                direction.z = 0;
                direction.Normalize();
                var camBound = _cameraController.Bound;
                enemy.transform.position += direction * (Mathf.Max(bound.size.x, bound.size.y) + Mathf.Max(camBound.size.x, camBound.size.y));
            }
        }

        private EnemyConfiguration GetConfiguration(EnemyType type)
        {
            return EnemyConfiguration.GetForType(type);
        }

        private static void SpawnItemsOnDestroy(Swarm swarm)
        {
            RandomPickableModuleSpawner.SpawnGroupModules(new Vector2(swarm.transform.position.x, swarm.transform.position.y), radius: 1);
            swarm.OnDestroied -= SpawnItemsOnDestroy;
        }

        private int GetModulCount()
        {
            return (int)(_playerController.Swarm.ModuleCount * Random.Range(0.3f, 1.3f)) + 1;
        }

    }
}
