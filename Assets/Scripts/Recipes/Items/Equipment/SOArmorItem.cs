using UnityEngine;

[CreateAssetMenu(fileName = "New Armor Item SO", menuName = "Items/SOArmorItem")]
public class SOArmorItem : SOEquipmentItem
{
    [SerializeField, Header("-------------- Armor Data --------------")]
    private int _defense;

    public override EquipmentType EquipmentType
    {
        get
        {
            if (_equipmentType == EquipmentType.Weapon)
            {
                Debug.LogWarning($"{name} is an armor item with \"Weapon\" equipment type. Change its equipment type. ");
            }
            return _equipmentType;
        }
    }

    public int Defense { get { return _defense; } }
}