using UnityEngine;

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
