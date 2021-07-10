using System;
using UnityEngine;

namespace GT.Game.SwarmControls
{
    public class BaseControl : MonoBehaviour
    {
        [Header("Movement settings")]
        [SerializeField] private float _linearSpeedCoef = 3f;
        [SerializeField] private float _angularSpeedCoef = 50f;
        [SerializeField] private MovementBehaviour _movementBehaviour;

        public event Action<Vector2> OnTargetPositionChanged;
        public event Action OnFire;

        public void IncreaseSpeed(float linearSpeed, float angularSpeed, float linearVelocityLimit, float angularVelocityLimit)
        {
            _movementBehaviour.IncreaseLimit(linearVelocityLimit, angularVelocityLimit);
            _linearSpeedCoef += linearSpeed;
            _angularSpeedCoef += angularSpeed;
        }

        protected virtual void MoveToDirection(float x, float y)
        {
            Vector2 linearVelocity = new Vector2(x, y) * Time.deltaTime * _linearSpeedCoef;
            _movementBehaviour.AddLinearVelocity(linearVelocity);
        }

        protected virtual void RotateShip(float angle)
        {
            float angularVelocity = angle * Time.deltaTime * _angularSpeedCoef;
            _movementBehaviour.AddAngularVelocity(angularVelocity);
        }

        protected void TriggerOnTargetPositionChanged(Vector2 position) => OnTargetPositionChanged?.Invoke(position);

        protected void TriggerOnFire() => OnFire?.Invoke();
    }
}