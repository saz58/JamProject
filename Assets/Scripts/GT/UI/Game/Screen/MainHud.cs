using System;
using System.Collections.Generic;
using GT.Data.Game;
using GT.Game;
using GT.UI.Game.Component;
using UnityEngine;

namespace GT.UI.Game.Screen
{
    public class MainHud : BaseScreen
    {
        [SerializeField] private ModulesList modulesList = default;
        [SerializeField] private TMPro.TextMeshProUGUI scoreField = default;

        private void Start()
        {
            OnScoresUpdated(ScoreManager.TotalScoresCount);
            ScoreManager.OnScoreChanged += OnScoresUpdated;
        }

        public void Init(List<ModuleData> data)
        {
            
        }

        public override void Open(Action onOpen)
        {
            
        }

        public override void Close(Action onClose)
        {
            modulesList.Clear();
            ScoreManager.OnScoreChanged -= OnScoresUpdated;
            Destroy(gameObject);
        }

        private void OnScoresUpdated(int newScore)
        {
            scoreField.text = newScore.ToString();
        }
    }
}