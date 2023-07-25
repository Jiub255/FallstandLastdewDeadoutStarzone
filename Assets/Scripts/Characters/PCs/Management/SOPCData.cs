using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Characters/SOPCData", fileName = "New PC Data SO")]
public class SOPCData : ScriptableObject
{
    /// <summary>
    /// Called by clicking on PC icon in PC HUD. 
    /// </summary>
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
    private int _injury;
    private int _pain;
    [SerializeField]
    private Stats _stats;
    [SerializeField]
    private Equipment _equipment = new();
    private GameObject _pcInstance;
    private bool _healing = false;
    private PCState _activeState;
    // TODO - Keep selected bool here? Or use SelectedPC on current team SO?
    private bool _selected = false;

    public Sprite Icon { get { return _icon; } }
    public Sprite CharacterImage { get { return _characterImage; } }
    public GameObject PCPrefab { get { return _pcPrefab; } }
    public SOPCMovementState PCMovementStateSO { get { return _pcMovementStateSO;} }
    public SOPCSharedData PCSharedDataSO { get { return _pcSharedDataSO; } }

    public int Injury { get { return _injury; } set { _injury = value; } }
    public int Pain { get { return _pain; } set { _pain = value; } }
    public Stats Stats { get { return _stats; } }
    public Equipment Equipment { get { return _equipment; } }
    public GameObject PCInstance { get { return _pcInstance; } set { _pcInstance = value; } }
    public bool Healing { get { return _healing; } set { _healing = value; } }
    public PCState ActiveState { get { return _activeState; } set { _activeState = value; } }
    public bool Selected { get { return _selected; } set { _selected = value; } }

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