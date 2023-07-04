using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Item SO", menuName = "Scriptable Object/Equipment/Equipment Item SO")]
public class EquipmentItemSO : ItemSO
{
	// Put this on weapon subclass?
	public int Damage;

	// Put this on armor subclass? 
	public int Defense;

	// Craftable stuff. 
	public bool Craftable = false;
	[NaughtyAttributes.ShowIf("Craftable")]
	public List<CraftingItemAmount> CraftingCost = new List<CraftingItemAmount>();

	// Gets called when you click on the item in inventory. 
	public override void Use()
	{
		// Equip item. 
		Debug.Log("Equipped " + name);
	}
}