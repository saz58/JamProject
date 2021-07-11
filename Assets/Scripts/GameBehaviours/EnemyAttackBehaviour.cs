using UnityEngine;
using System.Collections;
using GT.Data.Game;
using GT.Game.Modules;
using GT.Game.SwarmControls;
using GT.Game.Swarms;
using System;

public class EnemyAttackBehaviour : MonoBehaviour
{
    private Swarm _player;
    private Swarm _owner;
    private EnemyAttackData _settings;

    public Action<Vector3> LookAt;
    public Action Shoot;
    public Action<float, float> Move;
    public Action<float> Rorate;

    public void Setup(Swarm owner, Swarm player, EnemyAttackData settings)
    {
        _owner = owner;
        _player = player;
        _settings = settings;
    }

    public void Update()
    {
        var distanceToPlayer = Vector2.Distance(_player.Center(), transform.position);
        var playerMaxExtend = Mathf.Max(_player.GetBound().size.x, _player.GetBound().size.x);
        var enemyMaxExtend = Mathf.Max(_owner.GetBound().size.x, _owner.GetBound().size.x);

        var track = _settings.TrackDistance * UnityEngine.Random.Range(0, 1 + _settings.TrackDistance);
        if (distanceToPlayer - playerMaxExtend - enemyMaxExtend > track)
            return;

        LookAt?.Invoke(_player.Center());

        var activation = _settings.ActivationDistance * UnityEngine.Random.Range(0, 1 + _settings.ActivationDistance);
        if (distanceToPlayer - playerMaxExtend - enemyMaxExtend < activation && distanceToPlayer - playerMaxExtend - enemyMaxExtend > 2)
        {
            var moveDirection = _player.Center() - transform.position;
            moveDirection.Normalize();
            Move?.Invoke(moveDirection.x, moveDirection.y);
        }

        var attack = _settings.AttackDistance * UnityEngine.Random.Range(0, 1 + _settings.AttackDistanceRandomRange);

        if (distanceToPlayer - playerMaxExtend - enemyMaxExtend < attack) 
        {
            Shoot?.Invoke();
        }

    }


}
