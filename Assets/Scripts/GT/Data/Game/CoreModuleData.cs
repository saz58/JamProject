using GT.Game;
using GT.Game.Modules;
using GT.Game.Modules.Stats;

namespace GT.Data.Game
{
    public class CoreModuleData : ModuleData
    {
        public override ModuleType Type => ModuleType.Core;

        public CoreModuleData(int id) : base(id, GameConsts.CoreModuleHealth)
        {
        }

        public override ModuleStats ToStats()
        {
            return new ModuleStats(CurrentHealth, MaxHealth);
        }
    }
}