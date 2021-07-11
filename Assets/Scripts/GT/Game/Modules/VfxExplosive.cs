using CustomExtension;
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
            GameApplication.Instance.DelayCoroutine(3, () => { PoolManager.Return(nameof(VfxExplosive), this); });
        }

        public void OnReturnToPool()
        {
            vfx.Stop();
        }
    }
}