using GT.Game.Modules.Stats;
using Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ShellPoolKey
{
    PlayerShellItem,
    EnemyShellItem
}

public class Shell : MonoBehaviour, IPoolObject
{
    [SerializeField] private ShellPoolKey _shellPoolKey;
    [SerializeField] private Transform _shellRaycastOrigin;
    [SerializeField] private UnityAction _onHitAction;
    
    private float _speed;
    private float _timeToAutodestroy;
    private float _damage;
    private int _layerMask;

    public void Shoot(float startSpeed, float lifeTime, float damage, int layerMask)
    {
        _speed = startSpeed;
        _timeToAutodestroy = lifeTime;
        _layerMask = layerMask;
        _damage = damage;
    }

    private void Update()
    {
        UpdateShellState(Time.deltaTime);
    }

    private void UpdateShellState(float deltaTime)
    {
        Debug.DrawRay(_shellRaycastOrigin.position, _shellRaycastOrigin.up, Color.red);
        var stepSize = _speed * deltaTime;
        var hitResult = Physics2D.Raycast(_shellRaycastOrigin.position, _shellRaycastOrigin.up, stepSize, _layerMask);
        
        if(hitResult.collider != null)
        {
            var stats = hitResult.collider.GetComponent<ModuleStats>();
            
            if(stats != null)
            {
                stats.ReceiveDamage(_damage);
            }

            OnHit();
        }
        else
        {
            transform.position += _shellRaycastOrigin.up * stepSize;
            _timeToAutodestroy -= deltaTime;
            
            if(_timeToAutodestroy <= 0)
            {
                Despawn();
            }
        }
    }

    private void OnHit()
    {
        _onHitAction?.Invoke();
        Despawn();
    }

    private void Despawn()
    {
        PoolManager.Return(_shellPoolKey.ToString(), this);
    }

    public void OnGetWithPool()
    {
        _timeToAutodestroy = 0;
    }

    public void OnReturnToPool()
    {
    }
}
