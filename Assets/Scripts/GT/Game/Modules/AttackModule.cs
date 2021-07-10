using GT.Game.Modules.Stats;
using GT.Game.Swarms;
using UnityEngine;

namespace GT.Game.Modules
{
    public class AttackModule : BaseModule
    {
        [SerializeField] private AimBehaviour _aimBehaviour;
        [SerializeField] private FireBehaviour _fireBehaviour;

        public new AttackModuleStats Stats => (AttackModuleStats)base.Stats;

        protected override void AddEffectToSwarm(Swarm swarm)
        {
            swarm.OnTargetPositionChanged += AimTo;
            swarm.OnFire += Fire;
        }

        protected override void RemoveEffectFromSwarm(Swarm swarm)
        {
            swarm.OnTargetPositionChanged -= AimTo;
            swarm.OnFire -= Fire;
        }

        private void AimTo(Vector2 vector2)
        {
            _aimBehaviour.AimTo(vector2);
        }

        private void Fire()
        {
            _fireBehaviour.Fire();
        }
    }
}
