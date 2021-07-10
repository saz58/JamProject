using GT.Game.Modules;
using GT.Game.Modules.Stats;

namespace GT.Data.Game
{
    public class SpeedModuleData : ModuleData
    {
        public float SpeedIncrease { get; }

        public override ModuleType Type => ModuleType.Attack;

        public SpeedModuleData(int id) : base(id)
        {
        }

        public override ModuleStats ToStats()
        {
            return new SpeedModuleStats(CurrentHealth, SpeedIncrease);
        }
    }
}