using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private MovementBehaviour _swarmMovement;

    private Transform _myTransform;
    public Transform MyTransform => _myTransform;

    public Vector3 MovementDirection() => _swarmMovement.Velocity;

    private void Awake()
    {
        _myTransform = transform;
        _swarmMovement = GetComponent<MovementBehaviour>();
    }
}
