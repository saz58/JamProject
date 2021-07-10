using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimBehaviour : MonoBehaviour
{
    [SerializeField] private float _angleLimitPerCall = 1f;
    [SerializeField] private Transform _aimedTransform;


    // Note: for tests only
    /*private void Update()
    {
        AimTo(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }*/

    private void AimTo(Vector3 point)
    {
        var angleToLook = CalcLookAngle(_aimedTransform.position, point);
        var currentAngle = _aimedTransform.rotation.eulerAngles.z;
        var deltaAngle = Mathf.DeltaAngle(currentAngle, angleToLook);

        Debug.Log($"DeltaAngle = {deltaAngle}");

        if (Mathf.Abs(deltaAngle) > _angleLimitPerCall)
        {
            Debug.Log($"currentAngle = {currentAngle}; AngleToLook {angleToLook}; angle to Rotate = {Mathf.Sign(angleToLook) * _angleLimitPerCall};");
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
