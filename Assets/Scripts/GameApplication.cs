using GT;
using GT.Asset;
using GT.UI;
using UnityEngine;

public class GameApplication : MonoBehaviour
{
    public static GameApplication Instance;
    [SerializeField] private UIScreenController uiScreenController = default;
    [SerializeField] public GT.Audio.Audio gameAudio = default;
    [SerializeField] private SceneAddress _startFromScene = default;


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void BeforeSceneLoad()
    {
        Debug.Log($" Build version: {AppSemanticVersion.Instance.semVer} " +
                  $"StreamingAssets path: {Application.streamingAssetsPath} " +
                  $"Addressables runtime path: {UnityEngine.AddressableAssets.Addressables.RuntimePath}");
    }

    private void Awake()
    {
        Debug.Log($"boot application");

        Instance = this;
        DontDestroyOnLoad(gameObject);

        uiScreenController.GetLoadingScreen(loadingScreen => { RunGame(); });

        void RunGame()
        {
            AddressableHelper.LoadSceneByKey(_startFromScene.ToString(), () =>
            {
                Debug.Log("Woohoo game scene is loaded.");
                uiScreenController.HideLoadingScreen();
            }, null);
        }
    }

    private void OnDestroy()
    {
    }
}