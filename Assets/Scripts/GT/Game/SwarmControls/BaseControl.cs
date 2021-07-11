using System;
using UnityEngine;

namespace GT.Game.SwarmControls
{
    public class BaseControl : MonoBehaviour
    {
        public event Action<Vector2> OnTargetPositionChanged;
        public event Action OnFire;

        private MovementBehaviour _movementBehaviour;

        public void IncreaseSpeed(float linearSpeed, float angularSpeed, float linearVelocityLimit, float angularVelocityLimit)
        {
            _movementBehaviour.IncreaseLimit(linearSpeed, angularSpeed, linearVelocityLimit, angularVelocityLimit);
        }

        public void Setup(MovementBehaviour movement)
        {
            _movementBehaviour = movement;
        }

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
    }
}