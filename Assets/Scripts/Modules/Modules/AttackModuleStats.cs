namespace Modules
{
    public class AttackModuleStats : ModuleStats
    {
        public float Damage { get; }

        public AttackModuleStats(int hp, float damage) : base(hp)
        {
            Damage = damage;
        }
    }
}