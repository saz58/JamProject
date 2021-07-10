using System;
using UnityEngine;

namespace GT.Game.Modules.Stats
{
    public class ModuleStats
    {
        public float Hp { get; private set; }

        public event Action<float> OnHealthChanged;

        public ModuleStats(float hp)
            => Hp = hp;

        public void ReceiveDamage(float damage)
        {
            Hp = Mathf.Max(Hp - damage, 0);
            OnHealthChanged?.Invoke(Hp);
        }
    }
}