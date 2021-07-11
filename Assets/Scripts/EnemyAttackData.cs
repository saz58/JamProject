using GT.Game.Enemy;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAttackData", menuName = "ScriptableObjects/EnemyAttackData", order = 1)]

public class EnemyAttackData : ScriptableObject
{
    public EnemyType Type;
    public float AttackDistance;
    [Range(0, 1)] public float AttackDistanceRandomRange;
    public float ActivationDistance;
    [Range(0, 1)] public float ActivationDistanceRange;

    public float TrackDistance = 7f;
    [Range(0, 1)] public float TrackDistanceRange;
}