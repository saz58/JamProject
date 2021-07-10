using UnityEngine;
using UnityEngine.AddressableAssets;

public class BaseScene : MonoBehaviour
{
    public static BaseScene Current { get; private set; }
    protected static bool Inited { get; private set; }
    protected virtual void Awake()
    {
        Current = this;
        Time.timeScale = 1;
        if (!Inited)
            Initialize();
    }

    protected void Initialize()
    {
        Inited = true;
        Application.targetFrameRate = 60;
        Addressables.InitializeAsync();
    }
}
