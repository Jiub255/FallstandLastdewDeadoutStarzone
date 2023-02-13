using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Item SO", menuName = "Scriptable Object/Equipment/Equipment Item SO")]
public class EquipmentItemSO : ItemSO
{
	// Put this on weapon subclass?
	public int damage;

	// Put this on armor subclass? 
	public int defense;

	public override void Use()
    {

    }
}