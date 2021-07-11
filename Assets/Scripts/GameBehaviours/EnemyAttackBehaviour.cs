using UnityEngine;
using System.Collections;
using GT.Data.Game;
using GT.Game.Modules;
using GT.Game.SwarmControls;
using GT.Game.Swarms;
using System;

public class EnemyAttackBehaviour : MonoBehaviour
{
    private Swarm _swarm;

    public Action<Vector3> LookAt;
    public Action Shoot;
    public Action<float, float> Move;
    public Action<float> Rorate;

    public void Setup(Swarm player)
    {
        _swarm = player;
    }

    public void Update()
    {
        var distanceToPlayer = Vector2.Distance(_swarm.transform.position, transform.position);
        if (distanceToPlayer > 10)
            return;

        LookAt?.Invoke( _swarm.transform.position);

        if (distanceToPlayer < 4 && distanceToPlayer > 2)
        {
            var moveDirection = _swarm.transform.position - transform.position;
            moveDirection.Normalize();
            Move?.Invoke(moveDirection.x, moveDirection.y);
        }

        if (distanceToPlayer < 3) // visible for camera 
        {
            Shoot?.Invoke();
        }

    }


}
