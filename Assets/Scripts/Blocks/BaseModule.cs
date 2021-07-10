using UnityEngine;

public class BaseModule : MonoBehaviour
{
    public static readonly Vector2[] ConnectDirrections =
    {
        new Vector2(0.86f, 0.5f),
        new Vector2(0.86f, -0.5f),
        new Vector2(-0.86f, 0.5f),
        new Vector2(-0.86f, -0.5f),
        new Vector2(0f, 1f),
        new Vector2(0f, -1f),
    };

    private Swarm _swarm;

    public Vector2 Position { get; set; }

    public void ConnectTo(Swarm swarm)
    {
        _swarm = swarm;

        AddEffectToSwarm(swarm);
    }

    private void OnDestroy()
    {
        RemoveEffectFromSwarm(_swarm);
        OnDestroyInner();
    }

    protected virtual void AddEffectToSwarm(Swarm swarm) { }

    protected virtual void RemoveEffectFromSwarm(Swarm swarm) { }

    protected virtual void OnDestroyInner() { }
}
