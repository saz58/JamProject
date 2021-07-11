using GT.Game.Modules;
using GT.Game.Modules.Stats;

namespace GT.Data.Game
{
    public abstract class ModuleData
    {
        public abstract ModuleType Type { get; }
        public int Id { get; set; }
        public float MaxHealth { get; set; }
        public float CurrentHealth { get; set; }
        public string Name { get; set; }

        public abstract ModuleStats ToStats();

        public ModuleData(int id, float health)
        {
            Id = id;
            MaxHealth = CurrentHealth = health;
        }

        public ModuleData(int id) : this (id, 20)
        {
        }
    }
}