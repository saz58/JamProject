using GT.Data.Game;
using GT.Game;
using GT.Game.Enemy;
using GT.Game.Modules;
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

        private static bool _startFightImmidiatelly;

        private void Start()
        {
            _cameraController.RegisterOverlayCamera();
            Debug.Log("[BattleScene] Start");

            if (_startFightImmidiatelly)
            {
                UIScreenController.Instance.HideLoadingScreen();
                StartGame();
            }
            else
            {
                UIScreenController.Instance.Create<StartScreen>(screen =>
                {
                    UIScreenController.Instance.HideLoadingScreen();
                    screen.Init(StartGame);
                });
            }


            void StartGame()
            {
                UIScreenController.Instance.Create<MainHud>(hud =>
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
                });
            }
        }

        private void SetupScene()
        {
            Debug.Log("[BattleScene] SetupScene");

            ScoreManager.Reset();
            DataHandler.Clear();

            AddressableHandler.Instance.OnAllLoaded -= SetupScene;
            _init = true;
            CacheLoader.Setup();

            var swarm = SwarmFactory.CreatePlayer(transform.position);
            Player = swarm.gameObject.AddComponent<PlayerController>();
            Player.Setup(swarm);
            swarm.OnDestroied += OnPlayerSwarmDestroied;

            RandomPickableModuleSpawner.SpawnGroupModules(new Vector2(swarm.transform.position.x + 4, swarm.transform.position.y), 5, 4);

            _cameraController.Setup(Player.transform);
            _enemySpawner.Setup(Player, _cameraController);
            _backgroundController.Setup(_cameraController);
        }

        private void OnPlayerSwarmDestroied(Swarm swarm)
        {
            UIScreenController.Instance.Create<GameOverScreen>(screen =>
            {
                screen.Init(ScoreManager.TotalScoresCount);
                screen.Open(() =>
                {
                    _startFightImmidiatelly = true;
                    UIScreenController.Instance.GetLoadingScreen(null);
                    UIScreenController.Instance.DestroyScreen<MainHud>();
                    SceneLoader.GoTo(SceneId);

                });
            });
        }

        private void OnDestroy()
        {
            Player = null;
        }
    }
}