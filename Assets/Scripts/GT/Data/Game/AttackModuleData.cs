using GT.Game;
using GT.Game.Modules;
using GT.Game.Modules.Stats;

namespace GT.Data.Game
{
    public class AttackModuleData : ModuleData
    {
        public float Damage { get; }

        public override ModuleType Type => ModuleType.Attack;

        public AttackModuleData(int id) : this(id, GameConsts.AttackModuleHealth, GameConsts.AttackDamage)
        {
        }

        public AttackModuleData(int id, float health, float damage) : base(id, health)
        {
            Damage = damage;
        }

        public override ModuleStats ToStats()
        {
            return new AttackModuleStats(CurrentHealth, MaxHealth, Damage);
        }
    }
}