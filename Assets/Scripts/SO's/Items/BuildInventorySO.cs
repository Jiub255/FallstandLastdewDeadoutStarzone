using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildInventorySO", menuName = "Items/BuildInventorySO")]
public class BuildInventorySO : ScriptableObject
{
	public List<BuildingItemSO> BuildItems = new List<BuildingItemSO>();
}