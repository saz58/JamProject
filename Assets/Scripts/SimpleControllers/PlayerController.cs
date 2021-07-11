using GT.Game.Swarms;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private MovementBehaviour _swarmMovement;

    private Transform _myTransform;
    private Swarm _swarm;

    public Transform MyTransform => _myTransform;
    public Swarm Swarm => _swarm;

    public Vector3 MovementDirection() => _swarmMovement.Velocity;

    public void Setup(Swarm swarm)
    {
        _swarm = swarm;
        _myTransform = transform;
        _swarmMovement = GetComponent<MovementBehaviour>();
    }
}
