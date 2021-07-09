using CustomExtension;
using GT.CustomComponent.ProgressBar;
using UnityEngine;
using UnityEngine.UI;

namespace GT.UI
{
    public class LoadingScreen : BaseScreen
    {
        [SerializeField] private TMPro.TextMeshProUGUI infoField = default;
        [SerializeField] private TMPro.TextMeshProUGUI titleField = default;
        [SerializeField] private UIProgressBar progressBar = default;
        [SerializeField] private Color32 regularColor = Color.white;
        [SerializeField] private Color32 errorColor = Color.red;
        [SerializeField] private Graphic[] screenGraphics = default;

        private void Awake()
        {
            infoField.color = regularColor;
            titleField.color = regularColor;
        }

        public void DisplayInfo(string info)
        {
            infoField.color = regularColor;
            infoField.text = $"[ {info} ]";

            if (progressBar.gameObject.activeSelf && progressBar.iSDone)
                progressBar.gameObject.SetActive(false);
        }

        public void DisplayError(string error)
        {
            infoField.color = errorColor;
            infoField.text = $"[ {error} ]";
        }

        public void SetLoadingProgress(float v)
        {
            progressBar.UpdateProgress(v);
        }

        public override void Open(System.Action onOpen)
        {
            gameObject.SetActive(true);
            progressBar.gameObject.SetActive(true);
            foreach (var graphic in screenGraphics)
                StartCoroutine(graphic.Fade(1, 0.24f));

            this.DelayCoroutine(0.32f, onOpen);
            
            titleField.text = $"[{Application.productName}: {AppSemanticVersion.Instance.semVer}]";

        }

        public override void Close(System.Action onClose)
        {
            if (gameObject.activeSelf == false) return;
            onClose += () => gameObject.SetActive(false);
            foreach (var graphic in screenGraphics)
                StartCoroutine(graphic.Fade(0, 0.24f));

            this.DelayCoroutine(0.3f, onClose);
        }
    }
}