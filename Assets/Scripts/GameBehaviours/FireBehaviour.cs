using GT.Audio;
using Pool;
using UnityEngine;

public class FireBehaviour : MonoBehaviour
{
    [SerializeField] private Transform _shellSpawnPoint;
    [SerializeField] private float _dispersionAngle;
    [SerializeField] private float _coolDown = 0.5f;
    [SerializeField] private AudioClip _shotClip;

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

    public void ChangeDamage(float damage)
    {
        _damage = damage;
    }

    public void Fire(Vector2 shipVelocity)
    {
        if (Time.time - _fireTime < _coolDown)
        {
            return;
        }

        var c = GetComponent<Collider2D>();
        var r = c.attachedRigidbody;

        _fireTime = Time.time;
        
        var shell = PoolManager.Get<Shell>(_shellPoolKey.ToString());
        shell.transform.position = _shellSpawnPoint.position;
        shell.transform.rotation = _shellSpawnPoint.rotation;

        var directionVector = (_shellSpawnPoint.rotation * Vector3.up).normalized;
        var additionalSpeed = Vector3.Dot(directionVector, r.velocity);

        if (additionalSpeed < 0)
        {
            additionalSpeed = 0;
        }

        shell.transform.Rotate(Vector3.forward, Random.Range(_inverceDispersionAngle, _dispersionAngle));

        float additioalSpeed = 0f;
        var angleeToShipVelocity = Vector3.Angle(shipVelocity.normalized, shell.transform.up);

        if (angleeToShipVelocity < 90)
        {
            additioalSpeed = Mathf.Cos(angleeToShipVelocity * Mathf.Deg2Rad) * shipVelocity.magnitude;
           
        }

        shell.Shoot(_speed + additioalSpeed, _lifeTime, _damage, _shellRaycasLayerMask.value);
        GameApplication.Instance.gameAudio.PlaySfx(SoundFx.BlasterShoot);
    }
}
