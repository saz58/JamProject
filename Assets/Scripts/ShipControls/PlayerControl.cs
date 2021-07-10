using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : BaseControl
{
    [SerializeField] private Camera _coordsTransformCamera;

    [Header("Aiming settings")]
    [SerializeField] private AimBehaviour _aimBehaviour;

    private bool _isConsturtorMode = false;

    private void Awake()
    {
        if(_coordsTransformCamera == null)
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

    protected override void AimTo(Vector3 point)
    {
        if (!_isConsturtorMode)
        {
            var worldPoint = _coordsTransformCamera.ScreenToWorldPoint(point);
            Debug.Log($"_coordsTransformCamera {_coordsTransformCamera}; point {point}, worldPoint {worldPoint}");
            _aimBehaviour.AimTo(worldPoint);
        }
    }

    protected override void ToggleConstructMode()
    {
        _isConsturtorMode = !_isConsturtorMode;
    }

    protected override void Shoot()
    {
        if(!_isConsturtorMode)
        {

        }
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }
}
