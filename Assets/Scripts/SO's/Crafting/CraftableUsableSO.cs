using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Craftable Usable Item SO", menuName = "Scriptable Object/Usable/Craftable Usable Item SO")]
public class CraftableUsableSO : UsableItemSO
{
	public List<CraftingItemAmount> CraftingCost = new List<CraftingItemAmount>();
}