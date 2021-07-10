using Scene;
using UnityEngine;
using UnityEngine.UI;

public class InitCanvasUI : MonoBehaviour
{
    [SerializeField] private InitScene _initScene;
    [SerializeField] private Button _playButton;

    private void Start()
    {
        _playButton.onClick.RemoveAllListeners();
        _playButton.onClick.AddListener(Play);
    }

    private void Play()
    {
        SceneLoader.GoTo(SceneId.Battle);
    }
}
