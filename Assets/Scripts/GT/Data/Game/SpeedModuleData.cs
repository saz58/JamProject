using GT.Game.Modules;
using GT.Game.Modules.Stats;

namespace GT.Data.Game
{
    public class SpeedModuleData : ModuleData
    {
        public float LinearSpeedIncrease { get; }
        public float AngularSpeedIncrease { get; }
        public float LinearVelocityIncrease { get; }
        public float AngularVelocityIncrease { get; }

        public override ModuleType Type => ModuleType.Speed;

        public SpeedModuleData(
            int id,
            float linearSpeedIncrease,
            float angularSpeedIncrease,
            float linearVelocityIncrease,
            float angularVelocityIncrease)
            : base(id)
        {
            LinearSpeedIncrease = linearSpeedIncrease;
            AngularSpeedIncrease = angularSpeedIncrease;
            LinearVelocityIncrease = linearVelocityIncrease;
            AngularVelocityIncrease = angularVelocityIncrease;
        }

        public override ModuleStats ToStats()
        {
            return new SpeedModuleStats(CurrentHealth, MaxHealth, LinearSpeedIncrease, AngularSpeedIncrease, LinearVelocityIncrease, AngularVelocityIncrease);
        }
    }
}