using System;
using System.Collections;
using GT.Game.Modules.Stats;
using GT.Game.Swarms;
using Pool;
using UnityEngine;

namespace GT.Game.Modules
{
    public class BaseModule : MonoBehaviour
    {
        public static readonly Vector2[] ConnectDirrections =
        {
            new Vector2(0.86f, 0.5f),
            new Vector2(0.86f, -0.5f),
            new Vector2(-0.86f, 0.5f),
            new Vector2(-0.86f, -0.5f),
            new Vector2(0f, 1f),
            new Vector2(0f, -1f),
        };

        [SerializeField] private GameObject _destroyVFX;
        [SerializeField] private ModuleDamageAnimamtion _damageAnimation;

        private Action<BaseModule> _onDestroy;

        private Swarm _swarm;
        public ModuleStats Stats { get; private set; }

        private Vector2 _position;
        public Vector2 Position
        {
            get => _position;
            set => transform.localPosition = _position = value;
        }

        public void Initialize(ModuleStats stats, Action<BaseModule> onDestroy)
        {
            Stats = stats;
            _onDestroy = onDestroy;
            Stats.OnHealthChanged += OnHealthChanged;
        }

        public void ConnectTo(Swarm swarm)
        {
            _swarm = swarm;

            AddEffectToSwarm(swarm);
        }

        public virtual void Destroy()
        {
            _onDestroy?.Invoke(this);
            RemoveEffectFromSwarm(_swarm);
            OnDestroyInner();
        }

        public void ReceiveDamage(float damage)
        {
            Stats.ReceiveDamage(damage);
        }

        protected virtual void AddEffectToSwarm(Swarm swarm) { }

        protected virtual void RemoveEffectFromSwarm(Swarm swarm) { }

        protected virtual void OnDestroyInner() { }

        private void OnHealthChanged(float hp)
        {
            if (hp <= 0)
            {
                _swarm.RemoveModule(this);
                Destroy();
            }

            _damageAnimation.PlayDamage();
        }
    }
}