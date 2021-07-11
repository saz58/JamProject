using Pool;
using UnityEngine;

namespace GT.Game.Modules
{
    public static class VfxSpawner
    {
        public static void AddVfx(Vector2 pos)
        {
            var vfx = PoolManager.Get<VfxExplosive>(nameof(VfxExplosive));
            vfx.transform.position = pos;
            vfx.OnGetWithPool();
        }
    }
}