using Pool;
using UnityEngine;

namespace GT.Game.Modules
{
    public class VfxExplosive : MonoBehaviour, IPoolObject
    {
        [SerializeField] private ParticleSystem vfx;
        
        [EditorButton]
        public void OnGetWithPool()
        {
            vfx.Play();
        }

        public void OnReturnToPool()
        {
            vfx.Stop();
        }
    }
}