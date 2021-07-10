using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [Range(0,float.PositiveInfinity)]
    [SerializeField] private float _linearVelocityLimit = 50f;
    [Range(0, float.PositiveInfinity)]
    [SerializeField] private float _angularVelocityLimit = 360f;

    public void IncreaseLimit(float linearVelocityLimit, float angunalVelicityLimit)
    {
        _linearVelocityLimit += linearVelocityLimit;
        _angularVelocityLimit += angunalVelicityLimit;
    }

    public void AddLinearVelocity(Vector2 linearVelocity)
    {
        Vector2 newVelocity = _rigidbody.velocity + linearVelocity;
        

        if (newVelocity.magnitude > _linearVelocityLimit)
        {
            newVelocity = newVelocity.normalized * _linearVelocityLimit;
        }

        _rigidbody.velocity = newVelocity;
    }

    public void AddAngularVelocity(float angularVelocity)
    {
        float newAngularVelocity = _rigidbody.angularVelocity + angularVelocity;
        
        if (Mathf.Abs(newAngularVelocity) > _angularVelocityLimit)
        {
            newAngularVelocity = Mathf.Sign(newAngularVelocity) * _angularVelocityLimit;
        }

        _rigidbody.angularVelocity = newAngularVelocity;
    }
}
