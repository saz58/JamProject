using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private MovementData _movementSettings;

    public void IncreaseLimit(float linearSpeedCoef, float angularSpeedCoef, float linearVelocityLimit, float angunalVelicityLimit)
    {
        _movementSettings._linearVelocityLimit += linearVelocityLimit;
        _movementSettings._angularVelocityLimit += angunalVelicityLimit;

        _movementSettings._linearSpeedCoef += linearSpeedCoef;
        _movementSettings._angularSpeedCoef += angularSpeedCoef;

    }

    public Vector3 Velocity => _rigidbody.velocity;

    public void Setup(MovementData settings, Rigidbody2D rigidbody)
    {
        _movementSettings = settings;
        _rigidbody = rigidbody;
    }

    public virtual void AddLinearVelocity(Vector2 linearVelocity)
    {
        Vector2 newVelocity = _rigidbody.velocity + linearVelocity * _movementSettings._linearSpeedCoef;
        if (newVelocity.magnitude > _movementSettings._linearVelocityLimit)
        {
            newVelocity = newVelocity.normalized * _movementSettings._linearVelocityLimit;
        }

        _rigidbody.velocity = newVelocity;
    }

    public virtual void AddAngularVelocity(float angularVelocity)
    {
        float newAngularVelocity = _rigidbody.angularVelocity + angularVelocity * _movementSettings._angularSpeedCoef;
        
        if (Mathf.Abs(newAngularVelocity) > _movementSettings._angularVelocityLimit)
        {
            newAngularVelocity = Mathf.Sign(newAngularVelocity) * _movementSettings._angularVelocityLimit;
        }

        _rigidbody.angularVelocity = newAngularVelocity;
    }
}
