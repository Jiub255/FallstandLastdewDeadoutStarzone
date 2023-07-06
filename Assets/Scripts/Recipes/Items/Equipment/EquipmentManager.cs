using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    [SerializeField]
    private Transform _weaponParent;
    [SerializeField]
    private SOEquipmentType _weaponEquipmentType;

    // So StatManager can get equipment list. 
    public Equipment Equipment { get; private set; }

    private void OnEnable()
    {
        SOEquipmentItem.OnEquip += Equip;
        SOEquipmentItem.OnUnequip += Unequip;

        Equipment = new();
    }

    private void OnDisable()
    {
        SOEquipmentItem.OnEquip -= Equip;
        SOEquipmentItem.OnUnequip -= Unequip;
    }

    public void Equip(SOEquipmentItem newItem)
    {
        Equipment.Equip(newItem);

        if (newItem.EquipmentType == _weaponEquipmentType)
        {
            // Destroy old weapon object. 
            Destroy(_weaponParent.GetChild(0).gameObject);

            // Instantiate new weapon object
            GameObject weapon = Instantiate(newItem.EquipmentItemPrefab, _weaponParent);
            weapon.transform.localPosition = Vector3.zero;
        }
    }

    public void Unequip(SOEquipmentItem equipmentItem)
    {
        Equipment.Unequip(equipmentItem);

        // Destroy old weapon object. 
        Destroy(_weaponParent.GetChild(0).gameObject);
    }
}