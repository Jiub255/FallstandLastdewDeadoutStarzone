using System.Collections.Generic;
using UnityEngine;

// TODO: Make each of the different inventories have a list of ItemAmounts, but then have Add/Remove methods in the SO 
// which stop you from adding the wrong subtype of ItemAmount. 
[CreateAssetMenu(fileName = "New Build Inventory SO", menuName = "Scriptable Object/Building/Build Inventory SO")]
public class BuildInventorySO : ScriptableObject, IInventory
{
	public List<BuildItemSO> BuildItemSOs = new List<BuildItemSO>();
}