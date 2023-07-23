using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Characters/SOPCData", fileName = "New PC Data SO")]
public class SOPCData : ScriptableObject
{
    // Or do Transform for PC instances? 
    public static event Action<GameObject> OnSelectPC;

    [SerializeField, Header("These variables DON'T change during runtime")]
    private Sprite _icon;
    [SerializeField]
    private Sprite _characterImage;
    [SerializeField]
    private GameObject _pcPrefab;
    [SerializeField]
    private SOPCMovementState _pcMovementStateSO;
    [SerializeField]
    private SOPCSharedData _pcSharedDataSO;

    [Header("These variables DO change during runtime")]
    [SerializeField]
    private Equipment _equipment = new();
    [SerializeField]
    private Stats _stats;
    private GameObject _pcInstance;
    private int _injury;
    private int _pain;

    public Sprite Icon { get { return _icon; } }
    public Sprite CharacterImage { get { return _characterImage; } }
    public GameObject PCPrefab { get { return _pcPrefab; } }
    public SOPCMovementState PCMovementStateSO { get { return _pcMovementStateSO;} }
    public SOPCSharedData PCSharedDataSO { get { return _pcSharedDataSO; } }

    public GameObject PCInstance { get { return _pcInstance; } set { _pcInstance = value; } }
    public Equipment Equipment { get { return _equipment; } }
    public int Injury { get { return _injury; } set { _injury = value; } }
    public int Pain { get { return _pain; } set { _pain = value; } }
    public Stats Stats { get { return _stats; } }

    public int Attack()
    {
        int weaponAttack = Equipment.Weapon().WeaponAttack;
        int attackStat = Stats[StatType.Attack].ModdedValue;
        // TODO - Probably use different formula eventually. 
        return weaponAttack + attackStat;
    }

    // Called by clicking PC icon button. 
    public void Use()
    {
        // PCSelector hears this. Selects this PC for now. Center camera on double click later. 
        OnSelectPC?.Invoke(PCInstance);
    }
}