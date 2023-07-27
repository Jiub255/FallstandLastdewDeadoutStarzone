using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Item SO", menuName = "Items/SOWeaponItem")]
public class SOWeaponItem : SOEquipmentItem
{
    [SerializeField, Header("------------- Weapon Data --------------")]
    private int _weaponDamage;
    [SerializeField]
    private float _attackRange;
    [SerializeField]
    private float _attacksPerSecond;

    public override EquipmentType EquipmentType { get { return EquipmentType.Weapon; } }
    /// <summary>
    /// This is only the weapon's attack stat, get the total attack stat from SOPCData.Attack(); 
    /// </summary>
    public int WeaponDamage { get { return _weaponDamage; } }
    public float AttackRange { get { return _attackRange; } }
    public float AttackPerSecond { get { return _attacksPerSecond; } }
}