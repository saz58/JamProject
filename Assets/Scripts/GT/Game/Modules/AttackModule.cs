using GT.Game.Modules.Stats;
using GT.Game.Swarms;
using UnityEngine;

namespace GT.Game.Modules
{
    public class AttackModule : BaseModule
    {
        public new AttackModuleStats Stats => (AttackModuleStats)base.Stats;

        protected override void AddEffectToSwarm(Swarm swarm)
        {
            swarm.OnMouseMove += AimTo;
            swarm.OnMouseClick += Fire;
        }

        protected override void RemoveEffectFromSwarm(Swarm swarm)
        {
            swarm.OnMouseMove -= AimTo;
            swarm.OnMouseClick -= Fire;
        }

        private void AimTo(Vector2 vector2)
        { }

        private void Fire()
        { }
    }
}
