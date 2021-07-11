using System.Collections;
using System.Collections.Generic;
using GT.Game.SwarmControls;
using UnityEngine;

public class EnemyControl : BaseControl
{
    private EnemyAttackBehaviour _attackBehaviour;
    public void SetEnemyAttackBehaviour(EnemyAttackBehaviour behaviour)
    {
        _attackBehaviour = behaviour;
        AddListeners();
    }

    protected override void AddListeners()
    {
        if (!_attackBehaviour)
            return;

        _attackBehaviour.LookAt += AimTo;
        _attackBehaviour.Shoot += Shoot;
        _attackBehaviour.Move += MoveToDirection;
    }

    protected override void RemoveListeners()
    {
        if (!_attackBehaviour)
            return;

        _attackBehaviour.LookAt -= AimTo;
        _attackBehaviour.Shoot -= Shoot;
        _attackBehaviour.Move -= MoveToDirection;
    }

    protected override void AimTo(Vector3 point)
    {
        var worldPoint = _coordsTransformCamera.ScreenToWorldPoint(point);
        TriggerOnTargetPositionChanged(worldPoint);
    }

    protected override void Shoot()
    {
        TriggerOnFire();
    }


}