namespace GT.Game.Modules.Stats
{
    public class SpeedModuleStats : ModuleStats
    {
        public float SpeedIncrease { get; }

        public SpeedModuleStats(float hp, float speedIncrease) : base(hp)
        {
            SpeedIncrease = speedIncrease;
        }
    }
}