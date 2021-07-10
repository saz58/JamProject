using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimBehaviour : MonoBehaviour
{
    [SerializeField] private float _angleLimitPerCall = 1f;
    [SerializeField] private Transform _aimedTransform;

    public void AimTo(Vector3 point)
    {
        var angleToLook = CalcLookAngle(_aimedTransform.position, point);
        var currentAngle = _aimedTransform.rotation.eulerAngles.z;
        var deltaAngle = Mathf.DeltaAngle(currentAngle, angleToLook);

        if (Mathf.Abs(deltaAngle) > _angleLimitPerCall)
        {
            angleToLook = currentAngle + Mathf.Sign(deltaAngle) * _angleLimitPerCall;
        }

        _aimedTransform.rotation = Quaternion.Euler(Vector3.forward * angleToLook);
    }

    private float CalcLookAngle(Vector3 from, Vector3 to)
    {
        Vector2 lookDirection = to - from;
        return Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
    }
}
