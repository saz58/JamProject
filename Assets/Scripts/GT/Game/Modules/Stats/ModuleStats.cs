using System;
using UnityEngine;

namespace GT.Game.Modules.Stats
{
    public class ModuleStats
    {
        public float Hp { get; private set; }
        public float MaxHP { get; private set; }

        public event Action<float> OnHealthChanged;

        public ModuleStats(float hp, float maxHP)
            => (Hp, MaxHP) = (hp, maxHP);

        public void ReceiveDamage(float damage)
        {
            Hp = Mathf.Max(Hp - damage, 0);
            OnHealthChanged?.Invoke(Hp);
        }
    }
}