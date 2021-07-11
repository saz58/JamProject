using GT.Game.Modules;
using GT.Game.Modules.Stats;

namespace GT.Data.Game
{
    public class AttackModuleData : ModuleData
    {
        public int Damage { get; }

        public override ModuleType Type => ModuleType.Attack;

        public AttackModuleData(int id) : base(id)
        {
        }

        public override ModuleStats ToStats()
        {
            return new AttackModuleStats(CurrentHealth, MaxHealth, Damage);
        }
    }
}