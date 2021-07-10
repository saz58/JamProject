using UnityEngine;

namespace Modules
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

        private Swarm _swarm;
        public ModuleStats Stats { get; private set; }

        private Vector2 _position;
        public Vector2 Position
        {
            get => _position;
            set => transform.localPosition = _position = value;
        }

        public void Initialize(ModuleStats stats)
        {
            Stats = stats;
        }

        public void ConnectTo(Swarm swarm)
        {
            _swarm = swarm;

            AddEffectToSwarm(swarm);
        }

        public virtual void Destroy()
        {
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            RemoveEffectFromSwarm(_swarm);
            OnDestroyInner();
        }

        protected virtual void AddEffectToSwarm(Swarm swarm) { }

        protected virtual void RemoveEffectFromSwarm(Swarm swarm) { }

        protected virtual void OnDestroyInner() { }
    }
}