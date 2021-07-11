namespace GT.Game.Modules.Stats
{
    public class AttackModuleStats : ModuleStats
    {
        public float Damage { get; }

        public AttackModuleStats(float hp, float maxHP, float damage) : base(hp, maxHP)
        {
            Damage = damage;
        }
    }
}