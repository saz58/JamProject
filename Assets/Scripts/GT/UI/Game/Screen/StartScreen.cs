using System;
using CustomExtension;
using UnityEngine;
using UnityEngine.UI;

namespace GT.UI.Game.Screen
{
    public class StartScreen : BaseScreen
    {
        [SerializeField] private Button startBtn = default;
        [SerializeField] private Button skipBtn = default;
        [SerializeField] private Image splashImg = default;
        [SerializeField] private Image tutorial = default;

        private const string tutorialDisplayed = "tutorial";

        [EditorButton]
        private void Test()
        {
            Init(null);
        }

        private void Awake()
        {
            splashImg.SetAlpha(0);
            tutorial.SetAlpha(0);
            skipBtn.gameObject.SetActive(false);
            startBtn.gameObject.SetActive(false);
            // PlayerPrefs.DeleteAll();
        }

        public void Init(Action startBtnAction)
        {
            startBtnAction += () => Close(null);
            
            skipBtn.onClick.RemoveAllListeners();
            startBtn.onClick.RemoveAllListeners();
            
            StartCoroutine(splashImg.Fade(1, 1, () =>
            {
                startBtn.gameObject.SetActive(true);
                startBtn.transform.CycleScale(this, 2, Vector3.one * 1.1F,0.05F);
            }));
            
            if (PlayerPrefs.GetInt(tutorialDisplayed, 0) > 0)
            {
                startBtn.onClick.AddListener(() =>
                {
                    startBtnAction?.Invoke();
                    StartCoroutine(startBtn.transform.Scale(Vector3.zero, 0.1F));
                    StartCoroutine(splashImg.Fade(0, 0.4F));
                });
            }
            else
                startBtn.onClick.AddListener(DisplayTutorial);
            
            void DisplayTutorial()
            {
                StartCoroutine(startBtn.transform.Scale(Vector3.zero, 0.1F));
                skipBtn.gameObject.SetActive(true);
                StartCoroutine(splashImg.Fade(0, 0.4F));
                StartCoroutine(tutorial.Fade(1, 0.4F));
                skipBtn.onClick.AddListener(()=>
                {
                    StartCoroutine(tutorial.Fade(0, 0.4F));
                    startBtnAction?.Invoke();
                });
                PlayerPrefs.SetInt(tutorialDisplayed, 1);
            }
        }

        public override void Open(Action onOpen)
        {
        }

        public override void Close(Action onClose)
        {
            Destroy(gameObject);
        }
    }
}