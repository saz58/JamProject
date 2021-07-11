using Pool;
using UnityEngine;

public class CacheLoader
{
    private static GameAssetsData assetsData;
    public static GameAssetsData AssetsData
    {
        get
        {
            if(assetsData == null)
                assetsData = Resources.Load<GameAssetsData>(nameof(GameAssetsData));
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

    private static MovementData _movementSettings;
    public static MovementData MovementSettings
    {
        get
        {
            if (_movementSettings == null)
                _movementSettings = Resources.Load<MovementData>(nameof(MovementData));
            return _movementSettings;
        }
    }
}