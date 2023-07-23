using UnityEngine;

[CreateAssetMenu(menuName = "Player Characters/SOPCSharedData", fileName = "New PC Shared Data SO")]
public class SOPCSharedData : ScriptableObject
{
    [SerializeField]
    private LayerMask _pCLayerMask;
    [SerializeField]
    private LayerMask _enemyLayerMask;
    [SerializeField]
    private LayerMask _lootContainerLayerMask;
    [SerializeField]
    private LayerMask _exitLayerMask;
    [SerializeField]
    private LayerMask _groundLayerMask;

    public LayerMask PCLayerMask { get { return _pCLayerMask; } }
    public LayerMask EnemyLayerMask { get { return _enemyLayerMask; } }
    public LayerMask LootContainerLayerMask { get { return _lootContainerLayerMask; } }
    public LayerMask ExitLayerMask { get { return _exitLayerMask; } }
    public LayerMask GroundLayerMask { get { return _groundLayerMask; } }
}