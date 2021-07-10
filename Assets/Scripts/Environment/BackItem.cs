using UnityEngine;
using Pool;

public class BackItem : MonoBehaviour, IPoolObject
{
    [SerializeField] private float _height = 5;
    [SerializeField] private float _width = 5;

    public Vector2 Rect => new Vector2(_width, _height);

    private Transform _cacheTransform;
    public Transform MyTransform => _cacheTransform;

    public Vector2 BottomLeftCorner => MyTransform.position
        - Vector3.right * _width * 0.5f - Vector3.up * _height * 0.5f;

    public Vector2 TopRightCorner => MyTransform.position
        + Vector3.right * _width * 0.5f + Vector3.up * _height * 0.5f;

    private void Awake()
    {
        _cacheTransform = transform;
        
    }

    public void OnGetWithPool()
    {
    }

    public void OnReturnToPool()
    {
    }

}
