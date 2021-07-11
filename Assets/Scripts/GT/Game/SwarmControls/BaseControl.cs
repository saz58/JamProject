using System;
using UnityEngine;

namespace GT.Game.SwarmControls
{
    public abstract class BaseControl : MonoBehaviour
    {
        public event Action<Vector2> OnTargetPositionChanged;
        public event Action OnFire;

        private MovementBehaviour _movementBehaviour;
        [SerializeField] protected Camera _coordsTransformCamera;

        public void IncreaseSpeed(float linearSpeed, float angularSpeed, float linearVelocityLimit, float angularVelocityLimit)
        {
            _movementBehaviour.IncreaseLimit(linearSpeed, angularSpeed, linearVelocityLimit, angularVelocityLimit);
        }

        public void Setup(MovementBehaviour movement)
        {
            _movementBehaviour = movement;
            if (_coordsTransformCamera == null)
                _coordsTransformCamera = Camera.main;
            AddListeners();
        }

        protected abstract void AddListeners();
        protected abstract void RemoveListeners();
        protected abstract void AimTo(Vector3 point);
        protected abstract void Shoot();


        protected virtual void MoveToDirection(float x, float y)
        {
            Vector2 linearVelocity = new Vector2(x, y) * Time.deltaTime;
            _movementBehaviour.AddLinearVelocity(linearVelocity);
        }

        protected virtual void RotateShip(float angle)
        {
            float angularVelocity = angle * Time.deltaTime;
            _movementBehaviour.AddAngularVelocity(angularVelocity);
        }

        protected void TriggerOnTargetPositionChanged(Vector2 position) => OnTargetPositionChanged?.Invoke(position);

        protected void TriggerOnFire() => OnFire?.Invoke();

        private void OnDestroy()
        {
            RemoveListeners();
        }
    }
}