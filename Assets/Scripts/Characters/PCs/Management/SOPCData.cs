using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/SOPCData", fileName = "New PC Data SO")]
public class SOPCData : SaveableSO
{
    /// <summary>
    /// PCSelector listens, handles click (double or single click). 
    /// </summary>
//    public static event Action<GameObject> OnClickPCIcon;
    public event Action OnClickPCIcon;

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

    [SerializeField, Header("These variables DO change during runtime")]
    private Stats _stats;
    [SerializeField]
    private Equipment _equipment = new();

    // These don't change during runtime. 
    public Sprite Icon { get { return _icon; } }
    public Sprite CharacterImage { get { return _characterImage; } }
    public GameObject PCPrefab { get { return _pcPrefab; } }
    public SOPCMovementState PCMovementStateSO { get { return _pcMovementStateSO;} }
    public SOPCSharedData PCSharedDataSO { get { return _pcSharedDataSO; } }

    // These do change during runtime. 
    public int Injury { get; set; }
    public int Relief { get; set; }
    public int Pain { get; set; }
    public bool Healing { get; set; }
    public Stats Stats { get { return _stats; } }
    public Equipment Equipment { get { return _equipment; } }
    public GameObject PCInstance { get; set; }
    public PCState ActiveState { get; set; }
    public bool Selected { get; set; }
    public PCController PCController { get; set; }
    public SelectedPCIcon SelectedPCIcon { get; set; }

    public int WeaponDamage()
    {
        int weaponAttack = Equipment.Weapon().WeaponDamage;
        int attackStat = Stats[StatType.Attack].ModdedValue;
        // TODO - Probably use different formula eventually. 
        return weaponAttack + attackStat;
    }

    /// <summary>
    /// Called by clicking PC icon button. 
    /// </summary>
    public void SelectPC()
    {
        // PCSelector hears this. Selects this PC for now. Center camera on double click later. 
        OnClickPCIcon?.Invoke(/*PCInstance*/);
    }

    public override void SaveData(GameData gameData)
    {
        throw new NotImplementedException();
    }

    public override void LoadData(GameData gameData)
    {
        throw new NotImplementedException();
    }
}