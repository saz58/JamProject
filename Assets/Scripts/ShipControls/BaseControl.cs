using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseControl : MonoBehaviour
{
    [Header("Movement settings")]
    [SerializeField] private float _linearSpeedCoef = 3f;
    [SerializeField] private float _angularSpeedCoef = 50f;
    [SerializeField] private MovementBehaviour _movementBehaviour;
    
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

    protected virtual void AimTo(Vector3 point)
    {

    }
    protected virtual void Shoot()
    {

    }

    protected virtual void ToggleConstructMode()
    {

    }
}
