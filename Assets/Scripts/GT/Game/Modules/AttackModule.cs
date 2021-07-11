using GT.Game.Modules.Stats;
using GT.Game.Swarms;
using UnityEngine;

namespace GT.Game.Modules
{
    public class AttackModule : BaseModule
    {
        [SerializeField] private AimBehaviour _aimBehaviour;
        [SerializeField] private FireBehaviour _fireBehaviour;

        private Rigidbody2D _rigidbody;

        public new AttackModuleStats Stats => (AttackModuleStats)base.Stats;

        protected override void AddEffectToSwarm(Swarm swarm)
        {
            if (!swarm)
                return;

            _fireBehaviour.ChangeDamage(Stats.Damage);
            swarm.OnTargetPositionChanged += AimTo;
            swarm.OnFire += Fire;
            _rigidbody = swarm.GetComponent<Rigidbody2D>();
        }

        protected override void RemoveEffectFromSwarm(Swarm swarm)
        {
            if (!swarm)
                return;

            swarm.OnTargetPositionChanged -= AimTo;
            swarm.OnFire -= Fire;
        }

        private void AimTo(Vector2 vector2)
        {
            _aimBehaviour.AimTo(vector2);
        }

        private void Fire()
        {
            _fireBehaviour.Fire(_rigidbody.velocity);
        }
    }
}
