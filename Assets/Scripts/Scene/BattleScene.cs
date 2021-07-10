using System;
using Scene;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scene
{
    public class BattleScene : BaseScene
    {
        public static SceneId SceneId = SceneId.Battle;
        [SerializeField] private GameObject _cameraPrefab;

        private bool _init;

        private void Start()
        {
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
        }

        
        public void Update()
        {
            if (!_init || !Application.isFocused)
                return;
        }

    }
}