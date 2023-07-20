using UnityEngine;

[CreateAssetMenu(menuName = "States/Enemy/SOEnemyCombatState", fileName = "New Enemy Combat State SO")]
public class SOEnemyCombatState : ScriptableObject
{
    [SerializeField]
    private float _attackRadius;
    [SerializeField]
    private float _timeBetweenAttacks;

    public float AttackRadiusSquared { get { return _attackRadius * _attackRadius; } }
    public float TimeBetweenAttacks { get { return _timeBetweenAttacks; } }
}