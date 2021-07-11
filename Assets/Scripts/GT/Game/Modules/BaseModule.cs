using System;
using GT.Audio;
using GT.Game.Modules.Animations;
using GT.Game.Modules.Stats;
using GT.Game.Swarms;
using UnityEngine;

namespace GT.Game.Modules
{
    public class BaseModule : MonoBehaviour
    {
        private PolygonCollider2D _collider;
        public PolygonCollider2D Collider
        {
            get
            {
                if(_collider == null)
                    _collider = GetComponentInChildren<PolygonCollider2D>(true);
                return _collider;
            }
        }

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
        [SerializeField] private ModuleHPDisplay _hpDisplay;

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
            _hpDisplay.ShowHp(stats.Hp, stats.MaxHP);
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
            GameApplication.Instance.gameAudio.PlaySfxOnce(SoundFx.ModuleDestroy);
        }

        public void ReceiveDamage(float damage)
        {
            Stats.ReceiveDamage(damage);
            GameApplication.Instance.gameAudio.PlaySfx(Audio.SoundFx.BlasterHit);
        }

        protected virtual void AddEffectToSwarm(Swarm swarm) { }

        protected virtual void RemoveEffectFromSwarm(Swarm swarm) { }

        protected virtual void OnDestroyInner() { }

        private void OnHealthChanged(float hp)
        {
            _hpDisplay.ShowHp(hp, Stats.MaxHP);
            if (hp <= 0)
            {
                _swarm.RemoveModule(this);
                Destroy();
            }

            _damageAnimation.PlayDamage();
        }
    }
}