using Pool;
using UnityEngine;

public class FireBehaviour : MonoBehaviour
{
    [SerializeField] private Transform _shellSpawnPoint;
    [SerializeField] private float _dispersionAngle;
    [SerializeField] private float _coolDown = 0.5f;

    [Header("Shell params")]
    [SerializeField] private float _speed;
    [SerializeField] private float _lifeTime;
    [SerializeField] private float _damage;

    [SerializeField] private LayerMask _shellRaycasLayerMask;
    [SerializeField] private ShellPoolKey _shellPoolKey;

    private float _inverceDispersionAngle;
    private float _fireTime;

    private void Awake()
    {
        _inverceDispersionAngle = _dispersionAngle * (-1f);
    }

    public void Fire()
    {
        if (Time.time - _fireTime < _coolDown)
        {
            return;
        }

        _fireTime = Time.time;
        var shell = PoolManager.Get<Shell>(_shellPoolKey.ToString());
        shell.transform.position = _shellSpawnPoint.position;
        shell.transform.rotation = _shellSpawnPoint.rotation;
        shell.transform.Rotate(Vector3.forward, Random.Range(_inverceDispersionAngle, _dispersionAngle));
        shell.Shoot(_speed, _lifeTime, _damage, _shellRaycasLayerMask.value);
    }
}
