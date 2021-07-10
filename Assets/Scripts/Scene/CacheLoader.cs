using Pool;
using UnityEngine;

namespace Scene
{
    public class CacheLoader
    {
        private static GameAssetsData assetsData;
        public static GameAssetsData AssetsData
        {
            get
            {
                if(assetsData == null)
                    assetsData = Resources.Load<GameAssetsData>("GameAssetsData");
                return assetsData;
            }
        }

        public static bool CacheReady { get; set; }

        public static void Setup()
        {
            foreach (var assets in AssetsData.Assets)
            {
                var prefab = AddressableHandler.Instance.Get(assets.key);
                PoolManager.Instance.Register(new PoolDataStruct(assets.key, prefab, assets.count));
            }
        }
    }
}