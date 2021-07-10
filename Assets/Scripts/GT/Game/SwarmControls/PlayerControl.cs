using UnityEngine;

namespace GT.Game.SwarmControls
{
    public class PlayerControl : BaseControl
    {
        [SerializeField] private Camera _coordsTransformCamera;

        private bool _isConsturtorMode = false;

        private void Awake()
        {
            if (_coordsTransformCamera == null)
            {
                _coordsTransformCamera = Camera.main;
            }

            AddListeners();
        }

        private void AddListeners()
        {
            InputManager.Instance.OnMovementAxisUpdate += MoveToDirection;
            InputManager.Instance.OnRotationAxisUpdate += RotateShip;
            InputManager.Instance.OnWorldSpacePointerPositionUpdate += AimTo;
            InputManager.Instance.OnConstructButtonDown += ToggleConstructMode;
            InputManager.Instance.OnFireButtonPressed += Shoot;

        }
        private void RemoveListeners()
        {
            InputManager.Instance.OnMovementAxisUpdate -= MoveToDirection;
            InputManager.Instance.OnRotationAxisUpdate -= RotateShip;
            InputManager.Instance.OnWorldSpacePointerPositionUpdate -= AimTo;
            InputManager.Instance.OnConstructButtonDown -= ToggleConstructMode;
            InputManager.Instance.OnFireButtonPressed -= Shoot;
        }

        private void AimTo(Vector3 point)
        {
            if (!_isConsturtorMode)
            {
                var worldPoint = _coordsTransformCamera.ScreenToWorldPoint(point);
                TriggerOnTargetPositionChanged(worldPoint);
            }
        }

        private void ToggleConstructMode()
        {
            _isConsturtorMode = !_isConsturtorMode;
        }

        private void Shoot()
        {
            if (!_isConsturtorMode)
            {
                TriggerOnFire();
            }
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }
    }
}