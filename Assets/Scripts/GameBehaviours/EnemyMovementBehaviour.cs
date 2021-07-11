using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementBehaviour : MovementBehaviour
{
    public override void AddAngularVelocity(float angularVelocity)
    {
        base.AddAngularVelocity(angularVelocity);
    }

    public override void AddLinearVelocity(Vector2 linearVelocity)
    {
        base.AddLinearVelocity(linearVelocity);
    }
}
