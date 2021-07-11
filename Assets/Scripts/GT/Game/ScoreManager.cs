using System;

namespace GT.Game
{
    public static class GameConsts
    {
        public const int AttackDamage = 10;
        public const int CoreModuleHealth = 100;
        public const int ShieldModuleHealth = 150;
        public const int SpeedModuleHealth = 50;
        public const int AttackModuleHealth = 50;
        public const int ScoreForSwarm = 100;
        public const int ScoreForModule = 25;
    }

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