using GT.Game.Modules;
using GT.Game.Modules.Stats;

namespace GT.Data.Game
{
    public class CoreModuleData : ModuleData
    {
        public override ModuleType Type => ModuleType.Attack;

        public CoreModuleData(int id) : base(id)
        {
        }

        public override ModuleStats ToStats()
        {
            return new ModuleStats(CurrentHealth);
        }
    }
}