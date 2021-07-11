using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovementData", menuName = "ScriptableObjects/MovementData", order = 1)]

public class MovementData : ScriptableObject
{
    public float _linearSpeedCoef = 3f;
    public float _angularSpeedCoef = 50f;
    [Range(0, float.PositiveInfinity)]
    public float _linearVelocityLimit = 20f;
    [Range(0, float.PositiveInfinity)]
    public float _angularVelocityLimit = 60f;

    public MovementData Clone()
    {
        var clone = CreateInstance<MovementData>();
        clone._linearSpeedCoef = _linearSpeedCoef;
        clone._angularSpeedCoef = _angularSpeedCoef;
        clone._linearVelocityLimit = _linearVelocityLimit;
        clone._angularVelocityLimit = _angularVelocityLimit;
        return clone;
    }
}
