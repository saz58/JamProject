using UnityEngine;

namespace GT.Game.SwarmControls
{
    public class PlayerControl : BaseControl
    {
        private bool _isConsturtorMode = false;

        protected override void AddListeners()
        {
            InputManager.Instance.OnMovementAxisUpdate += MoveToDirection;
            InputManager.Instance.OnRotationAxisUpdate += RotateShip;
            InputManager.Instance.OnWorldSpacePointerPositionUpdate += AimTo;
            InputManager.Instance.OnConstructButtonDown += ToggleConstructMode;
            InputManager.Instance.OnFireButtonPressed += Shoot;

        }
        protected override void RemoveListeners()
        {
            InputManager.Instance.OnMovementAxisUpdate -= MoveToDirection;
            InputManager.Instance.OnRotationAxisUpdate -= RotateShip;
            InputManager.Instance.OnWorldSpacePointerPositionUpdate -= AimTo;
            InputManager.Instance.OnConstructButtonDown -= ToggleConstructMode;
            InputManager.Instance.OnFireButtonPressed -= Shoot;
        }

        protected override void AimTo(Vector3 point)
        {
            if (!_isConsturtorMode)
            {
                var worldPoint = _coordsTransformCamera.ScreenToWorldPoint(point);
                TriggerOnTargetPositionChanged(worldPoint);
            }
        }

        protected override void Shoot()
        {
            if (!_isConsturtorMode)
            {
                TriggerOnFire();
            }
        }

        private void ToggleConstructMode(bool toggle)
        {
            _isConsturtorMode = toggle;
        }
    }
}