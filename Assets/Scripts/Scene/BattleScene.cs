using UnityEngine;

namespace Scene
{
    public class BattleScene : BaseScene
    {
        public static SceneId SceneId = SceneId.Battle;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private BackgroundController _backgroundController;

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

            _cameraController.Setup(this);
            _backgroundController.Setup(_cameraController);
        }

        
        public void Update()
        {
            if (!_init || !Application.isFocused)
                return;
        }

    }
}