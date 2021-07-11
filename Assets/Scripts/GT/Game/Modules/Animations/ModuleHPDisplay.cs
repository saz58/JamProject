using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GT.Game.Modules.Animations
{
    public class ModuleHPDisplay : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer[] _spriteRenderers;
        [SerializeField] private Gradient _gradient;

        public void ShowHp(float current, float max)
        {
            var color = _gradient.Evaluate(current / max);
            foreach (var renderer in _spriteRenderers)
            {
                renderer.color = color;
            }
        }
    }
}