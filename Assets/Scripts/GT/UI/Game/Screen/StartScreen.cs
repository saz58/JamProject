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
        [SerializeField] private AudioClip _backgorundMusic;

        private const string TutorialDisplayed = "tutorial";

        private static bool _isBackgroundMusicStarted = false;

        private void Awake()
        {
            // splashImg.SetAlpha(0);
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
            
            startBtn.gameObject.SetActive(true);
            startBtn.transform.CycleScale(this, 2, Vector3.one * 1.1F,0.05F);
            
            if (PlayerPrefs.GetInt(TutorialDisplayed, 0) > 0)
            {
                startBtn.onClick.AddListener(() =>
                {
                    startBtnAction?.Invoke();
                    StartCoroutine(startBtn.transform.Scale(Vector3.zero, 0.1F));
                    StartCoroutine(splashImg.Fade(0, 0.4F));

                    if (!_isBackgroundMusicStarted)
                    {
                        GameApplication.Instance.gameAudio.PlayMusic(_backgorundMusic, 0.1f);
                    }
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
                PlayerPrefs.SetInt(TutorialDisplayed, 1);
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