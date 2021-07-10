using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float _rotationDelta = 50f;
    [SerializeField] private float _speedCoef = 10f;
    [Range(0,float.PositiveInfinity)]
    [SerializeField] private float _linearVelocityLimit = 50f;
    [Range(0, float.PositiveInfinity)]
    [SerializeField] private float _angularVelocityLimit = 360f;

    // Note: for tests only
    /*private void Update()
    {
        var linearVelocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * _speedCoef * Time.deltaTime; ;
        float angularVelocity = 0;

        if(Input.GetKey(KeyCode.Q))
        {
            angularVelocity = _rotationDelta * Time.deltaTime;
        }

        if(Input.GetKey(KeyCode.E))
        {
            angularVelocity = _rotationDelta * (-1) * Time.deltaTime;
        }

        AddVelocity(linearVelocity, angularVelocity);
    }*/

    private void AddVelocity(Vector2 linearVelocity, float angularVelocity)
    {
        Vector2 newVelocity = _rigidbody.velocity + linearVelocity;
        float newAngularVelocity = _rigidbody.angularVelocity + angularVelocity;

        if (newVelocity.magnitude > _linearVelocityLimit)
        {
            newVelocity = newVelocity.normalized * _linearVelocityLimit;
        }

        if(Mathf.Abs(newAngularVelocity) > _angularVelocityLimit)
        {
            newAngularVelocity = Mathf.Sign(newAngularVelocity) * _angularVelocityLimit;
        }

        _rigidbody.velocity = newVelocity;
        _rigidbody.angularVelocity = newAngularVelocity;
    }
}
