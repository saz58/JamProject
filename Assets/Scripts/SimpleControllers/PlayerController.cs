using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 1;

    private void Update()
    {
        var v = Input.GetAxis("Vertical");
        var h = Input.GetAxis("Horizontal");

        var d = Vector3.right * h + Vector3.up * v;
        d *= Speed;

        transform.Translate(d* Time.deltaTime);
    }
}
