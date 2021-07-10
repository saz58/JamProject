namespace Modules
{
    public class SpeedModuleStats : ModuleStats
    {
        public float SpeedIncrease { get; }

        public SpeedModuleStats(int hp, float speedIncrease) : base(hp)
        {
            SpeedIncrease = speedIncrease;
        }
    }
}