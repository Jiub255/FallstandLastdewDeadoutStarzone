using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Craftable Equipment Item SO", menuName = "Scriptable Object/Equipment/Craftable Equipment Item SO")]
public class CraftableEquipmentSO : EquipmentItemSO
{
	public List<CraftingItemAmount> CraftingCost = new List<CraftingItemAmount>();
}