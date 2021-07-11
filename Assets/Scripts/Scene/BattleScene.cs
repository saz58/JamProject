using GT.Data.Game;
using GT.Game.Enemy;
using GT.Game.Modules;
using GT.Game.Swarms;
using UnityEngine;

namespace Scene
{
    public class BattleScene : BaseScene
    {
        public static SceneId SceneId = SceneId.Battle;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private BackgroundController _backgroundController;
        [SerializeField] private EnemySpawner _enemySpawner;

        public static PlayerController Player;

        private bool _init;

        private void Start()
        {
            _cameraController.RegisterOverlayCamera();
            Debug.Log("[BattleScene] Start");

            if (!CacheLoader.CacheReady) {
                AddressableHandler.Instance.OnAllLoaded += SetupScene;
                AddressableHandler.Instance.GetAsync(CacheLoader.AssetsData.ItemKeys);
            }
            else
            {
                SetupScene();
            }
        }

        private void SetupScene()
        {
            Debug.Log("[BattleScene] SetupScene");
            AddressableHandler.Instance.OnAllLoaded -= SetupScene;
            _init = true;
            CacheLoader.Setup();

            var swarm = SwarmFactory.CreatePlayer(transform.position);
            Player = swarm.gameObject.AddComponent<PlayerController>();
            Player.Setup(swarm);

            _cameraController.Setup(Player.transform);
            _enemySpawner.Setup(Player);
            _backgroundController.Setup(_cameraController);
        }

        private void OnDestroy()
        {
            Player = null;
        }

        // TODO: do not remove this
        //public void Update()
        //{
        //    if (!_init || !Application.isFocused)
        //        return;
        //}

    }
}