using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Build Inventory SO", menuName = "Scriptable Object/Building/Build Inventory SO")]
public class BuildInventorySO : ScriptableObject, IInventory
{
	public List<BuildItemSO> BuildItemSOs = new List<BuildItemSO>();
}