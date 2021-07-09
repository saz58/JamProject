using UnityEngine;
using UnityEngine.UI;

namespace GT.CustomComponent.ProgressBar
{
    public class UIProgressBar : MonoBehaviour
    {
        [SerializeField] Slider slideBar = default;
        [SerializeField] private Image backgroundImg = default;
        [SerializeField] private bool inverted = default;
        public bool iSDone = false;

        private void Awake()
        {
            slideBar.value = inverted ? 1 : 0;
        }

        public void UpdateProgress(float progress)
        {
            slideBar.value = progress;
            iSDone = slideBar.value >= 1;
        }

        public void SetBackgroundSpriteColor(Color color)
        {
            backgroundImg.color = color;
        }
        public void SetBackgroundSprite(Sprite sprite)
        {
            backgroundImg.sprite = sprite;
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }

}