using GT.Game.Connectors;
using GT.Game.Modules;
using GT.Game.Modules.Stats;

namespace GT.Data.Game
{
    public class ShieldModuleData : ModuleData
    {
        public override ModuleType Type => ModuleType.Attack;

        public ShieldModuleData(int id) : base(id)
        {
        }

        public override ModuleStats ToStats()
        {
            return new ModuleStats(CurrentHealth);
        }
    }
}