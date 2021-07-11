namespace GT.Game.Modules.Stats
{
    public class SpeedModuleStats : ModuleStats
    {
        public float LinearSpeedIncrease { get; }
        public float AngularSpeedIncrease { get; }
        public float LinearVelocityLimitIncrease { get; }
        public float AngularVelocityLimitIncrease { get; }

        public SpeedModuleStats(
            float hp,
            float maxHP,
            float linearSpeedIncrease,
            float angularSpeedIncrease,
            float linearVelocityIncrease,
            float angularVelocityIncrease)
            : base(hp, maxHP)
        {
            LinearSpeedIncrease = linearSpeedIncrease;
            AngularSpeedIncrease = angularSpeedIncrease;
            LinearVelocityLimitIncrease = linearVelocityIncrease;
            AngularVelocityLimitIncrease = angularVelocityIncrease;
        }
    }
}