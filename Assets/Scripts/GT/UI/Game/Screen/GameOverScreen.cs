using System;
using CustomExtension;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GT.UI.Game.Screen
{
    public class GameOverScreen : BaseScreen
    {
        [SerializeField] private Image splashImg = default;
        [SerializeField] private Button btn = default;
        [SerializeField] private TextMeshProUGUI scoreLabel = default;

        private void Awake()
        {
            splashImg.SetAlpha(0);
        }

        public void Init(int scores)
        {
            scoreLabel.text = scores.ToString();
        }

        public override void Open(Action onOpen)
        {
            StartCoroutine(splashImg.Fade(1, 0.2F));
            onOpen += () => Close(null);
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => onOpen?.Invoke());
        }

        public override void Close(Action onClose)
        {
            Destroy(gameObject);
        }
    }
}