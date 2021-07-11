using System;

namespace GT.Game
{

    public static class ScoreManager
    {
        public static int TotalScoresCount { get; private set; }
        public static event Action<int> OnScoreChanged;

        public static void AddScore(int scores)
        {
            TotalScoresCount += scores;
            OnScoreChanged?.Invoke(TotalScoresCount);
        }

        public static void Reset()
        {
            TotalScoresCount = 0;
            OnScoreChanged?.Invoke(TotalScoresCount);
        }
    }
}