using UnityEngine;

namespace GT.Game.Modules.Animations
{
    public class ModuleDamageAnimamtion : MonoBehaviour
    {
        [SerializeField] private Animation _animation;
        [SerializeField] private string _animationName;

        public void PlayDamage()
        {
            var anim = _animation.GetClip(_animationName);
            _animation.Play(anim.name);
        }
    }
}