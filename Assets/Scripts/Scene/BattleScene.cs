using GT.Game;
using GT.Game.Enemy;
using GT.Game.Swarms;
using GT.UI;
using GT.UI.Game.Screen;
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

            UIScreenController.Instance.Create<StartScreen>(screen => { screen.Init(StartGame); });

            void StartGame()
            {
                if (!CacheLoader.CacheReady)
                {
                    AddressableHandler.Instance.OnAllLoaded += SetupScene;
                    AddressableHandler.Instance.GetAsync(CacheLoader.AssetsData.ItemKeys);
                }
                else
                {
                    SetupScene();
                }
            }
        }

        private void SetupScene()
        {
            Debug.Log("[BattleScene] SetupScene");

            ScoreManager.Reset();
            
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
    }
}